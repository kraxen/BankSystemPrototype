using BankSystemPrototype.ApplicationServices.DataContex;
using BankSystemPrototype.Infrastructure.Exceptions.Filters;
using BankSystemPrototype.Infrastructure.Logs.StartupFilters;
using BankSystemPrototype.Infrastructure.Swagger.Services;
using BankSystemPrototype.Infrastructure.Version.StartupFilters;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;

namespace BankSystemPrototype.Infrastructure.Extensions
{
    public static class HostBuilderExtensions
    {
        public static IHostBuilder AddInfrastructure(this IHostBuilder builder)
        {
            builder.ConfigureServices(services =>
            {
                //services.AddSingleton<IStartupFilter, LogsStartupFilter>();
                services.AddSwaggerService();
                services.AddSingleton<IStartupFilter, VersionStartupFilter>();
                services.AddControllers(options => options.Filters.Add<GlobalExceptionFilter>());
                services.AddTransient<IBankSystemDataContext, BankSystemEF>();
            });
            return builder;
        }
    }
}
