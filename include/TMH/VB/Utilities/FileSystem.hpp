#ifndef __TMH_VB_UTILITIES_FILESYSTEM_HPP
#define __TMH_VB_UTILITIES_FILESYSTEM_HPP

#include <filesystem>

namespace TMH
{
    namespace VB
    {
        namespace Utilities
        {
            namespace FileSystem
            {
                ::std::filesystem::path appDataDirectory();
            }
        }
    }
}

#endif