using System.Net.NetworkInformation;
using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Zip;
using SharpCompress.Compressors.BZip2;
using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class ArchiveUtils
{
    private static bool IsRevergePackage(Stream file)
    {
        try
        {
            using Reader reader = new(file);
            RevergePackageHeader header = reader.ReadRevergePackageHeader();
            return header.Identifier == RevergePackageHeader.IDENTIFIER;
        }
        catch(Exception)
        {
            return false;
        }
    }
    public static ArchiveType DetectArchive(Stream stream)
    {
        if(ZipArchive.IsZipFile(stream))
            return ArchiveType.Zip;
        if(RarArchive.IsRarFile(stream))
            return ArchiveType.Rar;
        if(SevenZipArchive.IsSevenZipFile(stream))
            return ArchiveType.SevenZip;
        if(TarArchive.IsTarFile(stream))
            return ArchiveType.Tar;
        if(GZipArchive.IsGZipFile(stream))
            return ArchiveType.GZip;
        if(IsRevergePackage(stream))
            return ArchiveType.RevergePackage;
        throw new Exception("unknown archive type");
    }
    public static bool ExtractArchive(string file,string output)
    {
        FileStream stream = File.OpenRead(file);
        bool result = ExtractArchive(stream,output);
        stream.Close();
        return result;
    }
    public static bool ExtractArchive(Stream file,string output)
    {
        try
        {
            ArchiveType type = DetectArchive(file);
            if(type != ArchiveType.RevergePackage)
            {
                IArchive? archive = type switch
                {
                    ArchiveType.Zip => ZipArchive.Open(file),
                    ArchiveType.Rar => RarArchive.Open(file),
                    ArchiveType.SevenZip => RarArchive.Open(file),
                    ArchiveType.Tar => TarArchive.Open(file),
                    ArchiveType.GZip => GZipArchive.Open(file),
                    _ => null,
                };
                if(archive == null) return false;
                archive.ExtractToDirectory(output);
                return true;
            }
        }
        catch(Exception ex)
        {
            Velvet.Error(ex);
            return false;
        }
        return true;
    }
    public static string? ExtractArchive(string file)
    {
        string temp = FileSystem.CreateTempFolder();
        return ExtractArchive(file,temp) ? temp : null;
    }
    public static string? ExtractArchive(Stream stream)
    {
        string temp = FileSystem.CreateTempFolder();
        return ExtractArchive(stream,temp) ? temp : null;
    }
}