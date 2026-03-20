#ifndef __TMH_GFS_HEADER_HPP
#define __TMH_GFS_HEADER_HPP

#include <string>
#include <istream>
#include <ostream>

namespace TMH
{
    namespace GFS
    {
        struct Header
        {
            ::std::uint32_t dataOffset;
            ::std::string identifier;
            ::std::string version;
            ::std::uint64_t entries;
        };
        void writeHeader(::std::ostream &stream,const Header &header);
        Header readHeader(::std::istream &stream);
    }
}

#endif