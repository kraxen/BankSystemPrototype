using System;
using BankSystemPrototype.Infrastructure.Logs.Middlewares;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;

namespace BankSystemPrototype.Infrastructure.Logs.StartupFilters
{
    public sealed class LogsStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.UseMiddleware<LoggingMiddleware>();
                next(app);
            };
        }
    }
}
