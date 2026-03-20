cmake_minimum_required(VERSION 4.0.0)

project(ThemModdingHerds)

set(CMAKE_CXX_STANDARD 26)

# GFS

add_library(${PROJECT_NAME}.GFS

    source/gfs/header.cpp
    source/gfs/entry.cpp
)
target_include_directories(${PROJECT_NAME}.GFS PRIVATE include)

add_executable(${PROJECT_NAME}.GFS.CLI source/cli/gfs.cpp)
target_include_directories(${PROJECT_NAME}.GFS.CLI PRIVATE include)
target_link_libraries(${PROJECT_NAME}.GFS.CLI PRIVATE ${PROJECT_NAME}.GFS)

add_subdirectory(tests)