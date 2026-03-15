## 🚀 Ejecutar API con Swagger

### Opción 1: Visual Studio
1. Abrir `GenshinDataApp.sln` en Visual Studio
2. Establecer `GenshinDataApp.Services` como proyecto de inicio
3. Presionar **F5** o click en ▶️
4. Swagger UI se abrirá automáticamente

### Opción 2: Terminal
```bash
cd GenshinDataApp.Services
dotnet run
```

Luego abrir en el navegador:
- **Swagger UI**: https://localhost:5001/swagger
- **API Base URL**: https://localhost:5001/api

---

## 🧪 Guía de Pruebas con Swagger

### 1️⃣ Registrar un Usuario
**Endpoint:** `POST /api/auth/register`

**Body (JSON):**
```json
{
  "email": "test@example.com",
  "password": "Test123!@#",
  "username": "testuser"
}
```

**Respuesta esperada (200 OK):**
```json
{
  "publicId": "guid-aqui",
  "email": "test@example.com",
  "username": "testuser",
  "userCode": "A1B2C3D4",
  "role": "User",
  "isEmailVerified": false,
  "accessToken": "eyJhbGciOi...",
  "refreshToken": "base64-token...",
  "expiresAt": "2025-01-21T12:30:00Z"
}
```

**📋 Copia el `accessToken` para usarlo después**

---

### 2️⃣ Login
**Endpoint:** `POST /api/auth/login`

**Body (JSON):**
```json
{
  "email": "test@example.com",
  "password": "Test123!@#"
}
```

**Respuesta:** Igual que register (nuevos tokens)

---

### 3️⃣ Acceder a Endpoint Protegido

**Paso A: Autorizar en Swagger**
1. Click en el botón **🔒 Authorize** (arriba a la derecha en Swagger UI)
2. Pegar el `accessToken` en el campo "Value"
3. Formato: `Bearer {tu-access-token-aqui}`
4. Click en **Authorize** → **Close**

**Paso B: Probar endpoint protegido**
**Endpoint:** `GET /api/users/me`

**Headers (automático):**
```
Authorization: Bearer eyJhbGciOi...
```

**Respuesta esperada (200 OK):**
```json
{
  "publicId": "guid-aqui",
  "email": "test@example.com",
  "username": "testuser",
  "userCode": "A1B2C3D4",
  "avatarPath": null,
  "isEmailVerified": false,
  "role": "User",
  "authProvider": "local"
}
```

**Si el token está expirado → 401 Unauthorized**

---

### 4️⃣ Refresh Token
**Endpoint:** `POST /api/auth/refresh`

**Body (JSON):**
```json
{
  "accessToken": "token-expirado-aqui",
  "refreshToken": "refresh-token-aqui"
}
```

**Respuesta:** Nuevos tokens

---

### 5️⃣ Logout
**Endpoint:** `POST /api/auth/logout` (requiere `[Authorize]`)

**Body (JSON):**
```json
{
  "accessToken": "token-actual",
  "refreshToken": "refresh-token-a-revocar"
}
```

**Respuesta (200 OK):**
```json
{
  "message": "Logged out successfully"
}
```

---

## ❌ Errores Comunes

### Error 401 Unauthorized
- **Causa:** Token inválido o expirado
- **Solución:** Hacer login nuevamente o usar refresh token

### Error 409 Conflict (Register)
- **Causa:** Email o username ya existe
- **Solución:** Usar otro email/username

### Error 400 Bad Request
- **Causa:** JSON malformado o campos vacíos
- **Solución:** Verificar que todos los campos requeridos estén presentes

### Error 500 Internal Server Error
- **Causa:** Problema con la base de datos o configuración
- **Solución:** Verificar logs en `logs/genshindata-YYYYMMDD.log`

---

## 🔍 Verificar en Base de Datos

Puedes verificar que los usuarios se crearon correctamente ejecutando en SSMS:

```sql
-- Ver usuarios creados
SELECT PublicId, Email, Username, UserCode, Role, IsEmailVerified, IsActive
FROM [User]
WHERE IsActive = 1;

-- Ver refresh tokens activos
SELECT rt.PublicId, u.Email, rt.ExpiresAt, rt.IsActive
FROM RefreshToken rt
INNER JOIN [User] u ON rt.UserId = u.Id
WHERE rt.IsActive = 1;
```

---

## 📝 Notas Importantes

1. **Access Token expira en 15 minutos** (configurable en `appsettings.Development.json`)
2. **Refresh Token expira en 7 días** (hardcoded en `AuthController.cs`)
3. **Refresh Token rotation**: Al renovar, el viejo se revoca automáticamente
4. **Password hash**: BCrypt con workFactor 12 (seguro pero lento ~300ms)
5. **UserCode**: Generado automáticamente (8 caracteres alfanuméricos únicos)

---

## 🎯 Flujo Completo Recomendado

1. ✅ **Register** → Obtener tokens
2. ✅ **Autorizar** en Swagger con el accessToken
3. ✅ **GET /api/users/me** → Verificar autenticación
4. ✅ **GET /api/users** → Listar usuarios (protegido)
5. ✅ **Esperar 15 minutos** (o cambiar ExpiryMinutes a 1 para pruebas rápidas)
6. ✅ **Refresh** → Renovar tokens
7. ✅ **Logout** → Revocar refresh token
