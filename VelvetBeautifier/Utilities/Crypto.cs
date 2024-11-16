using System.Security.Cryptography;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Crypto
{
    public static byte[] HashSHA256(Stream stream)
    {
        SHA256 algorithm = SHA256.Create();
        return algorithm.ComputeHash(stream);
    }
    public static byte[] Hash(string file)
    {
        if(!File.Exists(file)) return [];
        FileStream stream = File.OpenRead(file);
        byte[] result = HashSHA256(stream);
        stream.Close();
        return result;
    }
    public static bool Checksum(string file,string hash) => Checksum(file) == hash;
    public static string Checksum(string file)
    {
        return BitConverter.ToString(Hash(file)).Replace("-","").ToLower();
    }
}