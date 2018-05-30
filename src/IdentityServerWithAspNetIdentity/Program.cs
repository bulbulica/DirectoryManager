using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using IdentityServer.Core.Shared;
using IdentityServer.Persistence;
using Microsoft.AspNetCore;
using Microsoft.AspNetCore.Hosting;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using Serilog;
using Serilog.Events;
using Serilog.Sinks.SystemConsole.Themes;

namespace IdentityServerWithAspNetIdentity
{
    public class Program
    {
        public static void Main(string[] args)
        {
            Console.Title = "Identity Server";

            var host = BuildWebHost(args);

            using (var scope = host.Services.CreateScope())
            {
                var dataService = scope.ServiceProvider.GetService<IPersistenceContext>();
                if (dataService != null)
                {
                 dataService.InitializeData(scope.ServiceProvider);
                }

                var authService = scope.ServiceProvider.GetService<IAuthentication>();
                if (authService != null)
                {
                   authService.InitializeData(scope.ServiceProvider);
                }
            }
            host.Run();
        }

        public static IWebHost BuildWebHost(string[] args)
        {
            Log.Logger = new LoggerConfiguration()
                .MinimumLevel.Debug()
                .MinimumLevel.Override("Microsoft", LogEventLevel.Warning)
                .MinimumLevel.Override("System", LogEventLevel.Warning)
                .MinimumLevel.Override("Microsoft.AspNetCore.Authentication", LogEventLevel.Information)
                .Enrich.FromLogContext()
                .WriteTo.File(@"identityserver4_log.txt")
                .WriteTo.Console(outputTemplate: "[{Timestamp:HH:mm:ss} {Level}] {SourceContext}{NewLine}{Message:lj}{NewLine}{Exception}{NewLine}", theme: AnsiConsoleTheme.Literate)
                .CreateLogger();

            return WebHost.CreateDefaultBuilder(args)
                    .UseStartup<Startup>()
                    .ConfigureLogging(builder =>
                    {
                        builder.ClearProviders();
                        builder.AddSerilog();
                    })
                    .Build();
        }
    }
}
