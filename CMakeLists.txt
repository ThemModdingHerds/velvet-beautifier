cmake_minimum_required(VERSION 4.0.0)

include(FetchContent)

project(ThemModdingHerds VERSION 2.5.1)

# Project options

# CMake setup

set(CMAKE_CXX_STANDARD 26)

set(CMAKE_RUNTIME_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/bin)
set(CMAKE_LIBRARY_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/lib)
set(CMAKE_ARCHIVE_OUTPUT_DIRECTORY ${CMAKE_CURRENT_SOURCE_DIR}/lib)

# 3rd-Party

## nlohmann_json

FetchContent_Declare(
    nlohmann_json
    GIT_REPOSITORY https://github.com/nlohmann/json
    GIT_TAG v3.12.0
)
FetchContent_MakeAvailable(nlohmann_json)

## cpp-httplib

FetchContent_Declare(
    cpp-httplib
    GIT_REPOSITORY https://github.com/yhirose/cpp-httplib
    GIT_TAG v0.38.0
)
FetchContent_MakeAvailable(cpp-httplib)

# GFS

add_library(${PROJECT_NAME}.GFS

    source/gfs/header.cpp
    source/gfs/entry.cpp
)
target_include_directories(${PROJECT_NAME}.GFS PRIVATE include)

add_executable(${PROJECT_NAME}.GFS.CLI source/cli/gfs.cpp)
target_include_directories(${PROJECT_NAME}.GFS.CLI PRIVATE include)
target_link_libraries(${PROJECT_NAME}.GFS.CLI PRIVATE ${PROJECT_NAME}.GFS)
if(WIN32)
    target_sources(${PROJECT_NAME}.GFS.CLI PRIVATE source/win32/tool.rc)
endif()
configure_file(${CMAKE_CURRENT_SOURCE_DIR}/cmake/Configuration.hpp.in include/TMH/VB/Configuration.hpp)

# Velvet Beautifier

add_executable(${PROJECT_NAME}.VelvetBeautifier

    source/vb/main.cpp

    source/vb/utilities/fs.cpp
)
if(WIN32)
    target_sources(${PROJECT_NAME}.VelvetBeautifier PRIVATE source/win32/velvetbeautifier.rc)
endif()
target_include_directories(${PROJECT_NAME}.VelvetBeautifier PRIVATE include ${CMAKE_CURRENT_BINARY_DIR}/include)
target_link_libraries(${PROJECT_NAME}.VelvetBeautifier PRIVATE
    ${PROJECT_NAME}.GFS

    nlohmann_json::nlohmann_json
)
add_dependencies(${PROJECT_NAME}.VelvetBeautifier ${PROJECT_NAME}.GFS nlohmann_json::nlohmann_json)

add_subdirectory(tests)