#include <iostream>
#include <filesystem>
#include "../core/include/DevFramework/IPlugin.h"
#include "../core/src/PluginLoader.h"

int main(int argc, char** argv)
{
	std::string pluginsDir = "plugins";
	if (argc > 1) pluginsDir = argv[1];

	std::cout << "DevFramework Host starting. Looking for plugins in: " << pluginsDir << std::endl;

	DevFramework::PluginLoader loader;
	auto plugins = loader.loadPlugins(pluginsDir);

	for (auto p : plugins) {
		std::cout << "Initializing plugin: " << p->name() << std::endl;
		p->initialize();
	}

	std::cout << "Host running. Press Enter to shutdown..." << std::endl;
	std::cin.get();

	// Destructor of loader will shutdown and unload plugins
	return 0;
}
