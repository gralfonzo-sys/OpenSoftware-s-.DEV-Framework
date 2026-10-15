#include "PluginLoader.h"
#include <filesystem>
#include <iostream>

#ifdef _WIN32
  #include <windows.h>
  using LibHandle = HMODULE;
#else
  #include <dlfcn.h>
  using LibHandle = void*;
#endif

namespace fs = std::filesystem;

namespace DevFramework {

struct PluginLoader::Impl {
	std::vector<LibHandle> handles;
	std::vector<IPlugin*> instances;
};

PluginLoader::~PluginLoader()
{
	if (pImpl) {
		for (auto inst : pImpl->instances) {
			try { inst->shutdown(); } catch(...) {}
			delete inst;
		}
		for (auto h : pImpl->handles) {
#ifdef _WIN32
			FreeLibrary(h);
#else
			dlclose(h);
#endif
		}
		delete pImpl;
	}
}

static LibHandle loadLibrary(const std::string& path)
{
#ifdef _WIN32
	return LoadLibraryA(path.c_str());
#else
	return dlopen(path.c_str(), RTLD_LAZY);
#endif
}

static void* getSymbol(LibHandle h, const char* name)
{
#ifdef _WIN32
	return reinterpret_cast<void*>(GetProcAddress(h, name));
#else
	return dlsym(h, name);
#endif
}

std::vector<IPlugin*> PluginLoader::loadPlugins(const std::string& directory)
{
	if (!pImpl) pImpl = new Impl();

	std::vector<IPlugin*> result;
	try {
		if (!fs::exists(directory) || !fs::is_directory(directory)) return result;
		for (auto& entry : fs::directory_iterator(directory)) {
			if (!entry.is_regular_file()) continue;
			auto path = entry.path();
			auto ext = path.extension().string();
#ifdef _WIN32
			if (ext != ".dll") continue;
#else
			if (ext != ".so" && ext != ".dylib") continue;
#endif
			LibHandle h = loadLibrary(path.string());
			if (!h) {
#ifdef _WIN32
				std::cerr << "Failed to load " << path << "\n";
#else
				std::cerr << "Failed to load " << path << ": " << dlerror() << "\n";
#endif
				continue;
			}
			void* sym = getSymbol(h, "create_plugin");
			if (!sym) {
				std::cerr << "create_plugin symbol not found in " << path << "\n";
#ifdef _WIN32
				FreeLibrary(h);
#else
				dlclose(h);
#endif
				continue;
			}
			auto factory = reinterpret_cast<CreatePluginFunc>(sym);
			IPlugin* plugin = factory();
			if (!plugin) {
				std::cerr << "Factory returned null in " << path << "\n";
#ifdef _WIN32
				FreeLibrary(h);
#else
				dlclose(h);
#endif
				continue;
			}
			pImpl->handles.push_back(h);
			pImpl->instances.push_back(plugin);
			result.push_back(plugin);
		}
	} catch (const std::exception& ex) {
		std::cerr << "Plugin loading exception: " << ex.what() << "\n";
	}
	return result;
}

} // namespace DevFramework
