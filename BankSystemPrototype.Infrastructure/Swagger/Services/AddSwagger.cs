using System;
using System.IO;
using BankSystemPrototype.Infrastructure.Version.Models;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.OpenApi.Models;
using BankSystemPrototype.Infrastructure.Swagger.StartupFilters;

namespace BankSystemPrototype.Infrastructure.Swagger.Services
{
    public static class AddSwagger
    {
        public static IServiceCollection AddSwaggerService(this IServiceCollection services)
        {
            services.AddSingleton<IStartupFilter, SwaggerStartupFilter>();
            services.AddSwaggerGen(options =>
            {
                var version = new VersionModel();
                options.SwaggerDoc("v1",
                    new OpenApiInfo
                    {
                        Title = version.Name,
                        Version = version.Version
                    });

                options.CustomSchemaIds(x => x.FullName);

                var xmlFileName = version.Name + ".xml";
                var xmlFilePath = Path.Combine(AppContext.BaseDirectory, xmlFileName);
                options.IncludeXmlComments(xmlFilePath);
            });

            return services;
        }
    }
}
