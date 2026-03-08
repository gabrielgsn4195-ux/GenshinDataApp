# GenshinDataApp — Genshin Data

<div align="center">

![Genshin Data](https://img.shields.io/badge/Genshin_Data-Fan_Project-FFD700?style=for-the-badge)
![License](https://img.shields.io/badge/License-MIT-blue?style=for-the-badge)
![Status](https://img.shields.io/badge/Status-In_Development-green?style=for-the-badge)
![Unofficial](https://img.shields.io/badge/Unofficial-Fan_Made-red?style=for-the-badge)

**[Español](#español) · [English](#english) · [Português (Brasil)](#português-brasil)**

</div>

---

> ⚠️ **DISCLAIMER / AVISO LEGAL / AVISO LEGAL**
>
> This is an **unofficial fan-made project**. Not affiliated with, endorsed by, or sponsored by HoYoverse.
> **Genshin Impact™** and all related assets, characters, names, and content are **trademarks and copyrights of HoYoverse Co., Ltd. © HoYoverse Co., Ltd. All rights reserved.**
> All game data used in this application is sourced from publicly available community resources and belongs to their respective owners.
> The source code of this project is open source under the MIT License, but **game assets (images, names, characters, lore) are NOT included under this license** and remain property of HoYoverse.

---

<br/>

# Español

## 📖 Descripción

**Genshin Data** es una aplicación web de código abierto creada por y para la comunidad de Genshin Impact. Permite a los jugadores gestionar su inventario de personajes, armas y artefactos, consultar información detallada del juego y crear builds optimizadas para sus personajes favoritos.

## ✨ Funcionalidades

- 📋 **Catálogo del juego** — Consulta información detallada de personajes, armas y artefactos con costes de mejora
- 🎒 **Inventario personal** — Gestiona tu colección de personajes, armas y artefactos
- ⚔️ **Constructor de Builds** — Crea builds con drag & drop y visualiza las estadísticas en tiempo real
- 🔗 **Compartir builds** — Comparte tus builds con la comunidad mediante un enlace público
- 📊 **Comparador de builds** — Compara dos builds del mismo personaje lado a lado
- 🌍 **Multilenguaje** — Disponible en Español, Inglés y Português (Brasil)
- 🌙 **Modo oscuro/claro** — Detecta automáticamente la preferencia del sistema
- 👤 **Cuentas de usuario** — Registro propio o acceso con Google, Apple o Twitch

## 🛠️ Stack Tecnológico

| Capa | Tecnología |
|------|-----------|
| Backend | .NET Core |
| Base de datos | Microsoft SQL Server |
| API | ASP.NET Core Web API |
| Frontend | Angular + Tailwind CSS |
| Imágenes | Cloudflare R2 |
| Despliegue | Cloudflare |

## 🚀 Instalación y configuración

### Requisitos previos
- Visual Studio 2026
- .NET Core SDK
- Node.js y npm
- Microsoft SQL Server
- Git

### 1. Clonar el repositorio
```bash
git clone https://github.com/tuusuario/GenshinDataApp.git
cd GenshinDataApp
git checkout develop
```

### 2. Configurar el Backend
```bash
# Copia el archivo de configuración de ejemplo
cp GenshinDataApp.Services/appsettings.example.json GenshinDataApp.Services/appsettings.Development.json

# Edita appsettings.Development.json con tus valores:
# - Cadena de conexión a SQL Server
# - Clave secreta JWT
# - Credenciales OAuth (Google, Apple, Twitch)
# - Credenciales de Cloudflare R2
```

### 3. Configurar la Base de Datos
```bash
# Ejecuta las migraciones y procedimientos almacenados
# (instrucciones detalladas en /docs/database-setup.md)
```

### 4. Configurar el Frontend
```bash
cd GenshinDataApp.Frontend
npm install
```

### 5. Ejecutar en desarrollo
```bash
# Backend (desde Visual Studio o terminal)
dotnet run --project GenshinDataApp.Services

# Frontend
cd GenshinDataApp.Frontend
ng serve
```

## ⚙️ Variables de entorno requeridas

Crea `appsettings.Development.json` basándote en `appsettings.example.json`. **Nunca subas este archivo al repositorio.**

| Variable | Descripción |
|----------|------------|
| `ConnectionStrings:DefaultConnection` | Cadena de conexión a SQL Server |
| `Jwt:Secret` | Clave secreta para firmar los tokens JWT |
| `Jwt:ExpirationMinutes` | Tiempo de expiración del JWT (recomendado: 15) |
| `OAuth:Google:ClientId` | Client ID de Google OAuth |
| `OAuth:Google:ClientSecret` | Client Secret de Google OAuth |
| `OAuth:Apple:ClientId` | Client ID de Apple OAuth |
| `OAuth:Apple:ClientSecret` | Client Secret de Apple OAuth |
| `OAuth:Twitch:ClientId` | Client ID de Twitch OAuth |
| `OAuth:Twitch:ClientSecret` | Client Secret de Twitch OAuth |
| `CloudflareR2:AccountId` | ID de cuenta de Cloudflare |
| `CloudflareR2:AccessKey` | Access Key de Cloudflare R2 |
| `CloudflareR2:SecretKey` | Secret Key de Cloudflare R2 |
| `CloudflareR2:BucketName` | Nombre del bucket de R2 |

## 🌿 Ramas Git

| Rama | Entorno | Descripción |
|------|---------|-------------|
| `main` | Producción | Código estable y desplegado |
| `integration` | Integración | Validación previa a producción |
| `develop` | Desarrollo | Rama principal de trabajo |
| `feature/nombre` | Local | Nuevas funcionalidades |
| `hotfix/nombre` | — | Correcciones urgentes |

## 🤝 Contribuir

¡Las contribuciones son bienvenidas! Por favor lee las [guías de contribución](CONTRIBUTING.md) antes de hacer un Pull Request. Asegúrate de seguir las convenciones de código descritas en `.github/copilot-instructions.md`.

## ☕ Apoyar el proyecto

Si este proyecto te resulta útil, puedes apoyar su desarrollo en [Buy Me a Coffee](https://buymeacoffee.com/tuusuario). Las donaciones ayudan a cubrir los costes de servidor y mantenimiento.

*Las donaciones son completamente voluntarias. Este proyecto siempre será gratuito y de código abierto.*

## 📄 Licencia

El **código fuente** de este proyecto está bajo la licencia [MIT](LICENSE).

**Los assets del juego (imágenes, nombres de personajes, lore y demás contenido de Genshin Impact) NO están incluidos bajo esta licencia y son propiedad exclusiva de HoYoverse Co., Ltd.**

## ⚖️ Aviso de Copyright y Marca Registrada

```
Este es un proyecto no oficial creado por fans.
No está afiliado, respaldado ni patrocinado por HoYoverse.

Genshin Impact™ es una marca registrada de HoYoverse Co., Ltd.
© HoYoverse Co., Ltd. Todos los derechos reservados.

Todos los assets del juego, personajes, nombres y contenido relacionado
son propiedad de HoYoverse Co., Ltd.
Este proyecto se realiza únicamente con fines educativos y comunitarios,
sin ánimo de lucro.
```

---

<br/>

# English

## 📖 Description

**Genshin Data** is an open source web application created by and for the Genshin Impact community. It allows players to manage their inventory of characters, weapons and artifacts, consult detailed game information and create optimized builds for their favorite characters.

## ✨ Features

- 📋 **Game Catalog** — Browse detailed information on characters, weapons and artifacts with upgrade costs
- 🎒 **Personal Inventory** — Manage your collection of characters, weapons and artifacts
- ⚔️ **Build Creator** — Create builds with drag & drop and view stats in real time
- 🔗 **Build Sharing** — Share your builds with the community via a public link
- 📊 **Build Comparison** — Compare two builds for the same character side by side
- 🌍 **Multilanguage** — Available in Spanish, English and Português (Brasil)
- 🌙 **Dark/Light Mode** — Automatically detects system preference
- 👤 **User Accounts** — Register directly or sign in with Google, Apple or Twitch

## 🛠️ Tech Stack

| Layer | Technology |
|-------|-----------|
| Backend | .NET Core |
| Database | Microsoft SQL Server |
| API | ASP.NET Core Web API |
| Frontend | Angular + Tailwind CSS |
| Images | Cloudflare R2 |
| Deployment | Cloudflare |

## 🚀 Installation & Setup

### Prerequisites
- Visual Studio 2026
- .NET Core SDK
- Node.js and npm
- Microsoft SQL Server
- Git

### 1. Clone the repository
```bash
git clone https://github.com/yourusername/GenshinDataApp.git
cd GenshinDataApp
git checkout develop
```

### 2. Configure the Backend
```bash
# Copy the example configuration file
cp GenshinDataApp.Services/appsettings.example.json GenshinDataApp.Services/appsettings.Development.json

# Edit appsettings.Development.json with your values:
# - SQL Server connection string
# - JWT secret key
# - OAuth credentials (Google, Apple, Twitch)
# - Cloudflare R2 credentials
```

### 3. Set up the Database
```bash
# Run migrations and stored procedures
# (detailed instructions in /docs/database-setup.md)
```

### 4. Configure the Frontend
```bash
cd GenshinDataApp.Frontend
npm install
```

### 5. Run in development
```bash
# Backend (from Visual Studio or terminal)
dotnet run --project GenshinDataApp.Services

# Frontend
cd GenshinDataApp.Frontend
ng serve
```

## ⚙️ Required Environment Variables

Create `appsettings.Development.json` based on `appsettings.example.json`. **Never commit this file to the repository.**

| Variable | Description |
|----------|-------------|
| `ConnectionStrings:DefaultConnection` | SQL Server connection string |
| `Jwt:Secret` | Secret key for signing JWT tokens |
| `Jwt:ExpirationMinutes` | JWT expiration time (recommended: 15) |
| `OAuth:Google:ClientId` | Google OAuth Client ID |
| `OAuth:Google:ClientSecret` | Google OAuth Client Secret |
| `OAuth:Apple:ClientId` | Apple OAuth Client ID |
| `OAuth:Apple:ClientSecret` | Apple OAuth Client Secret |
| `OAuth:Twitch:ClientId` | Twitch OAuth Client ID |
| `OAuth:Twitch:ClientSecret` | Twitch OAuth Client Secret |
| `CloudflareR2:AccountId` | Cloudflare Account ID |
| `CloudflareR2:AccessKey` | Cloudflare R2 Access Key |
| `CloudflareR2:SecretKey` | Cloudflare R2 Secret Key |
| `CloudflareR2:BucketName` | R2 Bucket Name |

## 🌿 Git Branches

| Branch | Environment | Description |
|--------|------------|-------------|
| `main` | Production | Stable, deployed code |
| `integration` | Staging | Pre-production validation |
| `develop` | Development | Main working branch |
| `feature/name` | Local | New features |
| `hotfix/name` | — | Urgent production fixes |

## 🤝 Contributing

Contributions are welcome! Please read the [contribution guidelines](CONTRIBUTING.md) before submitting a Pull Request. Make sure to follow the code conventions described in `.github/copilot-instructions.md`.

## ☕ Support the Project

If you find this project useful, you can support its development on [Buy Me a Coffee](https://buymeacoffee.com/yourusername). Donations help cover server and maintenance costs.

*Donations are completely voluntary. This project will always be free and open source.*

## 📄 License

The **source code** of this project is licensed under the [MIT License](LICENSE).

**Game assets (images, character names, lore and other Genshin Impact content) are NOT included under this license and are the exclusive property of HoYoverse Co., Ltd.**

## ⚖️ Copyright & Trademark Notice

```
This is an unofficial fan-made project.
Not affiliated with, endorsed by, or sponsored by HoYoverse.

Genshin Impact™ is a trademark of HoYoverse Co., Ltd.
© HoYoverse Co., Ltd. All rights reserved.

All game assets, characters, names and related content
are property of HoYoverse Co., Ltd.
This project is made purely for educational and community purposes,
with no commercial intent.
```

---

<br/>

# Português (Brasil)

## 📖 Descrição

**Genshin Data** é uma aplicação web de código aberto criada pela e para a comunidade de Genshin Impact. Permite aos jogadores gerenciar seu inventário de personagens, armas e artefatos, consultar informações detalhadas do jogo e criar builds otimizadas para seus personagens favoritos.

## ✨ Funcionalidades

- 📋 **Catálogo do Jogo** — Consulte informações detalhadas de personagens, armas e artefatos com custos de melhoria
- 🎒 **Inventário Pessoal** — Gerencie sua coleção de personagens, armas e artefatos
- ⚔️ **Criador de Builds** — Crie builds com drag & drop e visualize as estatísticas em tempo real
- 🔗 **Compartilhar Builds** — Compartilhe suas builds com a comunidade via link público
- 📊 **Comparador de Builds** — Compare duas builds do mesmo personagem lado a lado
- 🌍 **Multilíngue** — Disponível em Español, English e Português (Brasil)
- 🌙 **Modo Escuro/Claro** — Detecta automaticamente a preferência do sistema
- 👤 **Contas de Usuário** — Registro próprio ou acesso com Google, Apple ou Twitch

## 🛠️ Stack Tecnológico

| Camada | Tecnologia |
|--------|-----------|
| Backend | .NET Core |
| Banco de Dados | Microsoft SQL Server |
| API | ASP.NET Core Web API |
| Frontend | Angular + Tailwind CSS |
| Imagens | Cloudflare R2 |
| Deploy | Cloudflare |

## 🚀 Instalação e Configuração

### Pré-requisitos
- Visual Studio 2026
- .NET Core SDK
- Node.js e npm
- Microsoft SQL Server
- Git

### 1. Clonar o repositório
```bash
git clone https://github.com/seuusuario/GenshinDataApp.git
cd GenshinDataApp
git checkout develop
```

### 2. Configurar o Backend
```bash
# Copie o arquivo de configuração de exemplo
cp GenshinDataApp.Services/appsettings.example.json GenshinDataApp.Services/appsettings.Development.json

# Edite appsettings.Development.json com seus valores:
# - String de conexão do SQL Server
# - Chave secreta JWT
# - Credenciais OAuth (Google, Apple, Twitch)
# - Credenciais do Cloudflare R2
```

### 3. Configurar o Banco de Dados
```bash
# Execute as migrações e stored procedures
# (instruções detalhadas em /docs/database-setup.md)
```

### 4. Configurar o Frontend
```bash
cd GenshinDataApp.Frontend
npm install
```

### 5. Executar em desenvolvimento
```bash
# Backend (pelo Visual Studio ou terminal)
dotnet run --project GenshinDataApp.Services

# Frontend
cd GenshinDataApp.Frontend
ng serve
```

## ⚙️ Variáveis de Ambiente Necessárias

Crie `appsettings.Development.json` baseando-se em `appsettings.example.json`. **Nunca faça commit deste arquivo no repositório.**

| Variável | Descrição |
|----------|-----------|
| `ConnectionStrings:DefaultConnection` | String de conexão do SQL Server |
| `Jwt:Secret` | Chave secreta para assinar tokens JWT |
| `Jwt:ExpirationMinutes` | Tempo de expiração do JWT (recomendado: 15) |
| `OAuth:Google:ClientId` | Client ID do Google OAuth |
| `OAuth:Google:ClientSecret` | Client Secret do Google OAuth |
| `OAuth:Apple:ClientId` | Client ID do Apple OAuth |
| `OAuth:Apple:ClientSecret` | Client Secret do Apple OAuth |
| `OAuth:Twitch:ClientId` | Client ID do Twitch OAuth |
| `OAuth:Twitch:ClientSecret` | Client Secret do Twitch OAuth |
| `CloudflareR2:AccountId` | ID da conta Cloudflare |
| `CloudflareR2:AccessKey` | Access Key do Cloudflare R2 |
| `CloudflareR2:SecretKey` | Secret Key do Cloudflare R2 |
| `CloudflareR2:BucketName` | Nome do bucket R2 |

## 🌿 Branches Git

| Branch | Ambiente | Descrição |
|--------|---------|-----------|
| `main` | Produção | Código estável e implantado |
| `integration` | Integração | Validação pré-produção |
| `develop` | Desenvolvimento | Branch principal de trabalho |
| `feature/nome` | Local | Novas funcionalidades |
| `hotfix/nome` | — | Correções urgentes |

## 🤝 Contribuir

Contribuições são bem-vindas! Por favor leia as [diretrizes de contribuição](CONTRIBUTING.md) antes de enviar um Pull Request. Certifique-se de seguir as convenções de código descritas em `.github/copilot-instructions.md`.

## ☕ Apoiar o Projeto

Se este projeto for útil para você, pode apoiar seu desenvolvimento no [Buy Me a Coffee](https://buymeacoffee.com/seuusuario). As doações ajudam a cobrir os custos de servidor e manutenção.

*As doações são completamente voluntárias. Este projeto sempre será gratuito e de código aberto.*

## 📄 Licença

O **código-fonte** deste projeto está licenciado sob a [Licença MIT](LICENSE).

**Os assets do jogo (imagens, nomes de personagens, lore e outros conteúdos de Genshin Impact) NÃO estão incluídos nesta licença e são propriedade exclusiva da HoYoverse Co., Ltd.**

## ⚖️ Aviso de Copyright e Marca Registrada

```
Este é um projeto não oficial criado por fãs.
Não é afiliado, endossado ou patrocinado pela HoYoverse.

Genshin Impact™ é uma marca registrada da HoYoverse Co., Ltd.
© HoYoverse Co., Ltd. Todos os direitos reservados.

Todos os assets do jogo, personagens, nomes e conteúdo relacionado
são propriedade da HoYoverse Co., Ltd.
Este projeto é feito puramente para fins educacionais e comunitários,
sem intenção comercial.
```

---

<div align="center">

Made with ❤️ by the Genshin Impact community

**[⬆ Back to top](#genshindataapp--genshin-data)**

</div>
