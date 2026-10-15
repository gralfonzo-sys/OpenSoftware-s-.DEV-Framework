cmake_minimum_required(VERSION 3.16)
project(DevFramework CXX)

option(USE_QT "Enable Qt integration" OFF)
set(CMAKE_CXX_STANDARD 17)
set(CMAKE_CXX_STANDARD_REQUIRED ON)

add_subdirectory(src/core)
add_subdirectory(src/host)
add_subdirectory(samples/sample_plugin)
add_subdirectory(tests)

message(STATUS "USE_QT=${USE_QT}")
