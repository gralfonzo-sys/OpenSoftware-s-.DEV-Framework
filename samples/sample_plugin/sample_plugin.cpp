#include "../src/core/include/DevFramework/IPlugin.h"
#include <iostream>

using namespace DevFramework;

class SamplePlugin : public IPlugin {
public:
	std::string name() const override { return "SamplePlugin"; }
	void initialize() override {
		std::cout << "SamplePlugin initialized." << std::endl;
	}
	void shutdown() override {
		std::cout << "SamplePlugin shutdown." << std::endl;
	}
};

extern "C" IPlugin* create_plugin()
{
	return new SamplePlugin();
}
