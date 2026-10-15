using System;
using Microsoft.Extensions.Configuration;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dev.Framework
{
    public class PluginContext
    {
        public IServiceCollection Services { get; }
        public IConfiguration Configuration { get; }
        public ILoggerFactory LoggerFactory { get; }

        public PluginContext(IServiceCollection services, IConfiguration configuration, ILoggerFactory loggerFactory)
        {
            Services = services ?? throw new ArgumentNullException(nameof(services));
            Configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
            LoggerFactory = loggerFactory ?? throw new ArgumentNullException(nameof(loggerFactory));
        }
    }
}
