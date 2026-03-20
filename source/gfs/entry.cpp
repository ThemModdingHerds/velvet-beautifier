#include <TMH/GFS/Entry.hpp>
#include <TMH/IO/PascalString.hpp>

namespace TMH
{
    void GFS::writeEntry(::std::ostream &stream,const Entry &entry)
    {
        IO::writePascalStringBE<::std::uint64_t>(stream,entry.filepath);
        IO::writeBigEndian(stream,entry.size);
        IO::writeBigEndian(stream,entry.alignment);
    }
    GFS::Entry GFS::readEntry(::std::istream &stream)
    {
        Entry entry;
        entry.filepath = IO::readPascalStringBE<::std::uint64_t,char>(stream);
        entry.size = IO::readBigEndian<::std::uint64_t>(stream);
        entry.alignment = IO::readBigEndian<::std::int32_t>(stream);
        return entry;
    }
    void GFS::writeEntries(::std::ostream &stream,const Entries &entries)
    {
        for(const Entry &entry : entries)
            writeEntry(stream,entry);
    }
    GFS::Entries GFS::readEntries(::std::istream &stream,::std::uint64_t count)
    {
        Entries entries;
        for(::std::uint64_t i = 0;i < count;i++)
            entries.push_back(readEntry(stream));
        return entries;
    }
}