#pragma once

#include <string>

namespace DevFramework {

class IPlugin {
public:
	virtual ~IPlugin() = default;
	virtual std::string name() const = 0;
	// Called after construction to perform initialization
	virtual void initialize() = 0;
	// Called before unloading
	virtual void shutdown() = 0;
};

using CreatePluginFunc = IPlugin* (*)();

} // namespace DevFramework
