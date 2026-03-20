set(NAME_PREFIX test)

# Big Endian test
set(BIG_ENDIAN_TEST ${NAME_PREFIX}_io_bigendian)
add_executable(${BIG_ENDIAN_TEST} bigendian.cpp)
target_include_directories(${BIG_ENDIAN_TEST} PRIVATE ../include)
# GFS test
set(GFS_TEST ${NAME_PREFIX}_gfs)
add_executable(${GFS_TEST} gfs.cpp)
target_include_directories(${GFS_TEST} PRIVATE ../include)
target_link_libraries(${GFS_TEST} PRIVATE ${PROJECT_NAME}.GFS)
add_dependencies(${GFS_TEST} ${PROJECT_NAME}.GFS)