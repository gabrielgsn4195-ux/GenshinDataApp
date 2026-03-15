using GenshinDataApp.Backend.Data;
using GenshinDataApp.Backend.Repositories;
using GenshinDataApp.Infrastructure.Security;
using Microsoft.AspNetCore.Authentication.JwtBearer;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using Serilog;
using System.Text;

var builder = WebApplication.CreateBuilder(args);

// Configure Serilog
Log.Logger = new LoggerConfiguration()
    .ReadFrom.Configuration(builder.Configuration)
    .Enrich.FromLogContext()
    .WriteTo.Console()
    .WriteTo.File("logs/genshindata-.log", rollingInterval: RollingInterval.Day)
    .CreateLogger();

builder.Host.UseSerilog();

// Configure JWT Settings
var jwtSettings = new JwtSettings();
builder.Configuration.GetSection("Jwt").Bind(jwtSettings);
builder.Services.AddSingleton(jwtSettings);

// Add DbContext
builder.Services.AddDbContext<GenshinDbContext>(options =>
    options.UseSqlServer(builder.Configuration.GetConnectionString("DefaultConnection")));

// Add Infrastructure services
builder.Services.AddScoped<IJwtHelper, JwtHelper>();
builder.Services.AddScoped<IPasswordHasher, PasswordHasher>();

// Add repositories
builder.Services.AddScoped<IUserRepository, UserRepository>();
builder.Services.AddScoped<IRefreshTokenRepository, RefreshTokenRepository>();

// Add Authentication
builder.Services.AddAuthentication(options =>
{
    options.DefaultAuthenticateScheme = JwtBearerDefaults.AuthenticationScheme;
    options.DefaultChallengeScheme = JwtBearerDefaults.AuthenticationScheme;
})
.AddJwtBearer(options =>
{
    options.TokenValidationParameters = new TokenValidationParameters
    {
        ValidateIssuer = true,
        ValidateAudience = true,
        ValidateLifetime = true,
        ValidateIssuerSigningKey = true,
        ValidIssuer = jwtSettings.Issuer,
        ValidAudience = jwtSettings.Audience,
        IssuerSigningKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(jwtSettings.Secret)),
        ClockSkew = TimeSpan.Zero
    };
});

builder.Services.AddAuthorization();

// Add CORS
builder.Services.AddCors(options =>
{
    options.AddPolicy("AllowFrontend", policy =>
    {
        if (builder.Environment.IsDevelopment())
        {
            // En Development: permitir cualquier origen (para Swagger)
            policy.AllowAnyOrigin()
                  .AllowAnyHeader()
                  .AllowAnyMethod();
        }
        else
        {
            // En Production: solo Angular
            policy.WithOrigins("http://localhost:4200", "https://yourdomain.com")
                  .AllowAnyHeader()
                  .AllowAnyMethod()
                  .AllowCredentials();
        }
    });
});

builder.Services.AddControllers();
builder.Services.AddEndpointsApiExplorer();
builder.Services.AddSwaggerGen();

var app = builder.Build();

// Configure the HTTP request pipeline.
if (app.Environment.IsDevelopment())
{
    app.UseSwagger();
    app.UseSwaggerUI(c =>
    {
        c.DocumentTitle = "GenshinDataApp API";

        // Interceptor de Swagger UI para inyectar header Authorization
        var interceptorJs = "function(req){var t=localStorage.getItem('jwt_token');if(t&&req.url.indexOf('/api/')!==-1){req.headers={...req.headers,Authorization:'Bearer '+t};}return req;}";
        c.UseRequestInterceptor(interceptorJs);

        // Habilitar persistencia de autorización
        c.ConfigObject.AdditionalItems["persistAuthorization"] = true;

        // Añadir script personalizado para gestión de token
        c.HeadContent = @"
            <style>
                #jwt-toolbar {
                    position: fixed;
                    top: 10px;
                    right: 20px;
                    z-index: 9999;
                    display: flex;
                    gap: 10px;
                    align-items: center;
                    background: rgba(0, 0, 0, 0.8);
                    padding: 10px 15px;
                    border-radius: 8px;
                    box-shadow: 0 4px 6px rgba(0, 0, 0, 0.3);
                }
                .jwt-btn {
                    padding: 8px 16px;
                    border: none;
                    border-radius: 4px;
                    cursor: pointer;
                    font-weight: bold;
                    font-size: 14px;
                    transition: all 0.3s;
                }
                .jwt-btn:hover {
                    transform: translateY(-2px);
                    box-shadow: 0 4px 8px rgba(0, 0, 0, 0.3);
                }
                #btn-set-token {
                    background: #4990e2;
                    color: white;
                }
                #btn-clear-token {
                    background: #e24949;
                    color: white;
                }
                #token-indicator {
                    color: #49e249;
                    font-weight: bold;
                    font-size: 14px;
                }
            </style>
            <script>
                function setJwtToken() {
                    const token = prompt('Ingrese el JWT token (sin ""Bearer""):');
                    if (token && token.trim()) {
                        localStorage.setItem('jwt_token', token.trim());
                        alert('✅ Token guardado exitosamente!');
                        location.reload();
                    }
                }

                function clearJwtToken() {
                    if (confirm('¿Está seguro de eliminar el token?')) {
                        localStorage.removeItem('jwt_token');
                        alert('🗑️ Token eliminado.');
                        location.reload();
                    }
                }

                function createJwtToolbar() {
                    // Crear toolbar flotante
                    const toolbar = document.createElement('div');
                    toolbar.id = 'jwt-toolbar';

                    // Botón Set Token
                    const btnSet = document.createElement('button');
                    btnSet.id = 'btn-set-token';
                    btnSet.className = 'jwt-btn';
                    btnSet.innerHTML = '🔑 Set JWT Token';
                    btnSet.onclick = setJwtToken;
                    toolbar.appendChild(btnSet);

                    // Botón Clear Token
                    const btnClear = document.createElement('button');
                    btnClear.id = 'btn-clear-token';
                    btnClear.className = 'jwt-btn';
                    btnClear.innerHTML = '🗑️ Clear Token';
                    btnClear.onclick = clearJwtToken;
                    toolbar.appendChild(btnClear);

                    // Indicador de token activo
                    const token = localStorage.getItem('jwt_token');
                    if (token) {
                        const indicator = document.createElement('span');
                        indicator.id = 'token-indicator';
                        indicator.innerHTML = '✓ Token Activo';
                        toolbar.appendChild(indicator);
                    }

                    document.body.appendChild(toolbar);
                    console.log('✅ JWT Toolbar cargado correctamente');
                }

                // Intentar cargar cuando el DOM esté listo
                if (document.readyState === 'loading') {
                    document.addEventListener('DOMContentLoaded', createJwtToolbar);
                } else {
                    createJwtToolbar();
                }

                // Backup: intentar después de 1 segundo si no se cargó
                setTimeout(function() {
                    if (!document.getElementById('jwt-toolbar')) {
                        createJwtToolbar();
                    }
                }, 1000);
            </script>";
    });
}

// HTTPS enforcement: Always redirect HTTP to HTTPS
app.UseHttpsRedirection();

app.UseCors("AllowFrontend");
app.UseAuthentication();
app.UseAuthorization();
app.MapControllers();

app.Run();

