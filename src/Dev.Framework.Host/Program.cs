using System;
using System.IO;
using System.Threading.Tasks;
using Dev.Framework;
using Dev.Framework.Contracts;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;

namespace Dev.Framework.Host
{
    internal static class Program
    {
        public static async Task<int> Main(string[] args)
        {
            var pluginsPath = Path.Combine(AppContext.BaseDirectory, "plugins");

            var builder = Host.CreateDefaultBuilder(args)
                .ConfigureAppConfiguration((context, config) =>
                {
                    config.AddJsonFile("appsettings.json", optional: true);
                })
                .ConfigureLogging((context, logging) =>
                {
                    logging.AddConsole();
                })
                .ConfigureServices((context, services) =>
                {
                    // registrar PluginLoader con un logger temporal
                    services.AddSingleton<PluginLoader>();

                    // Cargar plugins antes de construir el provider para permitir que registren servicios
                    var loggerFactory = LoggerFactory.Create(lb => lb.AddConsole());
                    var loader = new PluginLoader(loggerFactory.CreateLogger<PluginLoader>());
                    loggerFactory.CreateLogger<Program>().LogInformation("Buscando plugins en: {path}", pluginsPath);

                    var plugins = loader.LoadPluginsFromFolder(pluginsPath).ToArray();

                    // Permitir que los plugins registren sus servicios en el IServiceCollection
                    loader.RegisterPluginsServices(services, plugins);

                    // Registrar la lista de plugins para que el host pueda inicializarlos posteriormente
                    services.AddSingleton<IEnumerable<IPlugin>>(plugins);
                });

            using var host = builder.Build();

            var logger = host.Services.GetRequiredService<ILogger<Program>>();
            var loaderInstance = host.Services.GetRequiredService<PluginLoader>();
            var pluginsList = host.Services.GetService<IEnumerable<IPlugin>>() ?? Array.Empty<IPlugin>();

            // Inicializar plugins con el proveedor construido
            await loaderInstance.InitializePluginsAsync(host.Services, pluginsList).ConfigureAwait(false);

            logger.LogInformation("Host finalizado.");
            return 0;
        }
    }
}
