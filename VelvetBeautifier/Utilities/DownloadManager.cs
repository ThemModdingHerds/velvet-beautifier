using System.Formats.Asn1;
using System.Text.Json;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class DownloadManager
{
    public static async Task<Stream> Get(string url)
    {
        HttpClient client = new();
        return await client.GetStreamAsync(url);
    }
    public static async Task Get(string url,string path)
    {
        Stream data = await Get(url);
        FileStream file = File.OpenWrite(path);
        await data.CopyToAsync(file);
        file.Close();
    }
    public static async Task<string> GetTemp(string url)
    {
        string temp = FileSystem.CreateTempFile();
        await Get(url,temp);
        return temp;
    }
    public static async Task GetAndUnzip(string url,string path)
    {
        string temp = await GetTemp(url);
        FileSystem.ExtractZip(temp,path);
    }
    public static async Task<string> GetAndUnzip(string url)
    {
        string temp = FileSystem.CreateTempFolder();
        await GetAndUnzip(url,temp);
        return temp;
    }
    public static async Task<T?> GetJSON<T>(string url)
    {
        Stream result = await Get(url); 
        return JsonSerializer.Deserialize<T>(result);
    }
}