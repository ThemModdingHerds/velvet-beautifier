#ifndef __TMH_GFS_ENTRY_HPP
#define __TMH_GFS_ENTRY_HPP

#include <string>
#include <vector>

namespace TMH
{
    namespace GFS
    {
        const ::std::int32_t ALIGNMENT = 4096;
        struct Entry
        {
            ::std::string filepath;
            ::std::uint64_t size;
            ::std::int32_t alignment;
        };
        typedef ::std::vector<Entry> Entries;
        void writeEntry(::std::ostream &stream,const Entry &entry);
        Entry readEntry(::std::istream &stream);
        void writeEntries(::std::ostream &stream,const Entries &entries);
        Entries readEntries(::std::istream &stream,::std::uint64_t count);
        ::std::uint64_t entrySize(const Entry &entry);
    }
}

#endif