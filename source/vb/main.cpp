#include <TMH/VB/Utilities/FileSystem.hpp>
#include <nlohmann/json.hpp>

#include <cstdlib>
#include <fstream>
#include <iostream>

int main(int argc,char** argv)
{
    ::std::filesystem::path configPath = ::TMH::VB::Utilities::FileSystem::appDataDirectory() / "config.json";
    ::std::ifstream configFile(configPath);
    ::nlohmann::json config = ::nlohmann::json::parse(configFile);
    configFile.close();
    ::std::cout << config["client_path"] << ::std::endl;
    ::std::cout << config["server_path"] << ::std::endl;
    ::std::cout << config["version"] << ::std::endl;
    return EXIT_SUCCESS;
}