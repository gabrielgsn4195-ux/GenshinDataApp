# 🗄️ GenshinDataApp - Database Reference

> **Última actualización:** 2025-01-21  
> **Base de datos:** GenshinDataApp  
> **Motor:** Microsoft SQL Server  
> **Total Tablas:** 20+ (con _History)  
> **Total Stored Procedures:** 85

---

## 📋 Índice Rápido

- [Convenciones](#-convenciones)
- [Tablas de Autenticación](#-tablas-de-autenticación)
- [Tablas de Catálogos](#-tablas-de-catálogos)
- [Tablas de Inventario del Usuario](#-tablas-de-inventario-del-usuario)
- [Stored Procedures](#-stored-procedures-85-total)

---

## 🎨 Convenciones

### Campos Obligatorios en TODAS las Tablas

```sql
Id          INT IDENTITY(1,1) PRIMARY KEY,
PublicId    UNIQUEIDENTIFIER DEFAULT NEWID() UNIQUE,
IsActive    BIT DEFAULT 1
```

### Tablas _History (Auditoría)

Cada tabla tiene su tabla `_History` con campos adicionales:

```sql
OperationType   NVARCHAR(10),  -- INSERT, UPDATE, DELETE
OperationDate   DATETIME DEFAULT GETDATE(),
OperationUser   NVARCHAR(256) DEFAULT SYSTEM_USER
```

### Triggers de Auditoría

Convención: `TR_{TableName}_History`

---

## 🔐 Tablas de Autenticación

### User

**Propósito:** Gestión de usuarios con autenticación local y OAuth

**Columnas principales:**
```sql
Id                          INT IDENTITY PRIMARY KEY
PublicId                    UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive                    BIT DEFAULT 1
Email                       NVARCHAR(255) UNIQUE NOT NULL
Username                    NVARCHAR(50) UNIQUE NOT NULL
UserCode                    NVARCHAR(20) UNIQUE NOT NULL  -- 'user#1234' format
PasswordHash                NVARCHAR(256)                  -- NULL for OAuth users
AuthProvider                NVARCHAR(50) DEFAULT 'Local'  -- Local, Google, Apple, Twitch
AuthProviderId              NVARCHAR(255)                  -- External provider ID
Role                        NVARCHAR(20) DEFAULT 'User'   -- User, Admin
IsEmailVerified             BIT DEFAULT 0
EmailVerificationToken      UNIQUEIDENTIFIER
EmailVerificationExpiry     DATETIME
PasswordResetToken          UNIQUEIDENTIFIER
PasswordResetExpiry         DATETIME
AvatarPath                  NVARCHAR(500)
Bio                         NVARCHAR(500)
UsernameLastChangedAt       DATETIME
CreatedAt                   DATETIME DEFAULT GETDATE()
```

**Constraints:**
- `CK_User_AuthProvider`: `AuthProvider IN ('Local','Google','Apple','Twitch')`
- `CK_User_Role`: `Role IN ('User','Admin')`
- `CK_User_LocalAuth`: Si `AuthProvider='Local'`, entonces `PasswordHash IS NOT NULL`

**Índices:**
- `IX_User_PublicId` (Unique)
- `IX_User_Email` (Unique, filtered `WHERE IsActive=1`)
- `IX_User_Username` (Unique, filtered `WHERE IsActive=1`)
- `IX_User_UserCode` (Unique, filtered `WHERE IsActive=1`)

---

### RefreshToken

**Propósito:** Tokens JWT para autenticación persistente

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive            BIT DEFAULT 1
UserId              INT NOT NULL  -- FK to User.Id
Token               NVARCHAR(500) UNIQUE NOT NULL
DeviceFingerprint   NVARCHAR(256)
ExpiresAt           DATETIME NOT NULL
CreatedAt           DATETIME DEFAULT GETDATE()
```

**Foreign Keys:**
- `FK_RefreshToken_User`: `UserId → User.Id`

**Índices:**
- `IX_RefreshToken_Token` (Unique, filtered `WHERE IsActive=1`)
- `IX_RefreshToken_UserId` (filtered `WHERE IsActive=1`)
- `IX_RefreshToken_ExpiresAt` (filtered `WHERE IsActive=1`)

---

### EmailRateLimit

**Propósito:** Control anti-abuso de envío de emails

**Columnas principales:**
```sql
Email               NVARCHAR(255) PRIMARY KEY
RequestCount        INT DEFAULT 1
FirstRequestAt      DATETIME DEFAULT GETDATE()
LastRequestAt       DATETIME DEFAULT GETDATE()
```

**⚠️ EXCEPCIÓN:** Esta tabla NO tiene `PublicId`, `IsActive`, ni tabla `_History`

**Constraints:**
- `CK_EmailRateLimit_Count`: `RequestCount >= 1`
- Window: 60 minutos
- Max requests: 5

---

## 🎮 Tablas de Catálogos

### Element

**Propósito:** Elementos del juego (Pyro, Hydro, Electro, Anemo, Geo, Dendro, Cryo)

**Columnas:**
```sql
Id          INT IDENTITY PRIMARY KEY
PublicId    UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive    BIT DEFAULT 1
Name        NVARCHAR(50) UNIQUE NOT NULL
IconPath    NVARCHAR(500)
```

**Valores esperados:** Pyro, Hydro, Electro, Anemo, Geo, Dendro, Cryo

---

### WeaponType

**Propósito:** Tipos de armas (Sword, Claymore, Polearm, Bow, Catalyst)

**Columnas:**
```sql
Id          INT IDENTITY PRIMARY KEY
PublicId    UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive    BIT DEFAULT 1
Name        NVARCHAR(50) UNIQUE NOT NULL
IconPath    NVARCHAR(500)
```

**Valores esperados:** Sword, Claymore, Polearm, Bow, Catalyst

---

### Region

**Propósito:** Regiones del juego (Mondstadt, Liyue, Inazuma, etc.)

**Columnas:**
```sql
Id          INT IDENTITY PRIMARY KEY
PublicId    UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive    BIT DEFAULT 1
Name        NVARCHAR(50) UNIQUE NOT NULL
IconPath    NVARCHAR(500)
```

**Valores esperados:** Mondstadt, Liyue, Inazuma, Sumeru, Fontaine, Natlan, Snezhnaya

---

### StatType

**Propósito:** Tipos de estadísticas (HP%, ATK%, CRIT DMG%, etc.)

**Columnas:**
```sql
Id          INT IDENTITY PRIMARY KEY
PublicId    UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive    BIT DEFAULT 1
Name        NVARCHAR(100) UNIQUE NOT NULL
```

**Valores esperados:**
- HP, HP%
- ATK, ATK%
- DEF, DEF%
- Energy Recharge%
- Elemental Mastery
- CRIT Rate%
- CRIT DMG%
- Healing Bonus%
- Elemental DMG Bonus% (per element)
- Physical DMG Bonus%

---

### Character

**Propósito:** Catálogo de personajes del juego

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive            BIT DEFAULT 1
Name                NVARCHAR(100) UNIQUE NOT NULL
Title               NVARCHAR(200)
Rarity              INT NOT NULL  -- 4 or 5 stars only
ElementId           INT NOT NULL  -- FK to Element.Id
WeaponTypeId        INT NOT NULL  -- FK to WeaponType.Id
RegionId            INT NOT NULL  -- FK to Region.Id
BaseHp              INT NOT NULL
BaseAtk             INT NOT NULL
BaseDef             INT NOT NULL
AscensionStatId     INT NOT NULL  -- FK to StatType.Id
AscensionStatValue  DECIMAL(10,2) NOT NULL
ImagePath           NVARCHAR(500)
ThumbnailPath       NVARCHAR(500)
IconPath            NVARCHAR(500)
```

**Constraints:**
- `CK_Character_Rarity`: `Rarity IN (4, 5)`

**Foreign Keys:**
- `FK_Character_Element`: `ElementId → Element.Id`
- `FK_Character_WeaponType`: `WeaponTypeId → WeaponType.Id`
- `FK_Character_Region`: `RegionId → Region.Id`
- `FK_Character_AscensionStat`: `AscensionStatId → StatType.Id`

---

### Weapon

**Propósito:** Catálogo de armas del juego

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive            BIT DEFAULT 1
Name                NVARCHAR(100) UNIQUE NOT NULL
WeaponTypeId        INT NOT NULL  -- FK to WeaponType.Id
Rarity              INT NOT NULL  -- 1-5 stars
BaseAtk             DECIMAL(10,2) NOT NULL
SecondaryStatId     INT           -- FK to StatType.Id (nullable)
SecondaryStatValue  DECIMAL(10,2) -- nullable
PassiveName         NVARCHAR(200)
ImagePath           NVARCHAR(500)
ThumbnailPath       NVARCHAR(500)
```

**Constraints:**
- `CK_Weapon_Rarity`: `Rarity BETWEEN 1 AND 5`

**Foreign Keys:**
- `FK_Weapon_WeaponType`: `WeaponTypeId → WeaponType.Id`
- `FK_Weapon_SecondaryStat`: `SecondaryStatId → StatType.Id`

---

### ArtifactSet

**Propósito:** Catálogo de sets de artefactos

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive            BIT DEFAULT 1
Name                NVARCHAR(100) UNIQUE NOT NULL
MaxRarity           INT NOT NULL
TwoPieceBonus       NVARCHAR(500)
FourPieceBonus      NVARCHAR(1000)
ImagePath           NVARCHAR(500)
```

**Constraints:**
- `CK_ArtifactSet_Rarity`: `MaxRarity BETWEEN 1 AND 5`

---

### ArtifactPiece

**Propósito:** Piezas individuales de cada set de artefactos

**Columnas principales:**
```sql
Id              INT IDENTITY PRIMARY KEY
PublicId        UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive        BIT DEFAULT 1
ArtifactSetId   INT NOT NULL  -- FK to ArtifactSet.Id
SlotType        NVARCHAR(20) NOT NULL
Name            NVARCHAR(100) NOT NULL
IconPath        NVARCHAR(500)
```

**Constraints:**
- `CK_ArtifactPiece_SlotType`: `SlotType IN ('Flower','Plume','Sands','Goblet','Circlet')`

**Foreign Keys:**
- `FK_ArtifactPiece_Set`: `ArtifactSetId → ArtifactSet.Id`

---

## 👤 Tablas de Inventario del Usuario

### UserCharacter

**Propósito:** Personajes que posee el usuario con progresión

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive            BIT DEFAULT 1
UserId              INT NOT NULL  -- FK to User.Id
CharacterId         INT NOT NULL  -- FK to Character.Id
Level               INT DEFAULT 1  -- 1-90
AscensionPhase      INT DEFAULT 0  -- 0-6
Constellation       INT DEFAULT 0  -- 0-6
TalentNormalAtk     INT DEFAULT 1  -- 1-15
TalentSkill         INT DEFAULT 1  -- 1-15
TalentBurst         INT DEFAULT 1  -- 1-15
CreatedAt           DATETIME DEFAULT GETDATE()
```

**Constraints:**
- `CK_UserCharacter_Level`: `Level BETWEEN 1 AND 90`
- `CK_UserCharacter_Ascension`: `AscensionPhase BETWEEN 0 AND 6`
- `CK_UserCharacter_Constellation`: `Constellation BETWEEN 0 AND 6`
- `CK_UserCharacter_Talents`: `TalentNormalAtk BETWEEN 1 AND 15` (similar para Skill y Burst)
- `UQ_UserCharacter_UserChar`: UNIQUE(`UserId`, `CharacterId`) - **Un usuario no puede tener el mismo personaje duplicado**

**Foreign Keys:**
- `FK_UserCharacter_User`: `UserId → User.Id`
- `FK_UserCharacter_Character`: `CharacterId → Character.Id`

---

### UserWeapon

**Propósito:** Armas que posee el usuario (permite duplicados)

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive            BIT DEFAULT 1
UserId              INT NOT NULL  -- FK to User.Id
WeaponId            INT NOT NULL  -- FK to Weapon.Id
Level               INT DEFAULT 1  -- 1-90
AscensionPhase      INT DEFAULT 0  -- 0-6
Refinement          INT DEFAULT 1  -- 1-5
CreatedAt           DATETIME DEFAULT GETDATE()
```

**Constraints:**
- `CK_UserWeapon_Level`: `Level BETWEEN 1 AND 90`
- `CK_UserWeapon_Ascension`: `AscensionPhase BETWEEN 0 AND 6`
- `CK_UserWeapon_Refinement`: `Refinement BETWEEN 1 AND 5`

**⚠️ NOTA:** A diferencia de `UserCharacter`, **SÍ permite duplicados** (un usuario puede tener múltiples copias de la misma arma)

**Foreign Keys:**
- `FK_UserWeapon_User`: `UserId → User.Id`
- `FK_UserWeapon_Weapon`: `WeaponId → Weapon.Id`

---

### UserArtifact

**Propósito:** Artefactos que posee el usuario con stats

**Columnas principales:**
```sql
Id                  BIGINT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive            BIT DEFAULT 1
UserId              INT NOT NULL  -- FK to User.Id
ArtifactPieceId     INT NOT NULL  -- FK to ArtifactPiece.Id
Level               INT DEFAULT 0  -- 0-20
Rarity              INT NOT NULL   -- 1-5
MainStatId          INT NOT NULL   -- FK to StatType.Id
MainStatValue       DECIMAL(10,2) NOT NULL
SubStat1Id          INT            -- FK to StatType.Id (nullable)
SubStat1Value       DECIMAL(10,2)
SubStat2Id          INT
SubStat2Value       DECIMAL(10,2)
SubStat3Id          INT
SubStat3Value       DECIMAL(10,2)
SubStat4Id          INT
SubStat4Value       DECIMAL(10,2)
CreatedAt           DATETIME DEFAULT GETDATE()
```

**Constraints:**
- `CK_UserArtifact_Level`: `Level BETWEEN 0 AND 20`
- `CK_UserArtifact_Rarity`: `Rarity BETWEEN 1 AND 5`

**Foreign Keys:**
- `FK_UserArtifact_User`: `UserId → User.Id`
- `FK_UserArtifact_Piece`: `ArtifactPieceId → ArtifactPiece.Id`
- `FK_UserArtifact_MainStat`: `MainStatId → StatType.Id`
- `FK_UserArtifact_SubStat1-4`: `SubStatXId → StatType.Id`

---

## 🏗️ Tablas de Sistema

### Build

**Propósito:** Configuraciones de personajes (equipamiento completo)

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive            BIT DEFAULT 1
UserId              INT NOT NULL  -- FK to User.Id
UserCharacterId     INT NOT NULL  -- FK to UserCharacter.Id
UserWeaponId        INT NOT NULL  -- FK to UserWeapon.Id
ArtifactFlowerId    BIGINT         -- FK to UserArtifact.Id
ArtifactPlumeId     BIGINT
ArtifactSandsId     BIGINT
ArtifactGobletId    BIGINT
ArtifactCircletId   BIGINT
Name                NVARCHAR(100) NOT NULL
Description         NVARCHAR(1000)
Version             INT DEFAULT 1
IsShared            BIT DEFAULT 0  -- Public builds
CreatedAt           DATETIME DEFAULT GETDATE()
```

**⚠️ IMPORTANTE:** 
- `UserWeaponId` y todos los `Artifact*Id` deben pertenecer al mismo `UserId` (validación en SP)
- `UserCharacterId` debe pertenecer al `UserId`

**Foreign Keys:**
- `FK_Build_User`: `UserId → User.Id`
- `FK_Build_UserCharacter`: `UserCharacterId → UserCharacter.Id`
- `FK_Build_UserWeapon`: `UserWeaponId → UserWeapon.Id`
- `FK_Build_Artifact*`: `Artifact*Id → UserArtifact.Id`

---

### Language

**Propósito:** Idiomas soportados para internacionalización

**Columnas principales:**
```sql
Id          INT IDENTITY PRIMARY KEY
PublicId    UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive    BIT DEFAULT 1
Code        NVARCHAR(10) UNIQUE NOT NULL  -- 'en', 'es', 'pt-br'
Name        NVARCHAR(50) NOT NULL          -- 'English', 'Español', 'Português (Brasil)'
IsDefault   BIT DEFAULT 0
```

**Constraints:**
- Solo UN idioma puede tener `IsDefault = 1`

**Valores esperados:** en, es, pt-br, ja, zh-cn, zh-tw, ko, fr, de, ru

---

### BugReport

**Propósito:** Sistema de reportes de bugs de usuarios

**Columnas principales:**
```sql
Id                  INT IDENTITY PRIMARY KEY
PublicId            UNIQUEIDENTIFIER DEFAULT NEWSEQUENTIALID()
IsActive            BIT DEFAULT 1
UserId              INT NOT NULL  -- FK to User.Id
Title               NVARCHAR(150) NOT NULL  -- Private (user's original)
Description         NVARCHAR(2000) NOT NULL -- Private (user's original)
PublicTitle         NVARCHAR(150)           -- Admin-written (public version)
PublicDescription   NVARCHAR(1000)          -- Admin-written (public version)
Status              NVARCHAR(20) DEFAULT 'Reported'
Priority            NVARCHAR(10)            -- Admin-only (nullable)
AssignedToUserId    INT                     -- FK to User.Id (admin assigned)
CreatedAt           DATETIME DEFAULT GETDATE()
```

**Constraints:**
- `CK_BugReport_Status`: `Status IN ('Reported','Confirmed','InProgress','Fixed','Closed','Duplicate','WontFix')`
- `CK_BugReport_Priority`: `Priority IS NULL OR Priority IN ('Critical','High','Medium','Low')`

**Foreign Keys:**
- `FK_BugReport_User`: `UserId → User.Id`
- `FK_BugReport_AssignedTo`: `AssignedToUserId → User.Id`

---

### BugReportImage

**Propósito:** Imágenes adjuntas a bug reports

**Columnas principales:**
```sql
Id              INT IDENTITY PRIMARY KEY
PublicId        UNIQUEIDENTIFIER DEFAULT NEWID()
IsActive        BIT DEFAULT 1
BugReportId     INT NOT NULL  -- FK to BugReport.Id
ImagePath       NVARCHAR(500) NOT NULL
FileName        NVARCHAR(255) NOT NULL
FileSizeBytes   INT NOT NULL
```

**Foreign Keys:**
- `BugReportId` → `BugReport.Id`

---

## 📊 Stored Procedures (85 TOTAL)

### PARTE 1: User (13 SPs)

*(Documentado anteriormente en el archivo)*

### PARTE 2: RefreshToken (5 SPs)

*(Documentado anteriormente en el archivo)*

### PARTE 3: EmailRateLimit (3 SPs)

*(Documentado anteriormente en el archivo)*

---

### PARTE 4: Character (7 SPs)

#### SP_S_Character
```sql
CREATE PROCEDURE SP_S_Character
```
**Propósito:** Obtener todos los personajes con datos relacionados  
**Returns:** Character con JOINs a Element, WeaponType, Region  
**Filtro:** WHERE IsActive = 1

#### SP_S_CharacterByPublicId
```sql
CREATE PROCEDURE SP_S_CharacterByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener un personaje específico por PublicId  
**Returns:** Character con JOINs o NULL

#### SP_S_CharacterByElement
```sql
CREATE PROCEDURE SP_S_CharacterByElement
    @ElementPublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener personajes de un elemento específico  
**Returns:** Todos los Character del elemento dado

#### SP_S_CharacterByRarity
```sql
CREATE PROCEDURE SP_S_CharacterByRarity
    @Rarity INT
```
**Propósito:** Obtener personajes por rareza (4 o 5 estrellas)  
**Returns:** Todos los Character con la rareza especificada

#### SP_I_Character
```sql
CREATE PROCEDURE SP_I_Character
    @Name NVARCHAR(100),
    @ElementId INT,
    @WeaponTypeId INT,
    @RegionId INT,
    @Rarity INT,
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500) = NULL,
    @IconPath NVARCHAR(500) = NULL
```
**Propósito:** Insertar nuevo personaje  
**Returns:** PublicId del personaje creado

#### SP_U_Character
```sql
CREATE PROCEDURE SP_U_Character
    @PublicId UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @ElementId INT,
    @WeaponTypeId INT,
    @RegionId INT,
    @Rarity INT,
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500) = NULL,
    @IconPath NVARCHAR(500) = NULL
```
**Propósito:** Actualizar personaje existente  
**Validación:** WHERE PublicId = @PublicId AND IsActive = 1

#### SP_D_Character
```sql
CREATE PROCEDURE SP_D_Character
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación:** WHERE PublicId = @PublicId AND IsActive = 1

---

### PARTE 5: Weapon (7 SPs)

#### SP_S_Weapon
```sql
CREATE PROCEDURE SP_S_Weapon
```
**Propósito:** Obtener todas las armas activas con datos relacionados  
**Returns:** Weapon con JOINs a WeaponType, StatType (para SecondaryStatId)  
**Filtro:** WHERE IsActive = 1

#### SP_S_WeaponByPublicId
```sql
CREATE PROCEDURE SP_S_WeaponByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener arma específica por PublicId  
**Returns:** Weapon con JOINs o NULL

#### SP_S_WeaponByType
```sql
CREATE PROCEDURE SP_S_WeaponByType
    @WeaponTypePublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener armas de un tipo específico (Sword, Claymore, etc.)  
**Returns:** Todas las Weapon del tipo dado

#### SP_S_WeaponByRarity
```sql
CREATE PROCEDURE SP_S_WeaponByRarity
    @Rarity INT
```
**Propósito:** Obtener armas por rareza (1-5 estrellas)  
**Returns:** Todas las Weapon con la rareza especificada

#### SP_I_Weapon
```sql
CREATE PROCEDURE SP_I_Weapon
    @Name NVARCHAR(100),
    @WeaponTypeId INT,
    @Rarity INT,
    @BaseAtk DECIMAL(10,2),
    @SecondaryStatId INT = NULL,
    @SecondaryStatValue DECIMAL(10,2) = NULL,
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500) = NULL,
    @IconPath NVARCHAR(500) = NULL
```
**Propósito:** Insertar nueva arma  
**Nota:** BaseAtk es DECIMAL, SecondaryStatId es FK a StatType  
**Returns:** PublicId del arma creada

#### SP_U_Weapon
```sql
CREATE PROCEDURE SP_U_Weapon
    @PublicId UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @WeaponTypeId INT,
    @Rarity INT,
    @BaseAtk DECIMAL(10,2),
    @SecondaryStatId INT = NULL,
    @SecondaryStatValue DECIMAL(10,2) = NULL,
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500) = NULL,
    @IconPath NVARCHAR(500) = NULL
```
**Propósito:** Actualizar arma existente  
**Validación:** WHERE PublicId = @PublicId AND IsActive = 1

#### SP_D_Weapon
```sql
CREATE PROCEDURE SP_D_Weapon
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación:** WHERE PublicId = @PublicId AND IsActive = 1

---

### PARTE 6: ArtifactSet (7 SPs)

#### SP_S_ArtifactSet
```sql
CREATE PROCEDURE SP_S_ArtifactSet
```
**Propósito:** Obtener todos los sets de artefactos activos  
**Returns:** ArtifactSet con bonuses 2pc y 4pc  
**Filtro:** WHERE IsActive = 1

#### SP_S_ArtifactSetByPublicId
```sql
CREATE PROCEDURE SP_S_ArtifactSetByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener set específico por PublicId  
**Returns:** ArtifactSet o NULL

#### SP_S_ArtifactPieceByPublicId
```sql
CREATE PROCEDURE SP_S_ArtifactPieceByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener pieza individual de artefacto  
**Returns:** ArtifactPiece con JOIN a ArtifactSet

#### SP_S_ArtifactPieceBySlotType
```sql
CREATE PROCEDURE SP_S_ArtifactPieceBySlotType
    @SlotType NVARCHAR(20)
```
**Propósito:** Obtener piezas por tipo de slot (Flower, Plume, Sands, Goblet, Circlet)  
**Returns:** Todas las ArtifactPiece del slot especificado

#### SP_I_ArtifactSet
```sql
CREATE PROCEDURE SP_I_ArtifactSet
    @Name NVARCHAR(100),
    @Bonus2Pc NVARCHAR(500),
    @Bonus4Pc NVARCHAR(500),
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500) = NULL,
    @IconPath NVARCHAR(500) = NULL
```
**Propósito:** Insertar nuevo set de artefactos  
**Returns:** PublicId del set creado

#### SP_U_ArtifactSet
```sql
CREATE PROCEDURE SP_U_ArtifactSet
    @PublicId UNIQUEIDENTIFIER,
    @Name NVARCHAR(100),
    @Bonus2Pc NVARCHAR(500),
    @Bonus4Pc NVARCHAR(500),
    @ImagePath NVARCHAR(500),
    @ThumbnailPath NVARCHAR(500) = NULL,
    @IconPath NVARCHAR(500) = NULL
```
**Propósito:** Actualizar set existente  
**Validación:** WHERE PublicId = @PublicId AND IsActive = 1

#### SP_D_ArtifactSet
```sql
CREATE PROCEDURE SP_D_ArtifactSet
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación:** WHERE PublicId = @PublicId AND IsActive = 1  
**Nota:** ArtifactSet es catálogo, UserArtifact es inventario del usuario

---

### PARTE 7: Build (6 SPs)

#### SP_S_BuildByUserId
```sql
CREATE PROCEDURE SP_S_BuildByUserId
    @UserId INT
```
**Propósito:** Obtener todas las builds del usuario  
**Returns:** Build con JOINs a UserCharacter, UserWeapon, UserArtifact (5 piezas)  
**Validación ownership:** Solo builds del usuario especificado

#### SP_S_BuildByPublicId
```sql
CREATE PROCEDURE SP_S_BuildByPublicId
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Obtener build específica  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_S_BuildShared
```sql
CREATE PROCEDURE SP_S_BuildShared
```
**Propósito:** Obtener builds públicas compartidas  
**Returns:** Todas las Build WHERE IsShared = 1 AND IsActive = 1

#### SP_I_Build
```sql
CREATE PROCEDURE SP_I_Build
    @UserId INT,
    @UserCharacterId INT,
    @UserWeaponId INT,
    @ArtifactFlowerId INT,
    @ArtifactPlumeId INT,
    @ArtifactSandsId INT,
    @ArtifactGobletId INT,
    @ArtifactCircletId INT,
    @BuildName NVARCHAR(100),
    @IsShared BIT = 0
```
**Propósito:** Crear nueva build  
**Validación ownership:** Verifica que UserWeapon y todos los UserArtifact pertenezcan a @UserId  
**Returns:** PublicId de la build creada

#### SP_U_Build
```sql
CREATE PROCEDURE SP_U_Build
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT,
    @UserCharacterId INT,
    @UserWeaponId INT,
    @ArtifactFlowerId INT,
    @ArtifactPlumeId INT,
    @ArtifactSandsId INT,
    @ArtifactGobletId INT,
    @ArtifactCircletId INT,
    @BuildName NVARCHAR(100),
    @IsShared BIT
```
**Propósito:** Actualizar build existente  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId  
**Validación items:** Verifica que UserWeapon y UserArtifacts pertenezcan a @UserId

#### SP_D_Build
```sql
CREATE PROCEDURE SP_D_Build
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

---

### PARTE 8: UserCharacter (5 SPs)

#### SP_S_UserCharacterByUserId
```sql
CREATE PROCEDURE SP_S_UserCharacterByUserId
    @UserId INT
```
**Propósito:** Obtener personajes del inventario del usuario  
**Returns:** UserCharacter con JOIN a Character (catálogo)  
**Validación ownership:** WHERE UserId = @UserId AND IsActive = 1

#### SP_S_UserCharacterByPublicId
```sql
CREATE PROCEDURE SP_S_UserCharacterByPublicId
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Obtener personaje específico del inventario  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_I_UserCharacter
```sql
CREATE PROCEDURE SP_I_UserCharacter
    @UserId INT,
    @CharacterId INT,
    @Level INT = 1,
    @Constellation INT = 0,
    @TalentNormalAttack INT = 1,
    @TalentElementalSkill INT = 1,
    @TalentElementalBurst INT = 1
```
**Propósito:** Agregar personaje al inventario del usuario  
**Validación:** No permite duplicados (constraint unique en UserId + CharacterId)  
**Returns:** PublicId del UserCharacter creado

#### SP_U_UserCharacter
```sql
CREATE PROCEDURE SP_U_UserCharacter
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT,
    @Level INT,
    @Constellation INT,
    @TalentNormalAttack INT,
    @TalentElementalSkill INT,
    @TalentElementalBurst INT
```
**Propósito:** Actualizar progresión del personaje (nivel, constelaciones, talentos)  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_D_UserCharacter
```sql
CREATE PROCEDURE SP_D_UserCharacter
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

---

### PARTE 9: UserWeapon (5 SPs)

#### SP_S_UserWeaponByUserId
```sql
CREATE PROCEDURE SP_S_UserWeaponByUserId
    @UserId INT
```
**Propósito:** Obtener armas del inventario del usuario  
**Returns:** UserWeapon con JOIN a Weapon (catálogo)  
**Validación ownership:** WHERE UserId = @UserId AND IsActive = 1  
**Nota:** El usuario PUEDE tener múltiples copias de la misma arma

#### SP_S_UserWeaponByPublicId
```sql
CREATE PROCEDURE SP_S_UserWeaponByPublicId
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Obtener arma específica del inventario  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_I_UserWeapon
```sql
CREATE PROCEDURE SP_I_UserWeapon
    @UserId INT,
    @WeaponId INT,
    @Level INT = 1,
    @Refinement INT = 1
```
**Propósito:** Agregar arma al inventario del usuario  
**Nota:** Permite duplicados (a diferencia de UserCharacter)  
**Returns:** PublicId del UserWeapon creado

#### SP_U_UserWeapon
```sql
CREATE PROCEDURE SP_U_UserWeapon
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT,
    @Level INT,
    @Refinement INT
```
**Propósito:** Actualizar nivel y refinamiento del arma  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_D_UserWeapon
```sql
CREATE PROCEDURE SP_D_UserWeapon
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

---

### PARTE 10: UserArtifact (6 SPs)

#### SP_S_UserArtifactByUserId
```sql
CREATE PROCEDURE SP_S_UserArtifactByUserId
    @UserId INT
```
**Propósito:** Obtener artefactos del inventario del usuario  
**Returns:** UserArtifact con JOIN a ArtifactPiece y ArtifactSet  
**Validación ownership:** WHERE UserId = @UserId AND IsActive = 1

#### SP_S_UserArtifactByPublicId
```sql
CREATE PROCEDURE SP_S_UserArtifactByPublicId
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Obtener artefacto específico del inventario  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_S_UserArtifactBySlotType
```sql
CREATE PROCEDURE SP_S_UserArtifactBySlotType
    @UserId INT,
    @SlotType NVARCHAR(20)
```
**Propósito:** Obtener artefactos del usuario filtrados por slot  
**Slots válidos:** Flower, Plume, Sands, Goblet, Circlet  
**Returns:** UserArtifact WHERE UserId = @UserId AND SlotType = @SlotType

#### SP_I_UserArtifact
```sql
CREATE PROCEDURE SP_I_UserArtifact
    @UserId INT,
    @ArtifactPieceId INT,
    @Level INT = 0,
    @MainStatId INT,
    @MainStatValue DECIMAL(10,2),
    @SubStat1Id INT = NULL,
    @SubStat1Value DECIMAL(10,2) = NULL,
    @SubStat2Id INT = NULL,
    @SubStat2Value DECIMAL(10,2) = NULL,
    @SubStat3Id INT = NULL,
    @SubStat3Value DECIMAL(10,2) = NULL,
    @SubStat4Id INT = NULL,
    @SubStat4Value DECIMAL(10,2) = NULL
```
**Propósito:** Agregar artefacto al inventario del usuario  
**Nota:** MainStatId/SubStatXId son FKs a StatType, NUNCA texto  
**Returns:** PublicId del UserArtifact creado

#### SP_U_UserArtifact
```sql
CREATE PROCEDURE SP_U_UserArtifact
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT,
    @Level INT,
    @MainStatValue DECIMAL(10,2),
    @SubStat1Value DECIMAL(10,2) = NULL,
    @SubStat2Value DECIMAL(10,2) = NULL,
    @SubStat3Value DECIMAL(10,2) = NULL,
    @SubStat4Value DECIMAL(10,2) = NULL
```
**Propósito:** Actualizar nivel y valores de stats (no los IDs)  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

#### SP_D_UserArtifact
```sql
CREATE PROCEDURE SP_D_UserArtifact
    @PublicId UNIQUEIDENTIFIER,
    @UserId INT
```
**Propósito:** Eliminación lógica (IsActive = 0)  
**Validación ownership:** WHERE PublicId = @PublicId AND UserId = @UserId

---

### PARTE 11: Catalogs (8 SPs)

#### Element (2 SPs)

**SP_S_Element**
```sql
CREATE PROCEDURE SP_S_Element
```
**Propósito:** Obtener todos los elementos activos  
**Returns:** Element (Pyro, Hydro, Anemo, Electro, Dendro, Cryo, Geo)

**SP_S_ElementByPublicId**
```sql
CREATE PROCEDURE SP_S_ElementByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener elemento específico  
**Nota:** Element NO tiene columna Color

---

#### WeaponType (2 SPs)

**SP_S_WeaponType**
```sql
CREATE PROCEDURE SP_S_WeaponType
```
**Propósito:** Obtener todos los tipos de armas activos  
**Returns:** WeaponType (Sword, Claymore, Polearm, Bow, Catalyst)

**SP_S_WeaponTypeByPublicId**
```sql
CREATE PROCEDURE SP_S_WeaponTypeByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener tipo de arma específico

---

#### Region (2 SPs)

**SP_S_Region**
```sql
CREATE PROCEDURE SP_S_Region
```
**Propósito:** Obtener todas las regiones activas  
**Returns:** Region (Mondstadt, Liyue, Inazuma, Sumeru, Fontaine, Natlan, Snezhnaya)

**SP_S_RegionByPublicId**
```sql
CREATE PROCEDURE SP_S_RegionByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener región específica

---

#### StatType (2 SPs)

**SP_S_StatType**
```sql
CREATE PROCEDURE SP_S_StatType
```
**Propósito:** Obtener todos los tipos de estadísticas activos  
**Returns:** StatType (HP, ATK, DEF, HP%, ATK%, DEF%, Energy Recharge%, Elemental Mastery, CRIT Rate%, CRIT DMG%, etc.)

**SP_S_StatTypeByPublicId**
```sql
CREATE PROCEDURE SP_S_StatTypeByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener tipo de estadística específico

---

### PARTE 12: Language & BugReport (16 SPs)

#### Language (4 SPs)

**SP_S_Language**
```sql
CREATE PROCEDURE SP_S_Language
```
**Propósito:** Obtener todos los idiomas activos  
**Returns:** Language (en, es, pt-br, etc.)

**SP_S_LanguageByPublicId**
```sql
CREATE PROCEDURE SP_S_LanguageByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener idioma específico

**SP_S_LanguageByCode**
```sql
CREATE PROCEDURE SP_S_LanguageByCode
    @Code NVARCHAR(10)
```
**Propósito:** Obtener idioma por código ISO (e.g., 'en', 'es')

**SP_S_LanguageDefault**
```sql
CREATE PROCEDURE SP_S_LanguageDefault
```
**Propósito:** Obtener el idioma por defecto  
**Returns:** Language WHERE IsDefault = 1

---

#### BugReport (12 SPs)

**SP_S_BugReportByUserId**
```sql
CREATE PROCEDURE SP_S_BugReportByUserId
    @UserId INT
```
**Propósito:** Obtener bug reports del usuario  
**Returns:** BugReport WHERE UserId = @UserId

**SP_S_BugReportByPublicId**
```sql
CREATE PROCEDURE SP_S_BugReportByPublicId
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Obtener bug report específico  
**Returns:** BugReport con PublicTitle, PublicDescription (versión pública), AssignedTo (admin)

**SP_S_BugReportByStatus**
```sql
CREATE PROCEDURE SP_S_BugReportByStatus
    @Status NVARCHAR(20)
```
**Propósito:** Obtener bug reports por estado  
**Estados válidos:** Reported, Confirmed, InProgress, Fixed, Closed, Duplicate, WontFix

**SP_S_BugReportByPriority**
```sql
CREATE PROCEDURE SP_S_BugReportByPriority
    @Priority NVARCHAR(10)
```
**Propósito:** Obtener bug reports por prioridad  
**Prioridades válidas:** Low, Medium, High, Critical (puede ser NULL)

**SP_I_BugReport**
```sql
CREATE PROCEDURE SP_I_BugReport
    @UserId INT,
    @Title NVARCHAR(200),
    @Description NVARCHAR(MAX)
```
**Propósito:** Crear nuevo bug report  
**Defaults:** Status = 'Reported', Priority = NULL, PublicTitle = NULL, PublicDescription = NULL  
**Returns:** PublicId del bug report creado

**SP_U_BugReportStatus**
```sql
CREATE PROCEDURE SP_U_BugReportStatus
    @PublicId UNIQUEIDENTIFIER,
    @Status NVARCHAR(20)
```
**Propósito:** Actualizar estado del bug report (admin)  
**Validación:** Status IN ('Reported', 'Confirmed', 'InProgress', 'Fixed', 'Closed', 'Duplicate', 'WontFix')

**SP_U_BugReportPriority**
```sql
CREATE PROCEDURE SP_U_BugReportPriority
    @PublicId UNIQUEIDENTIFIER,
    @Priority NVARCHAR(10)
```
**Propósito:** Actualizar prioridad del bug report (admin)  
**Validación:** Priority IN ('Low', 'Medium', 'High', 'Critical') o NULL

**SP_U_BugReportPublicVersion**
```sql
CREATE PROCEDURE SP_U_BugReportPublicVersion
    @PublicId UNIQUEIDENTIFIER,
    @PublicTitle NVARCHAR(200),
    @PublicDescription NVARCHAR(MAX)
```
**Propósito:** Actualizar versión pública del bug report (admin)  
**Nota:** Title/Description son privados (del usuario), PublicTitle/PublicDescription son públicos (escritos por admins)

**SP_U_BugReportAssign**
```sql
CREATE PROCEDURE SP_U_BugReportAssign
    @PublicId UNIQUEIDENTIFIER,
    @AssignedToUserId INT
```
**Propósito:** Asignar bug report a un admin  
**Validación:** AssignedToUserId debe existir en User.Id

**SP_D_BugReport**
```sql
CREATE PROCEDURE SP_D_BugReport
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Eliminación lógica (IsActive = 0)

**SP_I_BugReportImage**
```sql
CREATE PROCEDURE SP_I_BugReportImage
    @BugReportId INT,
    @ImagePath NVARCHAR(500),
    @FileName NVARCHAR(255),
    @FileSizeBytes INT
```
**Propósito:** Agregar imagen a bug report  
**Returns:** PublicId de la imagen creada

**SP_D_BugReportImage**
```sql
CREATE PROCEDURE SP_D_BugReportImage
    @PublicId UNIQUEIDENTIFIER
```
**Propósito:** Eliminación lógica de imagen (IsActive = 0)

---

## 📋 Resumen de Stored Procedures

| Categoría | Cantidad | Archivos |
|-----------|----------|----------|
| **Authentication** | 21 SPs | Part1_User (13), Part2_RefreshToken (5), Part3_EmailRateLimit (3) |
| **Game Catalogs** | 27 SPs | Part4_Character (7), Part5_Weapon (7), Part6_ArtifactSet (7), Part11_Catalogs (8) |
| **User Inventory** | 16 SPs | Part8_UserCharacter (5), Part9_UserWeapon (5), Part10_UserArtifact (6) |
| **Build System** | 6 SPs | Part7_Build (6) |
| **Supporting Systems** | 15 SPs | Part12_Language (4), Part12_BugReport (12) |
| **TOTAL** | **85 SPs** | 12 archivos SQL |

---

## ⚠️ Lecciones Aprendidas (Schema Validation)

1. **Stats siempre usan FK a StatType** - `SecondaryStatId`, `MainStatId`, `SubStat1Id`... NUNCA columnas de texto (`SubStat`, `MainStat`)
2. **Weapon usa BaseAtk DECIMAL** - No `BaseAttack INT`
3. **Image paths consistentes** - `ImagePath`, `ThumbnailPath`, `IconPath` (NUNCA `SplashPath`)
4. **Build usa inventario del usuario** - `UserWeapon`, `UserArtifact` (NO catálogos `Weapon`, `Artifact`)
5. **Character usa RegionId FK** - No columna `Region` de texto
6. **Element NO tiene columna Color**
7. **Evitar aliases SQL reservados** - Nunca `asc`, `desc` o inapropiados como `ass` → Usar `setc`, `sets`, `setf`
8. **ArtifactSet vs UserArtifact** - Set es catálogo con bonuses, UserArtifact es inventario con stats
9. **Weapon rarity 1-5, Character rarity 4-5**
10. **Validar ownership en Build** - UserWeapon/UserArtifact deben pertenecer a @UserId
11. **BugReport tiene dual versions** - `Title`/`Description` (usuario) vs `PublicTitle`/`PublicDescription` (admin)
12. **BugReport Status values** - `Reported`, `Confirmed`, `InProgress`, `Fixed`, `Closed`, `Duplicate`, `WontFix`

---

## 🔍 Quick Reference

### Convenciones de Nomenclatura

- **SP_S_**: SELECT (consultas)
- **SP_I_**: INSERT (crear)
- **SP_U_**: UPDATE (actualizar)
- **SP_D_**: DELETE lógico (IsActive = 0)
- **SP_IU_**: INSERT OR UPDATE (upsert)

### Campos Obligatorios

Todas las tablas (excepto EmailRateLimit) tienen:
- `Id` INT IDENTITY PRIMARY KEY
- `PublicId` UNIQUEIDENTIFIER DEFAULT NEWID() o NEWSEQUENTIALID()
- `IsActive` BIT DEFAULT 1

Todas las tablas `_History` tienen además:
- `OperationType` CHAR(1) - 'I' (Insert), 'U' (Update), 'D' (Delete)
- `OperationDate` DATETIME2 DEFAULT GETDATE()
- `OperationUser` NVARCHAR(100)

### Patrones de Validación

1. **Ownership Validation** - Siempre usar `WHERE UserId = @UserId` en inventario del usuario
2. **Soft Delete** - Siempre `SET IsActive = 0`, nunca DELETE físico (excepto EmailRateLimit)
3. **PublicId Usage** - Exponer PublicId en API, NUNCA Id interno
4. **FK Validation** - Stats usan FKs a StatType, no texto
5. **Catalog vs Inventory** - Catalog (Character/Weapon/ArtifactSet) vs User Inventory (UserCharacter/UserWeapon/UserArtifact)

---

**🎯 FIN DE DOCUMENTACIÓN - 85 STORED PROCEDURES COMPLETADOS**
FileName        NVARCHAR(255)
FileSizeBytes   INT
```

**Foreign Keys:**
- `FK_BugReportImage_Report`: `BugReportId → BugReport.Id`

---

## 📊 Stored Procedures (85 TOTAL)

### Convención de Nombres

**Prefijos:**
- `SP_S_` → SELECT (consultas)
- `SP_I_` → INSERT (crear)
- `SP_U_` → UPDATE (actualizar)
- `SP_D_` → DELETE (soft delete, `IsActive=0`)
- `SP_IU_` → INSERT or UPDATE (upsert)

---

### PART 1: User (13 SPs)

**Autenticación y gestión de perfil**

1. **SP_S_UserByEmail**
   - Parámetros: `@Email`
   - Uso: Login lookup
   - Retorna: User completo si existe y está activo

2. **SP_S_UserByPublicId**
   - Parámetros: `@PublicId`
   - Uso: Obtener perfil de usuario
   - Retorna: User (sin PasswordHash)

3. **SP_S_UserByVerificationToken**
   - Parámetros: `@Token`
   - Uso: Verificar email
   - Valida: Token válido y no expirado

4. **SP_S_UserByResetToken**
   - Parámetros: `@Token`
   - Uso: Reset de contraseña
   - Valida: Token válido y no expirado (1 hora)

5. **SP_I_User**
   - Parámetros: `@Email`, `@Username`, `@PasswordHash`, `@AuthProvider`, `@AuthProviderId`
   - Uso: Registro de nuevo usuario
   - Valida: Username + UserCode únicos (genera `user#1234`)
   - Retorna: PublicId del usuario creado

6. **SP_U_User**
   - Parámetros: `@PublicId`, `@Username`, `@Bio`, `@AvatarPath`
   - Uso: Actualizar perfil
   - Valida: Username disponible (si cambió)

7. **SP_U_UserAvatar**
   - Parámetros: `@PublicId`, `@AvatarPath`
   - Uso: Actualizar solo avatar
   - Retorna: Confirmación

8. **SP_U_UserVerifyEmail**
   - Parámetros: `@Token`
   - Uso: Marcar email como verificado
   - Valida: Token válido y no expirado (24h)
   - Actualiza: `IsEmailVerified=1`, `EmailVerificationToken=NULL`

9. **SP_U_UserSetResetToken**
   - Parámetros: `@Email`
   - Uso: Generar token de reset
   - Genera: Token válido por 1 hora
   - Retorna: Token generado

10. **SP_U_UserResetPassword**
    - Parámetros: `@Token`, `@NewPasswordHash`
    - Uso: Reset de contraseña
    - Valida: Token válido y no expirado
    - Actualiza: `PasswordHash`, `PasswordResetToken=NULL`

11. **SP_U_UserChangePassword**
    - Parámetros: `@PublicId`, `@OldPasswordHash`, `@NewPasswordHash`
    - Uso: Cambio de contraseña (requiere antigua)
    - Valida: Contraseña antigua correcta
    - Invalida: TODOS los refresh tokens del usuario

12. **SP_U_UserChangeUsername**
    - Parámetros: `@PublicId`, `@NewUsername`
    - Uso: Cambiar username
    - Valida: Username disponible, no cambiado en últimos 30 días
    - Actualiza: `UsernameLastChangedAt`

13. **SP_D_User**
    - Parámetros: `@PublicId`
    - Uso: Eliminación lógica de cuenta
    - Actualiza: `IsActive=0`
    - Invalida: TODOS los refresh tokens

---

### PART 2: RefreshToken (5 SPs)

**Gestión de tokens JWT**

14. **SP_S_RefreshTokenByToken**
    - Parámetros: `@Token`
    - Uso: Validar refresh token
    - Valida: Token no expirado + usuario activo
    - Retorna: User.PublicId

15. **SP_I_RefreshToken**
    - Parámetros: `@UserPublicId`, `@Token`, `@ExpiresAt`, `@DeviceFingerprint`
    - Uso: Crear nuevo refresh token
    - Retorna: PublicId del token creado

16. **SP_D_RefreshToken**
    - Parámetros: `@Token`
    - Uso: Logout (invalidar un token específico)
    - Actualiza: `IsActive=0`

17. **SP_D_RefreshTokenByUser**
    - Parámetros: `@UserPublicId`
    - Uso: Invalidar TODOS los tokens del usuario
    - Usado en: Cambio de contraseña, eliminación de cuenta

18. **SP_D_RefreshTokenExpired**
    - Sin parámetros
    - Uso: Limpieza automática (tarea programada)
    - Elimina: Tokens expirados hace más de 7 días

---

### PART 3: EmailRateLimit (3 SPs)

**Control anti-abuso de emails**

19. **SP_S_EmailRateLimit**
    - Parámetros: `@Email`
    - Uso: Verificar si puede enviar email
    - Valida: < 5 requests en ventana de 60 minutos
    - Retorna: `CanSend BIT`, `RequestCount`, `TimeRemaining`

20. **SP_IU_EmailRateLimit**
    - Parámetros: `@Email`
    - Uso: Registrar intento de envío
    - Comportamiento: UPSERT (crea o actualiza contador)

21. **SP_D_EmailRateLimitExpired**
    - Sin parámetros
    - Uso: Limpieza automática
    - Elimina: Registros > 60 minutos
    - ⚠️ **ÚNICA tabla con DELETE físico**

---

_(Continuará...)_
