using System;
using System.Text.Json;
using BankSystemPrototype.Infrastructure.Version.Models;
using Microsoft.AspNetCore.Builder;
using Microsoft.AspNetCore.Hosting;
using Microsoft.AspNetCore.Http;

namespace BankSystemPrototype.Infrastructure.Version.StartupFilters
{
    public sealed class VersionStartupFilter : IStartupFilter
    {
        public Action<IApplicationBuilder> Configure(Action<IApplicationBuilder> next)
        {
            return app =>
            {
                app.Map("/version", b => b.Run(c => c.Response.WriteAsync(JsonSerializer.Serialize(new VersionModel()))));
                next(app);
            };
        }
    }
}
