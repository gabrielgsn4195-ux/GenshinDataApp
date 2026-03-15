using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Mvc.ApiExplorer;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Swashbuckle.AspNetCore.Swagger;
using System.Text.Json;

namespace GenshinDataApp.Services;

public class SwaggerGenerator
{
    public static async Task Main(string[] args)
    {
        if (args.Length > 0 && args[0] == "--generate-swagger")
        {
            var builder = WebApplication.CreateBuilder(args);

            builder.Services.AddControllers();
            builder.Services.AddEndpointsApiExplorer();
            builder.Services.AddSwaggerGen();

            var app = builder.Build();

            var swaggerProvider = app.Services.GetRequiredService<ISwaggerProvider>();
            var swagger = swaggerProvider.GetSwagger("v1");

            var json = JsonSerializer.Serialize(swagger, new JsonSerializerOptions
            {
                WriteIndented = true
            });

            var outputPath = Path.Combine(Directory.GetCurrentDirectory(), "swagger.json");
            await File.WriteAllTextAsync(outputPath, json);

            Console.WriteLine($"✓ Swagger JSON generated at: {outputPath}");
            Environment.Exit(0);
        }
    }
}