set(NAME_PREFIX test)

macro(tmh_test Name MainCPP)
    set(NAME ${NAME_PREFIX}_${Name})
    add_executable(${NAME} ${MainCPP})
    set_target_properties(${NAME}
        PROPERTIES
        RUNTIME_OUTPUT_NAME tests/${NAME}
    )
    target_include_directories(${NAME} PRIVATE ../include)
endmacro(tmh_test)

# IO tests
tmh_test(io_bigendian bigendian.cpp)