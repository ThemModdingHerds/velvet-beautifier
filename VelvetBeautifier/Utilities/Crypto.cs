using System.Security.Cryptography;

namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods for cryptographical things (no coins or NFTs)
/// </summary>
public static class Crypto
{
    /// <summary>
    /// Create a <c>SHA256</c> hash from the <c>stream</c>
    /// </summary>
    /// <param name="stream">The stream to hash</param>
    /// <returns>a byte array as the hash</returns>
    public static byte[] HashSHA256(Stream stream)
    {
        SHA256 algorithm = SHA256.Create();
        return algorithm.ComputeHash(stream);
    }
    /// <summary>
    /// Hash <c>file</c> to <c>SHA256</c> 
    /// </summary>
    /// <param name="file">A filepath</param>
    /// <returns>a byte array as the hash, can be empty if the file does not exist</returns>
    public static byte[] Hash(string file)
    {
        if(!File.Exists(file)) return [];
        using FileStream stream = File.OpenRead(file);
        byte[] result = HashSHA256(stream);
        return result;
    }
    /// <summary>
    /// Verify if <c>file</c> matches <c>hash</c>
    /// </summary>
    /// <param name="file">A filepath</param>
    /// <param name="hash">A checksum hash</param>
    /// <returns>return true if they match, otherwise false</returns>
    public static bool Checksum(string file,string hash) => Checksum(file) == hash;
    /// <summary>
    /// Get the checksum of <c>file</c>
    /// </summary>
    /// <param name="file">A filepath</param>
    /// <returns>a checksum which is just a SHA256 as a string</returns>
    public static string Checksum(string file)
    {
        return BitConverter.ToString(Hash(file)).Replace("-","").ToLower();
    }
}