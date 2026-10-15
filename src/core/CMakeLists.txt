add_library(devframework_core STATIC
	src/PluginLoader.cpp
)

target_include_directories(devframework_core
	PUBLIC
		$<BUILD_INTERFACE:${CMAKE_CURRENT_SOURCE_DIR}/include>
)

if(MSVC)
	target_compile_definitions(devframework_core PRIVATE "_CRT_SECURE_NO_WARNINGS")
endif()

target_compile_features(devframework_core PUBLIC cxx_std_17)

# expose header-only API via include folder
install(TARGETS devframework_core EXPORT DevFrameworkTargets)
install(DIRECTORY include/ DESTINATION include)
