enable_testing()

include(FetchContent)
FetchContent_Declare(
  catch2
  GIT_REPOSITORY https://github.com/catchorg/Catch2.git
  GIT_TAG v2.13.9
)
FetchContent_MakeAvailable(catch2)

add_executable(pluginloader_tests
	pluginloader_tests.cpp
)

target_include_directories(pluginloader_tests PRIVATE
	${CMAKE_SOURCE_DIR}/src/core/include
)

target_link_libraries(pluginloader_tests PRIVATE devframework_core Catch2::Catch2)

add_test(NAME pluginloader_tests COMMAND pluginloader_tests)
