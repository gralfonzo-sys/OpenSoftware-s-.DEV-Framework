#pragma once

#include <string>
#include <vector>
#include <memory>
#include "../include/DevFramework/IPlugin.h"

namespace DevFramework {

class PluginLoader {
public:
	PluginLoader() = default;
	~PluginLoader();

	// Load plugins from a directory. Returns pointers to IPlugin instances.
	std::vector<IPlugin*> loadPlugins(const std::string& directory);

private:
	struct Impl;
	Impl* pImpl = nullptr;
};

} // namespace DevFramework
