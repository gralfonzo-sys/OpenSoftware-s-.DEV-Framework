using System;
using System.Threading.Tasks;
using Dev.Framework.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging;

namespace Dev.Framework.Plugins.Sample
{
    public class SamplePlugin : IPlugin
    {
        public string Name => "SamplePlugin";

        public void ConfigureServices(IServiceCollection services)
        {
            services.AddSingleton<ISampleService, SampleService>();
        }

        public Task InitializeAsync(IServiceProvider serviceProvider)
        {
            var logger = serviceProvider.GetService<ILogger<SamplePlugin>>();
            logger?.LogInformation("SamplePlugin initialization.");
            return Task.CompletedTask;
        }
    }

    public interface ISampleService
    {
        string Echo(string s);
    }

    public class SampleService : ISampleService
    {
        public string Echo(string s) => $"Sample: {s}";
    }
}
