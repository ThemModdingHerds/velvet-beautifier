using SharpCompress.Archives;
using SharpCompress.Archives.GZip;
using SharpCompress.Archives.Rar;
using SharpCompress.Archives.SevenZip;
using SharpCompress.Archives.Tar;
using SharpCompress.Archives.Zip;
using ThemModdingHerds.GFS;
using ThemModdingHerds.IO.Binary;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains methods for archive files
/// </summary>
public static class ArchiveUtils
{
    /// <summary>
    /// is <c>stream</c> a Reverge Package?
    /// </summary>
    /// <param name="stream">A stream</param>
    /// <returns>true if the stream is a Reverge Package, otherwise false</returns>
    private static bool IsRevergePackage(Stream stream)
    {
        try
        {
            using Reader reader = new(stream);
            // we only need the header
            RevergePackageHeader header = reader.ReadRevergePackageHeader();
            return header.Identifier == RevergePackageHeader.IDENTIFIER;
        }
        catch(Exception)
        {
            return false;
        }
    }
    /// <summary>
    /// Check the archive type of <c>stream</c>
    /// </summary>
    /// <param name="stream">A stream</param>
    /// <returns>The archive type</returns>
    /// <exception cref="Exception">if no archive was detected</exception>
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
    /// <summary>
    /// Extract the archive at <c>input_filepath</c> to <c>output_folderpath</c>
    /// </summary>
    /// <param name="input_filepath">A valid filepath</param>
    /// <param name="output_folderpath">A valid folderpath</param>
    /// <returns>true if extraction was success, otherwise false</returns>
    public static bool ExtractArchive(string input_filepath,string output_folderpath)
    {
        using FileStream stream = File.OpenRead(input_filepath);
        return ExtractArchive(stream,output_folderpath);
    }
    /// <summary>
    /// Extract <c>stream</c> to <c>output_folderpath</c>
    /// </summary>
    /// <param name="stream">A stream</param>
    /// <param name="output_folderpath">A valid folderpath</param>
    /// <returns>true if extraction was success, otherwise false</returns>
    public static bool ExtractArchive(Stream stream,string output_folderpath)
    {
        try
        {
            // detect type
            ArchiveType type = DetectArchive(stream);
            if(type != ArchiveType.RevergePackage)
            {
                // use SharpCompress for extraction
                IArchive? archive = type switch
                {
                    ArchiveType.Zip => ZipArchive.Open(stream),
                    ArchiveType.Rar => RarArchive.Open(stream),
                    ArchiveType.SevenZip => SevenZipArchive.Open(stream),
                    ArchiveType.Tar => TarArchive.Open(stream),
                    ArchiveType.GZip => GZipArchive.Open(stream),
                    _ => null,
                };
                if(archive == null) return false;
                archive.ExtractToDirectory(output_folderpath);
                return true;
            }
            // extract the reverge package file
            using Reader reader = new(stream);
            RevergePackage gfs = reader.ReadRevergePackage();
            GFS.Utils.Extract(gfs,output_folderpath);
        }
        catch(Exception ex)
        {
            Velvet.Error(ex);
            return false;
        }
        return true;
    }
    /// <summary>
    /// Extract archive at <c>filepath</c> to a temporal folder
    /// </summary>
    /// <param name="filepath">A valid filepath</param>
    /// <returns>true if extraction was success, otherwise false</returns>
    public static string? ExtractArchive(string filepath)
    {
        string temp = FileSystem.CreateTempFolder();
        return ExtractArchive(filepath,temp) ? temp : null;
    }
    /// <summary>
    /// Extract <c>stream</c> to a temporal folder
    /// </summary>
    /// <param name="stream">A stream</param>
    /// <returns>true if extraction was success, otherwise false</returns>
    public static string? ExtractArchive(Stream stream)
    {
        string temp = FileSystem.CreateTempFolder();
        return ExtractArchive(stream,temp) ? temp : null;
    }
}