# 🧪 Guía de Testing - API con JWT

## 📋 Tabla de Contenidos
1. [Prerrequisitos](#prerrequisitos)
2. [Ejecutar la API](#ejecutar-la-api)
3. [Testing con Swagger UI (Gestión de Token Integrada)](#testing-con-swagger-ui-gestión-de-token-integrada) ⭐ **NUEVO**
4. [Testing con Postman/Thunder Client](#testing-con-postmanthunder-client)
5. [Verificación en Base de Datos](#verificación-en-base-de-datos)

---

## 🔧 Prerrequisitos

- ✅ API compilada correctamente
- ✅ Base de datos GenshinDataApp creada
- ✅ Stored procedures ejecutados
- ✅ Visual Studio o .NET SDK instalado

---

## 🚀 Ejecutar la API

### Opción 1: Visual Studio
1. Abre el proyecto `GenshinDataApp.Services`
2. Presiona `F5` o clic en ▶️ **GenshinDataApp.Services**
3. Se abrirá el navegador en `https://localhost:7267/swagger`

### Opción 2: Línea de comandos
```bash
cd GenshinDataApp.Services
dotnet run
```

La API estará disponible en:
- 🔒 HTTPS: `https://localhost:7267`
- 🔓 HTTP: `http://localhost:5153`

---

## 🎯 Testing con Swagger UI (Gestión Automática de Token) ⭐ RECOMENDADO

### Nueva Funcionalidad - Botones de Gestión de Token

La interfaz de Swagger ahora incluye botones personalizados en la barra superior:

- **🔑 Set JWT Token** - Guardar tu token JWT
- **🗑️ Clear Token** - Eliminar el token guardado  
- **✓ Token activo** - Indicador visual cuando hay un token guardado

**✅ VENTAJA:** Una vez guardado el token, se añade automáticamente a TODAS las peticiones.

### Paso 1: Registrar un usuario

**Endpoint:** `POST /api/auth/register`

1. Expande el endpoint en Swagger
2. Haz clic en **Try it out**
3. Ingresa el body:

```json
{
  "email": "test@example.com",
  "password": "Test123!@#",
  "username": "TestUser"
}
```

4. Haz clic en **Execute**

**Response esperado (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64encodedstring...",
  "expiresIn": 900
}
```

**⚠️ IMPORTANTE:** Copia SOLO el valor de `accessToken` (sin comillas, sin "Bearer").

---

### Paso 2: Guardar el Token

1. En la **barra superior** de Swagger UI, haz clic en **🔑 Set JWT Token**
2. Pega el `accessToken` que copiaste (SIN "Bearer")
3. Haz clic en **Aceptar**
4. Verás aparecer el indicador **✓ Token activo**
5. La página se recargará automáticamente

**✅ LISTO:** Todas las peticiones ahora incluirán automáticamente `Authorization: Bearer {token}`

---

### Paso 3: Probar Endpoints Protegidos

#### **GET /api/users/me** (Obtener perfil del usuario autenticado)

1. Expande el endpoint
2. Haz clic en **Try it out**
3. Haz clic en **Execute** (el token se añade automáticamente)

**Response esperado (200 OK):**
```json
{
  "publicId": "12345678-1234-1234-1234-123456789abc",
  "email": "test@example.com",
  "username": "TestUser",
  "userCode": "XY7K9M2N",
  "avatarPath": null,
  "isEmailVerified": false,
  "role": "User"
}
```

#### **GET /api/users/{id}** (Obtener usuario por PublicId)

1. Expande el endpoint
2. Haz clic en **Try it out**
3. Ingresa el `publicId` del usuario (copia del response anterior)
4. Haz clic en **Execute**

#### **PUT /api/users/{id}** (Actualizar usuario)

1. Expande el endpoint
2. Haz clic en **Try it out**
3. Ingresa el `publicId` en el parámetro
4. Modifica el body:

```json
{
  "username": "UpdatedUser",
  "email": "updated@example.com"
}
```

5. Haz clic en **Execute**

---

### Paso 4: Refrescar Token Expirado

Los access tokens expiran en 15 minutos. Si recibes error 401:

**Endpoint:** `POST /api/auth/refresh`

1. Expande el endpoint
2. Haz clic en **Try it out**
3. Ingresa el body con tu `refreshToken`:

```json
{
  "refreshToken": "tu_refresh_token_aqui"
}
```

4. Haz clic en **Execute**
5. Copia el nuevo `accessToken` de la respuesta
6. Haz clic en **🔑 Set JWT Token** nuevamente
7. Pega el nuevo token

---

### Paso 5: Cerrar Sesión

#### Opción A: Logout vía API

**Endpoint:** `POST /api/auth/logout`

1. Expande el endpoint
2. Haz clic en **Try it out**
3. Ingresa el body:

```json
{
  "refreshToken": "tu_refresh_token_aqui"
}
```

4. Haz clic en **Execute**
5. Haz clic en **🗑️ Clear Token** para limpiar el navegador

#### Opción B: Solo limpiar token del navegador

1. Haz clic en **🗑️ Clear Token**
2. Confirma la acción
3. La página se recargará y el token será eliminado

---

### ⚠️ Troubleshooting

**❌ No veo los botones 🔑 y 🗑️**
- Presiona `Ctrl + F5` para recargar sin caché
- Verifica que estés en modo Development (puerto 7267)
- Abre las DevTools del navegador (F12) y busca errores en Console

**❌ Error 401 - Unauthorized**
- Verifica que el indicador **✓ Token activo** esté visible
- Tu token expiró (15 min) - usa `/api/auth/refresh`
- Copiaste el token mal (sin espacios ni comillas)

**❌ Error 403 - Forbidden**
- Intentas acceder a recursos de otro usuario
- Tu cuenta está desactivada (IsActive = false)

**🔄 El token se borra al recargar la página**
- El token se guarda en `localStorage` del navegador
- Si limpias cookies/caché, deberás volver a configurarlo
- Los botones persisten el token entre recargas normales

---

## 📝 Testing con Swagger (Manual)

⚠️ **NOTA:** En .NET 10 con Swashbuckle 10.x, el botón "Authorize" no está disponible por defecto debido a incompatibilidades de paquetes. Debes agregar el header `Authorization` manualmente en cada request.

### ✅ Paso 1: Registrar un usuario

**Endpoint:** `POST /api/auth/register`

**Request Body:**
```json
{
  "email": "test@example.com",
  "password": "Test123!@#",
  "username": "TestUser"
}
```

**Response esperado (200 OK):**
```json
{
  "accessToken": "eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...",
  "refreshToken": "base64encodedstring...",
  "expiresIn": 900
}
```

**⚠️ IMPORTANTE:** Copia el `accessToken` completo para los siguientes pasos.

---

### 🔐 Paso 2: Probar endpoints protegidos (con header manual)

#### **GET /api/users/me** (Obtener perfil actual)

**En Swagger:**

1. Expande el endpoint `GET /api/users/me`
2. Haz clic en **Try it out**
3. En la sección de **Parameters**, haz clic en **Add String Item** o busca donde agregar headers
4. Agrega manualmente:
   - **Parameter name:** `Authorization`
   - **Value:** `Bearer TU_ACCESS_TOKEN_AQUI`
   
   **Ejemplo completo:**
   ```
   Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9.eyJzdWIiOiJhYmNk...
   ```

5. Haz clic en **Execute**

**⚠️ ALTERNATIVA MÁS FÁCIL:** Usa Postman, Thunder Client o `curl` en lugar de Swagger para testing con JWT.

---

## 🧪 Testing con Postman/Thunder Client (RECOMENDADO)

### Configuración de Authorization

1. **Crea una nueva request** en Postman
2. Selecciona el tab **Authorization**
3. Tipo: **Bearer Token**
4. Pega tu `accessToken` en el campo **Token**
5. Postman agregará automáticamente el header `Authorization: Bearer <token>`

### Ejemplos de Requests:

#### 1. **POST** Registrar Usuario
```http
POST https://localhost:7267/api/auth/register
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!@#",
  "username": "TestUser"
}
```

#### 2. **POST** Login
```http
POST https://localhost:7267/api/auth/login
Content-Type: application/json

{
  "email": "test@example.com",
  "password": "Test123!@#"
}
```

#### 3. **GET** Perfil Actual (requiere JWT)
```http
GET https://localhost:7267/api/users/me
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### 4. **GET** Listar Usuarios (requiere JWT)
```http
GET https://localhost:7267/api/users
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
```

#### 5. **POST** Refresh Token
```http
POST https://localhost:7267/api/auth/refresh
Content-Type: application/json

{
  "refreshToken": "TU_REFRESH_TOKEN_AQUI"
}
```

#### 6. **POST** Logout (requiere JWT)
```http
POST https://localhost:7267/api/auth/logout
Authorization: Bearer eyJhbGciOiJIUzI1NiIsInR5cCI6IkpXVCJ9...
Content-Type: application/json

{
  "refreshToken": "TU_REFRESH_TOKEN"
}
```

---

## 🗄️ Verificación en Base de Datos

### Ver usuarios creados
```sql
SELECT 
    PublicId,
    Email,
    Username,
    UserCode,
    AuthProvider,
    IsEmailVerified,
    IsActive,
    Role
FROM [User]
WHERE IsActive = 1;
```

### Ver refresh tokens activos
```sql
SELECT 
    rt.PublicId,
    u.Email,
    rt.Token,
    rt.ExpiresAt,
    rt.DeviceFingerprint,
    rt.IsActive,
    rt.CreatedAt
FROM [RefreshToken] rt
INNER JOIN [User] u ON rt.UserId = u.Id
WHERE rt.IsActive = 1
ORDER BY rt.CreatedAt DESC;
```

### Ver tokens revocados (después de logout)
```sql
SELECT 
    rt.PublicId,
    u.Email,
    rt.IsActive,
    rt.CreatedAt
FROM [RefreshToken] rt
INNER JOIN [User] u ON rt.UserId = u.Id
WHERE rt.IsActive = 0
ORDER BY rt.CreatedAt DESC;
```

---

## ⚠️ Errores Comunes

### 401 Unauthorized
- ✅ Verifica que el token no haya expirado (dura 15 minutos)
- ✅ Asegúrate de incluir `Bearer` antes del token
- ✅ No debe haber espacios extra en el header

### 409 Conflict - "Email already registered"
- El email ya existe en la base de datos
- Usa otro email o elimina el usuario existente

### 500 Internal Server Error
- Revisa los logs en la consola de Visual Studio
- Verifica que la base de datos esté accesible
- Comprueba que los stored procedures estén creados

---

## 📊 Checklist de Testing Completo

- [ ] ✅ POST /api/auth/register - Usuario creado
- [ ] ✅ POST /api/auth/login - Login exitoso
- [ ] ✅ GET /api/users/me - Perfil obtenido con token válido
- [ ] ✅ GET /api/users - Lista de usuarios (requiere autorización)
- [ ] ✅ POST /api/auth/refresh - Tokens renovados
- [ ] ✅ POST /api/auth/logout - Sesión cerrada
- [ ] ✅ GET /api/users/me con token expirado - Recibe 401
- [ ] ✅ Registro con email duplicado - Recibe 409
- [ ] ✅ Login con credenciales incorrectas - Recibe 401

---

## 🎯 Próximos Pasos

Una vez que hayas validado que todo funciona:

1. **Implementar validación de datos** (FluentValidation)
2. **Agregar verificación de email**
3. **Implementar reset de password**
4. **Configurar rate limiting**
5. **Agregar refresh token rotation**
6. **Implementar OAuth providers** (Google, Apple, Twitch)

---

**¿Problemas?** Revisa los logs en `GenshinDataApp.Services/logs/genshindata-<fecha>.log`
