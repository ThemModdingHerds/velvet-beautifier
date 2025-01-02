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
    public static Stream? Get(string url,Dictionary<string,string> headers)
    {
        try
        {
            HttpClient client = Client;
            HttpRequestMessage request = new(HttpMethod.Get,url);
            foreach(var pair in headers)
                request.Headers.Add(pair.Key,pair.Value);
            HttpResponseMessage response = client.Send(request);
            response.EnsureSuccessStatusCode();
            Stream stream = response.Content.ReadAsStream();
            if(stream.Length == 0)
                return null;
            return stream;
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
    public static Stream? Get(string url) => Get(url,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and save it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="filepath">The filepath to save</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>return true if request was successful, otherwise false</returns>
    public static bool Get(string url,string filepath,Dictionary<string,string> headers)
    {
        Stream? data = Get(url,headers);
        if(data == null) return false;
        FileStream file = File.OpenWrite(filepath);
        data.CopyToAsync(file);
        file.Close();
        return true;
    }
    /// <summary>
    /// Fetch <c>url</c> and save it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="filepath">The filepath to save</param>
    /// <returns>return true if request was successful, otherwise false</returns>
    public static bool Get(string url,string filepath) => Get(url,filepath,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and save it to a temporal file
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>The filepath to the temporal file</returns>
    public static string GetTemp(string url,Dictionary<string,string> headers)
    {
        string temp = FileSystem.CreateTempFile();
        Get(url,temp,headers);
        return temp;
    }
    /// <summary>
    /// Fetch <c>url</c> and save it to a temporal file
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <returns>The filepath to the temporal file</returns>
    public static string GetTemp(string url) => GetTemp(url,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and extract it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="filepath">The filepath to extract to</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>return true if extraction was successful, otherwise false</returns>
    public static bool GetAndUnzip(string url,string filepath,Dictionary<string,string> headers)
    {
        string temp = GetTemp(url,headers);
        return ArchiveUtils.ExtractArchive(temp,filepath);
    }
    /// <summary>
    /// Fetch <c>url</c> and extract it to <c>path</c>
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="filepath">The filepath to extract to</param>
    /// <returns>return true if extraction was successful, otherwise false</returns>
    public static bool GetAndUnzip(string url,string filepath) => GetAndUnzip(url,filepath,[]);
    
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and extract it to a temporal folder
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>return the temporal folderpath with the extracted content</returns>
    public static string GetAndUnzip(string url,Dictionary<string,string> headers)
    {
        string temp = FileSystem.CreateTempFolder();
        GetAndUnzip(url,temp,headers);
        return temp;
    }
    
    /// <summary>
    /// Fetch <c>url</c> and extract it to a temporal folder
    /// </summary>
    /// <param name="url">The URL endpoint</param>
    /// <returns>return the temporal folderpath with the extracted content</returns>
    public static string GetAndUnzip(string url) => GetAndUnzip(url,[]);
    /// <summary>
    /// Fetch <c>url</c> with <c>headers</c> and deserialize it to a JSON object
    /// </summary>
    /// <typeparam name="T">A type that can be serialized by JSON</typeparam>
    /// <param name="url">The URL endpoint</param>
    /// <param name="headers">A dictionary of headers</param>
    /// <returns>A JSON object</returns>
    public static T? GetJSON<T>(string url,Dictionary<string,string> headers)
    {
        Stream? result = Get(url,headers); 
        if(result == null) return default;
        return JsonSerializer.Deserialize<T>(result);
    }
    /// <summary>
    /// Fetch <c>url</c> and deserialize it to a JSON object
    /// </summary>
    /// <typeparam name="T">A type that can be serialized by JSON</typeparam>
    /// <param name="url">The URL endpoint</param>
    /// <returns>A JSON object</returns>
    public static T? GetJSON<T>(string url) => GetJSON<T>(url,[]);
}