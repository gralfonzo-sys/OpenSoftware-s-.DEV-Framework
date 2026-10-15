add_library(sample_plugin SHARED
	sample_plugin.cpp
)

target_include_directories(sample_plugin PUBLIC
	${CMAKE_SOURCE_DIR}/src/core/include
)

target_compile_features(sample_plugin PUBLIC cxx_std_17)

# Ensure plugin filename.extension is appropriate on Windows
set_target_properties(sample_plugin PROPERTIES PREFIX "" )

# After build, copy to host plugins dir
add_custom_command(TARGET sample_plugin POST_BUILD
	COMMAND ${CMAKE_COMMAND} -E make_directory ${CMAKE_BINARY_DIR}/src/host/plugins
	COMMAND ${CMAKE_COMMAND} -E copy $<TARGET_FILE:sample_plugin> ${CMAKE_BINARY_DIR}/src/host/plugins/
)

install(TARGETS sample_plugin DESTINATION plugins)
