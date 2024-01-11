using System.Text.Json;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class DownloadManager
{
    public static async Task<byte[]> Get(string url)
    {
        HttpClient client = new();
        return await client.GetByteArrayAsync(url);
    }
    public static async Task Get(string url,string path)
    {
        byte[] data = await Get(url);
        await File.WriteAllBytesAsync(path,data);
    }
    public static async Task GetAndUnzip(string url,string path)
    {
        string temp = IO.CreateTempFile();
        await Get(url,temp);
        IO.ExtractZip(temp,path);
    }
    public static async Task<string> GetAndUnzip(string url)
    {
        string temp = Directory.CreateTempSubdirectory().FullName;
        await GetAndUnzip(url,temp);
        return temp;
    }
    public static async Task<T?> GetJSON<T>(string url)
    {
        HttpClient client = new();
        string result = await client.GetStringAsync(url);
        return JsonSerializer.Deserialize<T>(result);
    }
}