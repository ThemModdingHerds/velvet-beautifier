using System.Text.Json;
namespace ThemModdingHerds.VelvetBeautifier.Utilities;
/// <summary>
/// Contains various methods of downloading files
/// </summary>
public static class DownloadManager
{
    /// <summary>
    /// Get the Http Client of this software
    /// </summary>
    public static HttpClient Client {
        get
        {
            HttpClient client = new();
            // User Agent of this Software is required for GitHub
            client.DefaultRequestHeaders.Add("User-Agent",$"{Velvet.NAME}/{Dotnet.LibraryVersion}");
            return client;
        }
    }
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> as a stream, can be null if the fetch requests fails
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>The response data of this fetch if it exists</returns>
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
    /// <summary>
    /// Fetch <c>url</c> as a stream, can be null if the fetch requests fails
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <returns>The response data of this fetch if it exists</returns>
    public static async Task<Stream?> Get(string url) => await Get(url,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and save it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="path">The filepath to save</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>return true if request was successful, otherwise false</returns>
    public static async Task<bool> Get(string url,string path,Dictionary<string,string> headers)
    {
        Stream? data = await Get(url,headers);
        if(data == null) return false;
        FileStream file = File.OpenWrite(path);
        await data.CopyToAsync(file);
        file.Close();
        return true;
    }
    /// <summary>
    /// Fetch <c>url</c> and save it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="path">The filepath to save</param>
    /// <returns>return true if request was successful, otherwise false</returns>
    public static async Task<bool> Get(string url,string path) => await Get(url,path,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and save it to a temporal file
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>The filepath to the temporal file</returns>
    public static async Task<string> GetTemp(string url,Dictionary<string,string> headers)
    {
        string temp = FileSystem.CreateTempFile();
        await Get(url,temp,headers);
        return temp;
    }
    /// <summary>
    /// Fetch <c>url</c> and save it to a temporal file
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <returns>The filepath to the temporal file</returns>
    public static async Task<string> GetTemp(string url) => await GetTemp(url,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and extract it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="path">The filepath to extract to</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>return true if extraction was successful, otherwise false</returns>
    public static async Task<bool> GetAndUnzip(string url,string path,Dictionary<string,string> headers)
    {
        string temp = await GetTemp(url,headers);
        return ArchiveUtils.ExtractArchive(temp,path);
    }
    /// <summary>
    /// Fetch <c>url</c> and extract it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="path">The filepath to extract to</param>
    /// <returns>return true if extraction was successful, otherwise false</returns>
    public static async Task<bool> GetAndUnzip(string url,string path) => await GetAndUnzip(url,path,[]);
    
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and extract it to a temporal folder
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>return the temporal folderpath with the extracted content</returns>
    public static async Task<string> GetAndUnzip(string url,Dictionary<string,string> headers)
    {
        string temp = FileSystem.CreateTempFolder();
        await GetAndUnzip(url,temp,headers);
        return temp;
    }
    
    /// <summary>
    /// Fetch <c>url</c> and extract it to a temporal folder
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <returns>return the temporal folderpath with the extracted content</returns>
    public static async Task<string> GetAndUnzip(string url) => await GetAndUnzip(url,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and deserialize it to a JSON object
    /// </summary>
    /// <typeparam name="T">A type that can be serialized by JSON</typeparam>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>A JSON object</returns>
    public static async Task<T?> GetJSON<T>(string url,Dictionary<string,string> headers)
    {
        Stream? result = await Get(url,headers); 
        if(result == null) return default;
        return JsonSerializer.Deserialize<T>(result);
    }
    /// <summary>
    /// Fetch <c>url</c> and deserialize it to a JSON object
    /// </summary>
    /// <typeparam name="T">A type that can be serialized by JSON</typeparam>
    /// <param name="url">The URL endpoint</param>
    /// <returns>A JSON object</returns>
    public static async Task<T?> GetJSON<T>(string url) => await GetJSON<T>(url,[]);
}