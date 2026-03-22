#include <TMH/VB/Utilities/FileSystem.hpp>
#include <TMH/VB/Configuration.hpp>

#ifdef _WIN32
    #include <Shlobj_core.h>
#endif

namespace TMH
{
    namespace VB
    {
        namespace Utilities
        {
            ::std::filesystem::path FileSystem::appDataDirectory()
            {
#ifdef _WIN32
                PWSTR path;
                HRESULT result = SHGetKnownFolderPath(
                    FOLDERID_LocalAppData,
                    0,
                    NULL,
                    &path
                );
                ::std::filesystem::path appdata = path;
                appdata /= TMH_VB_ALTNAME;
                CoTaskMemFree(path);
                return appdata;
#else
    #error Target OS does not have a appdata directory
#endif
            }
        }
    }
}