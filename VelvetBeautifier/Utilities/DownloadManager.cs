using System.Text.Json;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
public static class DownloadManager
{
    public static HttpClient Client {
        get
        {
            HttpClient client = new();
            client.DefaultRequestHeaders.Add("User-Agent",$"{Velvet.NAME}/{Dotnet.LibraryVersion}");
            return client;
        }
    }
    public static async Task<Stream?> Get(string url,Dictionary<string,string> headers)
    {
        try
        {
            HttpClient client = Client;
            foreach(var pair in headers)
                client.DefaultRequestHeaders.Add(pair.Key,pair.Value);
            return await client.GetStreamAsync(url);
        }
        catch(Exception e)
        {
            Velvet.Error(e.ToString());
            return null;
        }
    }
    public static async Task<Stream?> Get(string url) => await Get(url,[]);
    public static async Task<bool> Get(string url,string path,Dictionary<string,string> headers)
    {
        Stream? data = await Get(url,headers);
        if(data == null) return false;
        FileStream file = File.OpenWrite(path);
        await data.CopyToAsync(file);
        file.Close();
        return true;
    }
    public static async Task<bool> Get(string url,string path) => await Get(url,path,[]);
    public static async Task<string> GetTemp(string url,Dictionary<string,string> headers)
    {
        string temp = FileSystem.CreateTempFile();
        await Get(url,temp,headers);
        return temp;
    }
    public static async Task<string> GetTemp(string url) => await GetTemp(url,[]);
    public static async Task<bool> GetAndUnzip(string url,string path,Dictionary<string,string> headers)
    {
        string temp = await GetTemp(url,headers);
        return ArchiveUtils.ExtractArchive(temp,path);
    }
    public static async Task<bool> GetAndUnzip(string url,string path) => await GetAndUnzip(url,path,[]);
    public static async Task<string> GetAndUnzip(string url,Dictionary<string,string> headers)
    {
        string temp = FileSystem.CreateTempFolder();
        await GetAndUnzip(url,temp,headers);
        return temp;
    }
    public static async Task<string> GetAndUnzip(string url) => await GetAndUnzip(url,[]);
    public static async Task<T?> GetJSON<T>(string url,Dictionary<string,string> headers)
    {
        Stream? result = await Get(url,headers); 
        if(result == null) return default;
        return JsonSerializer.Deserialize<T>(result);
    }
    public static async Task<T?> GetJSON<T>(string url) => await GetJSON<T>(url,[]);
}