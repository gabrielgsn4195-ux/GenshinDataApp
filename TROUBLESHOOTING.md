# 🔧 Troubleshooting Guide - GenshinDataApp

> **Última actualización:** 2025-01-13 (Sesión 5)  
> **Propósito:** Documentar errores comunes y sus soluciones para evitar repetición

---

## 📋 Índice

1. [Autenticación JWT](#autenticación-jwt)
2. [Entity Framework Core](#entity-framework-core)
3. [Swagger UI](#swagger-ui)
4. [SQL Server & Stored Procedures](#sql-server--stored-procedures)
5. [ASP.NET Core Controllers](#aspnet-core-controllers)

---

## 🔐 Autenticación JWT

### Error 1: Claims no encontrados - Unauthorized 401

**Síntoma:**
```
GET /api/users/me → 401 Unauthorized
{ "message": "Invalid token" }
```

**Causa raíz:**  
.NET Core automáticamente **mapea** el claim `"sub"` del JWT a `ClaimTypes.NameIdentifier` durante la deserialización del token. El código buscaba solo `JwtRegisteredClaimNames.Sub`, que ya no existe en `User.Claims`.

**Solución:**
```csharp
// ❌ INCORRECTO - Solo busca "sub"
var publicIdClaim = User.Claims
    .FirstOrDefault(c => c.Type == JwtRegisteredClaimNames.Sub)?.Value;

// ✅ CORRECTO - Busca en ambos tipos
var publicIdClaim = User.Claims.FirstOrDefault(c => 
    c.Type == System.Security.Claims.ClaimTypes.NameIdentifier || 
    c.Type == System.IdentityModel.Tokens.Jwt.JwtRegisteredClaimNames.Sub)?.Value;
```

**Archivos afectados:**
- `UsersController.cs` - GetCurrentUser()
- `AuthController.cs` - Refresh()

**Prevención:**  
Siempre buscar claims en **ambos formatos** cuando uses JWT en .NET Core.

---

### Error 2: Token no persiste entre recargas de página

**Síntoma:**  
Token se pierde al recargar Swagger UI, hay que volverlo a ingresar.

**Causa raíz:**  
El token se guarda en `localStorage`, pero `localStorage` es **específico por origen** (`scheme://host:port`).

**Solución:**  
Usar siempre el **mismo origen** (HTTPS):
```json
// launchSettings.json - Solo HTTPS
{
  "profiles": {
    "https": {
      "applicationUrl": "https://localhost:7267"
    }
  }
}
```

**Archivos afectados:**
- `launchSettings.json`

**Prevención:**  
En desarrollo, configurar **HTTPS-only** desde el inicio.

---

## 🗄️ Entity Framework Core

### Error 3: CancellationToken pasado como parámetro SQL

**Síntoma:**
```
System.InvalidOperationException: The current provider doesn't have a store 
type mapping for properties of type 'CancellationToken'.
```

**Causa raíz:**  
Al pasar parámetros a `ExecuteSqlRawAsync()`, EF usa sobrecarga de `params object[]`. Si pasas `param, ct` sin array, **ambos se interpretan como parámetros SQL**.

**Solución:**
```csharp
// ❌ INCORRECTO - ct se interpreta como parámetro SQL
await _context.Database.ExecuteSqlRawAsync(
    "EXEC SP_U_RefreshTokenRevoke @Token",
    tokenParam,
    ct);

// ✅ CORRECTO - ct se pasa explícitamente como CancellationToken
await _context.Database.ExecuteSqlRawAsync(
    "EXEC SP_U_RefreshTokenRevoke @Token",
    new[] { tokenParam },
    ct);
```

**Archivos afectados:**
- `RefreshTokenRepository.cs` - RevokeByTokenAsync()
- `RefreshTokenRepository.cs` - RevokeAllByUserIdAsync()

**Prevención:**  
Siempre **wrappear parámetros SQL en array** cuando uses `CancellationToken`.

---

## 📄 Swagger UI

### Error 4: Botón "Authorize" no funciona en .NET 10

**Síntoma:**  
El botón "Authorize" en Swagger UI no aparece o no funciona correctamente.

**Causa raíz:**  
.NET 10 con Swashbuckle 10.x tiene conflictos de versiones con `Microsoft.OpenApi`. La configuración estándar `AddSwaggerGen(c => c.AddSecurityDefinition(...))` no funciona.

**Solución:**  
Implementar **custom JWT toolbar** con request interceptor:

```csharp
app.UseSwaggerUI(c =>
{
    // Request interceptor que inyecta Authorization header
    var interceptorJs = "function(req){var t=localStorage.getItem('jwt_token');if(t&&req.url.indexOf('/api/')!==-1){req.headers={...req.headers,Authorization:'Bearer '+t};}return req;}";
    c.UseRequestInterceptor(interceptorJs);
    
    // Custom toolbar con botones Set/Clear Token
    c.HeadContent = @"..."; // HTML + CSS + JavaScript
});
```

**Archivos afectados:**
- `Program.cs` (líneas 95-215)

**Prevención:**  
En .NET 10, usar **custom solutions** para Swagger JWT en lugar de configuración estándar.

---

### Error 5: TypeError - Invalid value en fetch

**Síntoma:**
```javascript
TypeError: Failed to execute 'fetch' on 'Window': Invalid value
```

**Causa raíz:**  
El objeto `headers` en la request de Swagger es **inmutable**. Asignación directa `req.headers.Authorization = ...` falla.

**Solución:**
```javascript
// ❌ INCORRECTO - Asignación directa
req.headers.Authorization = 'Bearer ' + t;

// ✅ CORRECTO - Crear nuevo objeto con spread operator
req.headers = {...req.headers, Authorization: 'Bearer ' + t};
```

**Archivos afectados:**
- `Program.cs` - Request interceptor

**Prevención:**  
Usar **spread operator** para modificar objetos inmutables en JavaScript.

---

## 🗃️ SQL Server & Stored Procedures

### Error 6: Stored Procedure no encontrado

**Síntoma:**
```
Microsoft.Data.SqlClient.SqlException (0x80131904): 
Could not find stored procedure 'SP_U_RefreshTokenRevoke'.
```

**Causa raíz:**  
El stored procedure no existe en la base de datos. En este caso, los SPs de UPDATE (`SP_U_*`) no se crearon inicialmente.

**Solución:**
```sql
-- Crear el SP faltante
CREATE PROCEDURE SP_U_RefreshTokenRevoke
    @Token NVARCHAR(MAX)
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE RefreshToken SET IsActive = 0
    WHERE Token = @Token AND IsActive = 1;
END;
```

**Archivos afectados:**
- Nuevo: `Database/scripts/Create_RefreshToken_UpdateProcedures.sql`

**Prevención:**  
Crear **todos los SPs básicos** (SELECT, INSERT, UPDATE, DELETE) al crear una tabla.

---

### Error 7: UPDATE failed - QUOTED_IDENTIFIER

**Síntoma:**
```
UPDATE failed because the following SET options have incorrect settings: 
'QUOTED_IDENTIFIER'. Verify that SET options are correct...
```

**Causa raíz:**  
SQL Server requiere `QUOTED_IDENTIFIER ON` para tablas con índices, vistas indexadas, o columnas computadas.

**Solución:**
```sql
-- ❌ INCORRECTO - SET dentro del procedimiento
CREATE PROCEDURE SP_U_RefreshTokenRevoke
AS
BEGIN
    SET QUOTED_IDENTIFIER ON;  -- ❌ Muy tarde
    UPDATE ...
END;

-- ✅ CORRECTO - SET antes de CREATE PROCEDURE
SET QUOTED_IDENTIFIER ON;
GO

CREATE PROCEDURE SP_U_RefreshTokenRevoke
AS
BEGIN
    SET NOCOUNT ON;
    UPDATE ...
END;
GO
```

**Archivos afectados:**
- `Database/scripts/Create_RefreshToken_UpdateProcedures.sql`

**Prevención:**  
Siempre incluir `SET QUOTED_IDENTIFIER ON; GO` **antes** de `CREATE PROCEDURE`.

---

### Error 8: UpdatedAt column no existe

**Síntoma:**
```
Invalid column name 'UpdatedAt'.
```

**Causa raíz:**  
Intentar actualizar columna `UpdatedAt` que no existe en la tabla. Solo `CreatedAt` está presente.

**Solución:**
```sql
-- ❌ INCORRECTO - UpdatedAt no existe
UPDATE RefreshToken
SET IsActive = 0, UpdatedAt = GETUTCDATE()
WHERE Token = @Token;

-- ✅ CORRECTO - Solo actualizar columnas existentes
UPDATE RefreshToken
SET IsActive = 0
WHERE Token = @Token;
```

**Prevención:**  
Verificar **esquema real de la tabla** con:
```sql
SELECT COLUMN_NAME, DATA_TYPE 
FROM INFORMATION_SCHEMA.COLUMNS 
WHERE TABLE_NAME = 'RefreshToken'
ORDER BY ORDINAL_POSITION;
```

---

## 🎮 ASP.NET Core Controllers

### Error 9: Request reached end of middleware pipeline

**Síntoma:**
```
Request reached the end of the middleware pipeline without being handled 
by application code. Request path: GET /api/users
```

**Causa raíz:**  
Aunque este error no se confirmó en nuestro caso, puede ocurrir si:
1. Orden incorrecto de rutas
2. Constraint mal configurado
3. Middleware faltante

**Solución:**
Ordenar rutas de **más específica a más genérica**:

```csharp
// ✅ CORRECTO - Orden: específica → parámetro → genérica
[HttpGet("me")]                    // 1. Más específica
public async Task<IActionResult> GetCurrentUser() { ... }

[HttpGet("{publicId:guid}")]       // 2. Con parámetro
public async Task<IActionResult> GetUserByPublicId(Guid publicId) { ... }

[HttpGet]                          // 3. Más genérica
public async Task<IActionResult> GetAllUsers() { ... }
```

**Archivos afectados:**
- `UsersController.cs`

**Prevención:**  
Seguir **best practice** de routing: específicas primero.

---

## 📚 Resumen de Lecciones Aprendidas

### ✅ Buenas Prácticas Implementadas

1. **Claim Mapping**: Siempre buscar en ambos formatos (NameIdentifier y Sub)
2. **HTTPS-only**: Configurar desde el inicio para evitar problemas de localStorage
3. **Array Wrapping**: Parámetros SQL siempre en array cuando se usa CancellationToken
4. **QUOTED_IDENTIFIER**: Incluir antes de CREATE PROCEDURE
5. **Route Ordering**: Específicas antes que genéricas
6. **Spread Operator**: Para objetos inmutables en JavaScript
7. **Custom Solutions**: En .NET 10, no asumir que configuraciones estándar funcionan

### 🚨 Errores a Evitar

1. ❌ Buscar solo `JwtRegisteredClaimNames.Sub` en claims
2. ❌ Usar HTTP y HTTPS simultáneamente en development
3. ❌ Pasar `param, ct` sin array a `ExecuteSqlRawAsync`
4. ❌ Asumir que todas las columnas existen sin verificar esquema
5. ❌ Crear tabla sin sus stored procedures UPDATE
6. ❌ Asignar directamente a objetos inmutables en JavaScript
7. ❌ Usar configuración estándar de Swagger en .NET 10 sin probar

### 📝 Checklist para Nuevas Features

Cuando implementes autenticación/autorización:
- [ ] Claims: Buscar en ambos formatos
- [ ] Logging: Agregar logs detallados para debugging
- [ ] HTTPS: Configurar solo HTTPS en launchSettings
- [ ] Stored Procedures: Crear SELECT, INSERT, UPDATE, DELETE
- [ ] Parámetros: Wrappear en array cuando uses CancellationToken
- [ ] Testing: Probar todos los endpoints en Swagger
- [ ] Documentation: Actualizar PROJECT-STATUS.md

---

**Última actualización:** 2025-01-13  
**Sesiones documentadas:** Sesión 5 (JWT + Swagger Integration)
