using System;
using System.IO;
using System.Linq;
using Dev.Framework;
using Dev.Framework.Contracts;
using Microsoft.Extensions.DependencyInjection;
using Microsoft.Extensions.Logging.Abstractions;
using Xunit;

namespace Dev.Framework.Tests
{
    public class PluginLoaderTests
    {
        [Fact]
        public void LoadPluginsFromFolder_NoFolder_ReturnsEmpty()
        {
            var logger = new NullLogger<PluginLoader>();
            var loader = new PluginLoader(logger);
            var tmp = Path.Combine(Path.GetTempPath(), Guid.NewGuid().ToString());

            var plugins = loader.LoadPluginsFromFolder(tmp);
            Assert.Empty(plugins);
        }

        [Fact]
        public void RegisterPluginsServices_CallsConfigureServices()
        {
            var logger = new NullLogger<PluginLoader>();
            var loader = new PluginLoader(logger);

            var services = new ServiceCollection();

            var fakePlugin = new FakePlugin();
            loader.RegisterPluginsServices(services, new IPlugin[] { fakePlugin });

            var provider = services.BuildServiceProvider();
            var value = provider.GetService<string>();
            Assert.Equal("configured", value);
        }

        private class FakePlugin : IPlugin
        {
            public string Name => "fake";
            public void ConfigureServices(IServiceCollection services)
            {
                services.AddSingleton<string>("configured");
            }

            public System.Threading.Tasks.Task InitializeAsync(IServiceProvider serviceProvider)
            {
                return System.Threading.Tasks.Task.CompletedTask;
            }
        }
    }
}
