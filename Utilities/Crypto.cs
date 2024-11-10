using System.Security.Cryptography;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class Crypto
{
    public static byte[] Hash(HashAlgorithm algorithm,byte[] input)
    {
        return algorithm.ComputeHash(input);
    }
    public static byte[] Hash(HashAlgorithm algorithm,Stream stream)
    {
        return algorithm.ComputeHash(stream);
    }
    public static byte[] HashMD5(byte[] input)
    {
        return Hash(MD5.Create(),input);
    }
    public static byte[] HashMD5(Stream stream)
    {
        return Hash(MD5.Create(),stream);
    }
    public static byte[] HashSHA1(byte[] input)
    {
        return Hash(SHA1.Create(),input);
    }
    public static byte[] HashSHA1(Stream stream)
    {
        return Hash(SHA1.Create(),stream);
    }
    public static byte[] HashSHA256(byte[] input)
    {
        return Hash(SHA256.Create(),input);
    }
    public static byte[] HashSHA256(Stream stream)
    {
        return Hash(SHA256.Create(),stream);
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