using System;
using Microsoft.Extensions.DependencyInjection;
using System.Threading.Tasks;

namespace Dev.Framework.Contracts
{
    /// <summary>
    /// Contrato mínimo para un plugin de .DEV Framework.
    /// </summary>
    public interface IPlugin
    {
        /// <summary>
        /// Nombre del plugin.
        /// </summary>
        string Name { get; }

        /// <summary>
        /// Registrar servicios en el contenedor DI del host.
        /// </summary>
        /// <param name="services"></param>
        void ConfigureServices(IServiceCollection services);

        /// <summary>
        /// Inicialización asíncrona tras construcción del IServiceProvider.
        /// </summary>
        /// <param name="serviceProvider"></param>
        /// <returns></returns>
        Task InitializeAsync(IServiceProvider serviceProvider);
    }
}
