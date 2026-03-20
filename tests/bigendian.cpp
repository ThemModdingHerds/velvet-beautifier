#include <TMH/IO/Endianness.hpp>

#include <cstdint>
#include <cassert>
#include <cstdlib>

int main(int argc,char** argv)
{
    ::std::int16_t s16 = -22275;
    assert(TMH::IO::swapEndian(s16) == -600);
    ::std::uint16_t u16 = 22530;
    assert(TMH::IO::swapEndian(u16) == 600);
    ::std::int32_t s32 = -2132411905;
    assert(TMH::IO::swapEndian(s32) == -400000);
    ::std::uint32_t u32 = 2149189120;
    assert(TMH::IO::swapEndian(u32) == 400000);
    ::std::int64_t s64 = 4625361744053665791;
    assert(TMH::IO::swapEndian(s64) == -7000000);
    ::std::uint64_t u64 = 53093651290521600;
    assert(TMH::IO::swapEndian(u64) == 6000000000);

    return EXIT_SUCCESS;
}