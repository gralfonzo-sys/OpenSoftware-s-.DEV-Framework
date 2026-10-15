using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Reflection;
using Dev.Framework.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;
using System.Threading.Tasks;

namespace Dev.Framework
{
    public class PluginLoader
    {
        private readonly ILogger<PluginLoader> _logger;

        public PluginLoader(ILogger<PluginLoader> logger)
        {
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Carga plugins desde una carpeta (dlls) y devuelve instancias que implementen IPlugin.
        /// </summary>
        /// <param name="path">Carpeta donde buscar DLLs.</param>
        /// <returns>Instancias de IPlugin encontradas.</returns>
        public IEnumerable<IPlugin> LoadPluginsFromFolder(string path)
        {
            if (string.IsNullOrEmpty(path)) throw new ArgumentNullException(nameof(path));
            if (!Directory.Exists(path))
            {
                _logger.LogWarning("El directorio de plugins no existe: {path}", path);
                return Enumerable.Empty<IPlugin>();
            }

            var dlls = Directory.GetFiles(path, "*.dll", SearchOption.TopDirectoryOnly);
            var plugins = new List<IPlugin>();

            foreach (var dll in dlls)
            {
                try
                {
                    var asm = Assembly.LoadFrom(dll);
                    var types = asm.GetTypes().Where(t => typeof(IPlugin).IsAssignableFrom(t) && !t.IsAbstract && t.IsClass);
                    foreach (var type in types)
                    {
                        if (Activator.CreateInstance(type) is IPlugin plugin)
                        {
                            _logger.LogInformation("Plugin cargado: {plugin}", plugin.Name);
                            plugins.Add(plugin);
                        }
                    }
                }
                catch (ReflectionTypeLoadException ex)
                {
                    _logger.LogError(ex, "Error cargando tipos desde {dll}", dll);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error cargando assembly {dll}", dll);
                }
            }

            return plugins;
        }

        /// <summary>
        /// Registra los servicios de los plugins en el IServiceCollection.
        /// </summary>
        public void RegisterPluginsServices(IServiceCollection services, IEnumerable<IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                try
                {
                    plugin.ConfigureServices(services);
                    _logger.LogDebug("Servicios registrados para plugin {plugin}", plugin.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error registrando servicios del plugin {plugin}", plugin.Name);
                }
            }
        }

        /// <summary>
        /// Inicializa plugins de forma asíncrona tras construir el proveedor.
        /// </summary>
        public async Task InitializePluginsAsync(IServiceProvider serviceProvider, IEnumerable<IPlugin> plugins)
        {
            foreach (var plugin in plugins)
            {
                try
                {
                    await plugin.InitializeAsync(serviceProvider).ConfigureAwait(false);
                    _logger.LogInformation("Plugin inicializado: {plugin}", plugin.Name);
                }
                catch (Exception ex)
                {
                    _logger.LogError(ex, "Error inicializando plugin {plugin}", plugin.Name);
                }
            }
        }
    }
}
