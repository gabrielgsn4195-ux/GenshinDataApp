# 🎯 Solución: Gestión de JWT en Swagger UI

## 📌 Problema Identificado

En .NET 10 con Swashbuckle.AspNetCore 10.x existe un conflicto de paquetes que impide configurar el botón "Authorize" estándar de Swagger:

- **Microsoft.OpenApi 2.x** (requerido por .NET 10) cambió su API completamente
- **Swashbuckle 10.x** aún usa patrones de configuración incompatibles con OpenApi 2.x
- Las clases `OpenApiSecurityScheme` y `OpenApiSecurityRequirement` son ahora interfaces inmutables
- Los namespace y propiedades cambiaron (ej: `Microsoft.OpenApi.Models` ya no existe)

### Intentos Fallidos

1. ❌ Configuración con `AddSecurityDefinition` tradicional → Error CS0144 (no se puede instanciar interfaz)
2. ❌ Uso de `Microsoft.OpenApi.Models` → Error CS0234 (namespace no existe)
3. ❌ Referencias explícitas a Microsoft.OpenApi 1.6.x → Error NU1605 (downgrade detectado)
4. ❌ Delegados/Lambdas en AddSecurityDefinition → API incompatible

## ✅ Solución Implementada

**Enfoque pragmático:** Interceptor de peticiones + UI personalizada

### Componentes

1. **Request Interceptor de Swagger UI**
   ```javascript
   (req) => {
       const token = localStorage.getItem('jwt_token');
       if (token) {
           req.headers['Authorization'] = 'Bearer ' + token;
       }
       return req;
   }
   ```

2. **Botones personalizados en la UI**
   - 🔑 **Set JWT Token**: Prompt para ingresar token, se guarda en localStorage
   - 🗑️ **Clear Token**: Elimina el token del localStorage
   - ✓ **Token activo**: Indicador visual cuando hay token guardado

3. **Persistencia de autorización**
   ```csharp
   c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
   ```

### Código en Program.cs

```csharp
app.UseSwaggerUI(c =>
{
    c.SwaggerEndpoint("/swagger/v1/swagger.json", "GenshinDataApp API V1");
    c.DocumentTitle = "GenshinDataApp API";

    // Interceptor que inyecta el header Authorization automáticamente
    c.UseRequestInterceptor("(req) => { const token = localStorage.getItem('jwt_token'); if (token) { req.headers['Authorization'] = 'Bearer ' + token; } return req; }");
    
    // Persistir estado de autorización
    c.ConfigObject.AdditionalItems["persistAuthorization"] = true;
    
    // Botones personalizados en la UI (HTML/JS en HeadContent)
    c.HeadContent = @"
        <script>
            function setJwtToken() {
                const token = prompt('Ingrese el JWT token (sin Bearer):');
                if (token) {
                    localStorage.setItem('jwt_token', token);
                    alert('Token guardado. Las peticiones ahora incluirán el header Authorization.');
                    location.reload();
                }
            }
            // ... más código
        </script>";
});
```

## 🎯 Ventajas

✅ **Sin dependencias adicionales**: Solo configuración de Swagger UI
✅ **Funciona con .NET 10 + Swashbuckle 10.x**: Sin conflictos de paquetes
✅ **Experiencia de usuario mejorada**: Botones visibles, token persistente
✅ **Automático**: Una vez configurado, el token se añade a todas las peticiones
✅ **Compatible**: Funciona en todos los navegadores modernos (localStorage)

## 📝 Uso

### Flujo de Testing

1. Ejecuta la API → Swagger UI se abre
2. **Registra/Login** → Copia el `accessToken` de la respuesta
3. Haz clic en **🔑 Set JWT Token** → Pega el token → Acepta
4. Observa el indicador **✓ Token activo** en la barra superior
5. **Prueba endpoints protegidos** → El header se añade automáticamente
6. Cuando el token expire (15 min) → Usa `/api/auth/refresh` → Repite paso 3
7. Al terminar → Haz clic en **🗑️ Clear Token**

### Verificación

Abre DevTools (F12) → Pestaña **Network** → Ejecuta una petición protegida → Inspecciona Headers:

```
Request Headers:
  Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

## 🔮 Futuro

Cuando Swashbuckle se actualice para soportar completamente Microsoft.OpenApi 2.x, se podrá migrar al botón "Authorize" estándar usando:

```csharp
options.AddSecurityDefinition("Bearer", /* configuración con nueva API */);
options.AddSecurityRequirement(/* requirements con nueva API */);
```

Por ahora, esta solución proporciona la misma funcionalidad con mejor UX.

## 📚 Referencias

- [Swashbuckle.AspNetCore](https://github.com/domaindrivendev/Swashbuckle.AspNetCore) v10.1.4
- [SwaggerUI Request Interceptors](https://swagger.io/docs/open-source-tools/swagger-ui/usage/configuration/)
- [TESTING-GUIDE-JWT.md](./TESTING-GUIDE-JWT.md) - Guía completa de uso
