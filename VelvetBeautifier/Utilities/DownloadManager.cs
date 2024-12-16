using System.Text.Json;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class DownloadManager
{
    public static async Task<Stream?> Get(string url)
    {
        try
        {
            HttpClient client = new();
            return await client.GetStreamAsync(url);
        }
        catch(Exception e)
        {
            Velvet.Error(e.ToString());
            return null;
        }
    }
    public static async Task<bool> Get(string url,string path)
    {
        Stream? data = await Get(url);
        if(data == null) return false;
        FileStream file = File.OpenWrite(path);
        await data.CopyToAsync(file);
        file.Close();
        return true;
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
        ArchiveUtils.ExtractArchive(temp,path);
    }
    public static async Task<string> GetAndUnzip(string url)
    {
        string temp = FileSystem.CreateTempFolder();
        await GetAndUnzip(url,temp);
        return temp;
    }
    public static async Task<T?> GetJSON<T>(string url)
    {
        Stream? result = await Get(url); 
        if(result == null) return default;
        return JsonSerializer.Deserialize<T>(result);
    }
}