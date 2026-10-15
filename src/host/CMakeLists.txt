add_executable(DevFrameworkHost
	main.cpp
)

target_include_directories(DevFrameworkHost PRIVATE
	${CMAKE_SOURCE_DIR}/src/core/include
)

target_link_libraries(DevFrameworkHost PRIVATE devframework_core)

target_compile_features(DevFrameworkHost PRIVATE cxx_std_17)

install(TARGETS DevFrameworkHost DESTINATION bin)
