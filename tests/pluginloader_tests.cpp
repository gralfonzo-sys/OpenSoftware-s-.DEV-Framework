#define CATCH_CONFIG_MAIN
#include <catch2/catch.hpp>

#include <filesystem>
#include "../src/core/include/DevFramework/IPlugin.h"
#include "../src/core/src/PluginLoader.h"

TEST_CASE("LoadPlugins_NoFolder_ReturnsEmpty") {
	DevFramework::PluginLoader loader;
	auto tmp = std::filesystem::temp_directory_path() / "nonexistent_plugin_dir_12345";
	auto vec = loader.loadPlugins(tmp.string());
	REQUIRE(vec.empty());
}

TEST_CASE("LoadPlugins_MalformedLibrary_Skips") {
	// This test only ensures the function does not throw on unexpected files.
	DevFramework::PluginLoader loader;
	auto tmp = std::filesystem::temp_directory_path() / "test_plugin_dir";
	std::filesystem::create_directories(tmp);
	auto file = tmp / "not_a_lib.txt";
	std::ofstream ofs(file);
	ofs << "hello";
	ofs.close();

	REQUIRE_NOTHROW(loader.loadPlugins(tmp.string()));

	std::filesystem::remove_all(tmp);
}
