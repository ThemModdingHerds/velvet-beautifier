#include <TMH/GFS.hpp>

#include <fstream>
#include <iostream>

int main(int argc,char** argv)
{
    ::std::ifstream file("test.gfs",::std::ios::binary);

    ::TMH::GFS::Header hds = ::TMH::GFS::readHeader(file);

    ::std::cout << "dataOffset: " << hds.dataOffset << ::std::endl;
    ::std::cout << "identifier: '" << hds.identifier << '\'' << ::std::endl;
    ::std::cout << "version: '" << hds.version << '\'' << ::std::endl;
    ::std::cout << "entries: " << hds.entries << ::std::endl;

    ::TMH::GFS::Entries entries = ::TMH::GFS::readEntries(file,hds.entries);

    for(::TMH::GFS::Entry &entry : entries)
    {
        ::std::cout << "path: '" << entry.filepath << '\'' << ::std::endl;
        ::std::cout << "size: " << entry.size << ::std::endl;
        ::std::cout << "alignment: " << entry.alignment << ::std::endl << ::std::endl;
    }

    return EXIT_SUCCESS;
}