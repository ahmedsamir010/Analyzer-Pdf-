using System.Reflection;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;

namespace API.ExtensionsMethods
{
    public static class SwaggerServicesExtensions
    {
        public static IServiceCollection AddSwaggerServices(this IServiceCollection services)
        {
            services.AddSwaggerGen(c =>
            {
                var xmlFile = $"{Assembly.GetExecutingAssembly().GetName().Name}.xml";
                var xmlPath = Path.Combine(AppContext.BaseDirectory, xmlFile);
                if (File.Exists(xmlPath))
                {
                    c.IncludeXmlComments(xmlPath);
                }
                c.SwaggerDoc("v1", new OpenApiInfo
                {
                    Title = "Task Allyant API",
                    Version = "v1",
                    Description = "API for analyzing PDF files in ZIP archives for keyword occurrences."
                });
            });
            return services;
        }
    }
}