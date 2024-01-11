using System.Security.Cryptography;
using System.Text.Json;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Crypto
{
    public static byte[] Hash(HashAlgorithm algorithm,byte[] input)
    {
        return algorithm.ComputeHash(input);
    }
    public static byte[] HashMD5(byte[] input)
    {
        return Hash(MD5.Create(),input);
    }
    public static byte[] HashSHA1(byte[] input)
    {
        return Hash(SHA1.Create(),input);
    }
    public static byte[] HashSHA256(byte[] input)
    {
        return Hash(SHA256.Create(),input);
    }
    public static byte[] Hash(string file)
    {
        if(!File.Exists(file)) return [];
        return HashSHA256(File.ReadAllBytes(file));
    }
    public static bool Checksum(string file,string hash)
    {
        return BitConverter.ToString(Hash(file)) == hash;
    }
    public static string Checksum(string file)
    {
        return BitConverter.ToString(Hash(file));
    }
}
public static class CryptoExt
{
    public static readonly string CHECKSUM_NAME = "checksum";
    public static Dictionary<string,string> GenerateChecksums(this Game game)
    {
        Dictionary<string,string> checksums = [];
        foreach(string resource in IO.GetAllFiles(game.GetTFHResourcesFolder()))
            checksums.Add(Path.GetFileName(resource),Crypto.Checksum(resource));
        foreach(string gfsfile in IO.GetAllFiles(game.GetData01Folder()))
            checksums.Add(Path.GetFileName(gfsfile),Crypto.Checksum(gfsfile));
        return checksums;
    }
    public static string GetChecksumsPath(this Game game) => Path.Combine(Environment.CurrentDirectory,string.Format("{0}.checksum",game.Name));
    public static Dictionary<string,string> CreateChecksums(this Game game)
    {
        if(!HasChecksums(game)) 
        {
            Dictionary<string,string> checksums = GenerateChecksums(game);
            File.WriteAllText(GetChecksumsPath(game),JsonSerializer.Serialize(checksums));
            return checksums;
        }
        return ReadChecksums(game);
    }
    public static bool HasChecksums(this Game game) => File.Exists(GetChecksumsPath(game));
    public static Dictionary<string,string> ReadChecksums(this Game game)
    {
        if(!HasChecksums(game))
            return CreateChecksums(game);
        return JsonSerializer.Deserialize<Dictionary<string,string>>(File.ReadAllText(GetChecksumsPath(game)))
        ?? throw new VelvetException("CryptoExt.ReadChecksums","Couldn't parse json");
    }
}