#include <TMH/GFS.hpp>

#include <iostream>
#include <fstream>
#include <filesystem>

#define OPEN(filepath) ::std::ifstream(filepath,::std::ios::binary)

::std::filesystem::path resolve(const ::std::filesystem::path &path,const ::std::filesystem::path::string_type::value_type &c)
{
    ::std::filesystem::path::string_type string = path.native();
    for(::std::filesystem::path::string_type::size_type i = 0;i < string.size();i++)
    {
        if(string[i] == L'/')
            string[i] = c;
    }
    return string;
}

int display(const char* filepath)
{
    if(!::std::filesystem::is_regular_file(filepath))
    {
        ::std::cerr << filepath << " is not a file" << ::std::endl;
        return EXIT_FAILURE;
    }
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
    if(!::std::filesystem::is_regular_file(filepath))
    {
        ::std::cerr << filepath << " is not a file" << ::std::endl;
        return EXIT_FAILURE;
    }
    ::std::filesystem::path filep = ::std::filesystem::current_path() / filepath;
    ::std::ifstream file = OPEN(filep);
    ::std::filesystem::path directorypath = filep.parent_path() / filep.stem();
    if(!file.is_open())
    {
        ::std::cerr << "failed to open " << filepath << ::std::endl;
        return EXIT_FAILURE;
    }
    ::TMH::GFS::Header header = ::TMH::GFS::readHeader(file);
    ::TMH::GFS::Entries entries = ::TMH::GFS::readEntries(file,header.entries);

    ::std::uint64_t offset = header.dataOffset;
    for(const ::TMH::GFS::Entry &entry : entries)
    {
        offset += (entry.alignment - (offset % entry.alignment)) % entry.alignment;

        ::std::filesystem::path entrypath = resolve(directorypath / entry.filepath,::std::filesystem::path::preferred_separator);
        ::std::filesystem::path entryDirectoryPath = entrypath.parent_path();
        ::std::error_code error;
        ::std::cout << "extracting " << entry.filepath << " to " << entrypath << "..." << ::std::endl;
        if(
            ::std::filesystem::is_directory(entryDirectoryPath) &&
            !::std::filesystem::remove_all(entryDirectoryPath,error) &&
            error
        )
        {
            ::std::cerr << "failed to remove directory " << entryDirectoryPath << ". Reason: " << error << ::std::endl;
            file.close();
            return EXIT_FAILURE;
        }
        if(
            !::std::filesystem::is_directory(entryDirectoryPath) && 
            !::std::filesystem::create_directories(entryDirectoryPath,error) && 
            error
        )
        {
            ::std::cerr << "failed to create directory " << entryDirectoryPath << ". Reason: " << error << ::std::endl;
            file.close();
            return EXIT_FAILURE;
        }
        ::std::ofstream entryfile(entrypath,::std::ios::binary);
        file.seekg(offset);
        char* data = new char[entry.size];
        file.read(data,entry.size);
        entryfile.write(data,entry.size);
        delete [] data;
        entryfile.close();
        offset += entry.size;
    }
    ::std::cout << "extracted " << header.entries << " entries from " << filepath << ::std::endl;
    file.close();
    return EXIT_SUCCESS;
}

int pack(const char* directoryPath)
{
    ::TMH::GFS::Entries entries;
    ::std::uint32_t offset = 0;
    for(const ::std::filesystem::directory_entry &fsEntry : ::std::filesystem::recursive_directory_iterator(directoryPath))
    {
        if(!fsEntry.is_regular_file())
            continue;
        ::std::filesystem::path entryPath = ::std::filesystem::proximate(fsEntry.path(),directoryPath);
        ::TMH::GFS::Entry entry = {
            resolve(entryPath,'/').string(),
            ::std::filesystem::file_size(fsEntry),
            ::TMH::GFS::ALIGNMENT
        };
        entries.push_back(entry);
        offset += ::TMH::GFS::entrySize(entry);
    }
    ::TMH::GFS::Header header = {
        offset,
        ::TMH::GFS::IDENTIFIER,
        ::TMH::GFS::VERSION,
        entries.size()
    };
    offset = header.dataOffset = offset + ::TMH::GFS::headerSize(header);
    ::std::filesystem::path filepath = directoryPath;
    filepath.replace_extension(".gfs");
    ::std::ofstream file(filepath);
    ::TMH::GFS::writeHeader(file,header);
    ::TMH::GFS::writeEntries(file,entries);
    for(const ::TMH::GFS::Entry &entry : entries)
    {
        offset += (entry.alignment - (offset % entry.alignment)) % entry.alignment;
        ::std::filesystem::path entrypath = resolve(::std::filesystem::path(directoryPath) / entry.filepath,::std::filesystem::path::preferred_separator);
        ::std::ifstream entryfile(entrypath,::std::ios::binary);
        char* data = new char[entry.size];
        entryfile.read(data,entry.size);
        entryfile.close();
        file.seekp(offset);
        file.write(data,entry.size);
        delete [] data;
        offset += entry.size;
    }
    return EXIT_SUCCESS;
}

int main(int argc,char** argv)
{
    return pack("G:\\ThemModdingHerds\\VelvetBeautifierCPP\\build\\.cmake");
    if(argc == 2)
    {
        const char* path = argv[1];
        if(::std::filesystem::is_directory(path))
            return pack(path);
        if(::std::filesystem::is_regular_file(path))
            return unpack(path);
    }
    if(argc != 3)
    {
        ::std::cerr << "Usage: <pack|unpack|display> <filepath-or-directorypath>" << ::std::endl;
        return EXIT_FAILURE;
    }
    const char* command = argv[1];
    const char* path = argv[2];

    if(::std::strcmp(command,"display") == 0)
        return display(path);
    if(::std::strcmp(command,"unpack") == 0)
        return unpack(path);
    if(::std::strcmp(command,"pack") == 0)
        return pack(path);

    ::std::cerr << "unknown command '" << command << '\'' << ::std::endl;
    return EXIT_FAILURE;
}