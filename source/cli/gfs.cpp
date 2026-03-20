#include <TMH/GFS.hpp>

#include <iostream>
#include <fstream>
#include <filesystem>

#define OPEN(filepath) ::std::ifstream(filepath,::std::ios::binary)

int display(const char* filepath)
{
    ::std::ifstream file = OPEN(filepath);
    if(!file.is_open())
    {
        ::std::cerr << "failed to open " << filepath << ::std::endl;
        return EXIT_FAILURE;
    }

    try
    {
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
    }
    catch(const ::std::exception &e)
    {
        ::std::cerr << "failed to parse " << filepath << "! Reason: " << e.what() << ::std::endl;
    file.close();
        return EXIT_FAILURE;
    }
    catch(...)
    {
        ::std::cerr << "failed to parse " << filepath << ::std::endl;
    file.close();
        return EXIT_FAILURE;
    }
    file.close();
    return EXIT_SUCCESS;
}

int unpack(const char* filepath)
{
    ::std::filesystem::path filep = ::std::filesystem::current_path() / filepath;
    ::std::ifstream file = OPEN(filep);
    if(!file.is_open())
    {
        ::std::cerr << "failed to open " << filepath << ::std::endl;
        return EXIT_FAILURE;
    }
    ::TMH::GFS::Header header = ::TMH::GFS::readHeader(file);
    ::TMH::GFS::Entries entries = ::TMH::GFS::readEntries(file,header.entries);

    ::std::uint64_t offset = header.dataOffset;
    for(::std::uint64_t i = 0;i < entries.size();i++)
    {
        const ::TMH::GFS::Entry &entry = entries.at(i);
        offset += (entry.alignment - (offset % entry.size)) % entry.alignment;
        ::std::filesystem::path entrypath = filep / entry.filepath;
        ::std::filesystem::create_directory(entrypath.parent_path());
        ::std::ofstream entryfile(entrypath);
        file.seekg(offset);
        char* data = new char[entry.size];
        file.read(data,entry.size);
        entryfile.write(data,entry.size);
        delete [] data;
        entryfile.close();
        offset += entry.size;
    }
    file.close();
    return EXIT_SUCCESS;
}

int main(int argc,char** argv)
{
    return unpack("G:\\ThemModdingHerds\\VelvetBeautifier\\build\\tests\\test.gfs");
    if(argc != 3)
    {
        ::std::cerr << "Usage: <pack|unpack|display> <filepath>" << ::std::endl;
        return EXIT_FAILURE;
    }
    const char* command = argv[1];
    const char* filepath = argv[2];

    if(::std::strcmp(command,"display") == 0)
        return display(filepath);
    if(::std::strcmp(command,"unpack") == 0)
        return unpack(filepath);

    ::std::cerr << "unknown command '" << command << '\'' << ::std::endl;
    return EXIT_FAILURE;
}