#include <TMH/GFS/Header.hpp>
#include <TMH/IO/Endianness.hpp>
#include <TMH/IO/PascalString.hpp>

namespace TMH
{
    void GFS::writeHeader(::std::ostream &stream,const Header &header)
    {
        IO::writeBigEndian(stream,header.dataOffset);
        IO::writePascalStringBE<::std::uint64_t>(stream,header.identifier);
        IO::writePascalStringBE<::std::uint64_t>(stream,header.version);
        IO::writeBigEndian(stream,header.entries);
    }
    GFS::Header GFS::readHeader(::std::istream &stream)
    {
        Header hds;
        hds.dataOffset = IO::readBigEndian<::std::uint32_t>(stream);
        hds.identifier = IO::readPascalStringBE<::std::uint64_t,char>(stream);
        hds.version = IO::readPascalStringBE<::std::uint64_t,char>(stream);
        hds.entries = IO::readBigEndian<::std::uint64_t>(stream);
        return hds;
    }
    ::std::uint64_t GFS::headerSize(const Header &header)
    {
        return sizeof(header.dataOffset) +
            sizeof(::std::uint64_t) + header.identifier.size() +
            sizeof(::std::uint64_t) + header.version.size() +
            sizeof(header.entries);
    }
}