using System.Text;

namespace ThemModdingHerds.VelvetBeautifier.GameNews;
public class News(int index,string subheader,string image,string title,string text,string url)
{
    public const string GAMENEWS_FILEPATH = "Scripts/GameNews.ini";
    public const string GAMENEWS_IMAGE_FOLDER = "Scripts/news_images/";
    public int Index {get;set;} = index;
    public string SubHeader {get;set;} = subheader;
    public string Image {get;set;} = image;
    public string Title {get;set;} = title;
    public string Text {get;set;} = text;
    public string Url {get;set;} = url;
    public string ToString(int index)
    {
        return string.Join('\n',[
            $"[news_{index}]",
            $"subheader = {SubHeader}",
            $"image = {Image}",
            $"title = {Title}",
            $"text = {Text}",
            $"url = {Url}",
            ""
        ]);
    }
    public override string ToString() => ToString(Index);
    public static List<News> ReadGameNews(string file)
    {
        string[] lines = File.ReadAllLines(file).Where((line) => line != string.Empty).ToArray();
        List<News> news = [];
        for(int i = 0;i < lines.Length;i++)
        {
            string line = lines[i].Trim();
            if(line.StartsWith("[news_") && line.EndsWith(']'))
            {
                int index = int.Parse(line["[news_".Length..(line.Length-1)]);
                line = lines[++i].Trim();
                if(!line.StartsWith("subheader = "))
                    throw new Exception("no subheader");
                string subheader = line["subheader = ".Length..];
                line = lines[++i].Trim();
                if(!line.StartsWith("image = "))
                    throw new Exception("no image");
                string image = line["image = ".Length..];
                line = lines[++i].Trim();
                if(!line.StartsWith("title = "))
                    throw new Exception("no title");
                string title = line["title = ".Length..];
                line = lines[++i].Trim();
                if(!line.StartsWith("text = "))
                    throw new Exception("no text");
                string text = line["text = ".Length..];
                line = lines[++i].Trim();
                if(!line.StartsWith("url = "))
                    throw new Exception("no url");
                string url = line["url = ".Length..];
                news.Add(new (index,subheader,image,title,text,url));
            }
        }
        return news;
    }
    public static void WriteGameNews(string file,IEnumerable<News> gamenews)
    {
        FileStream stream = File.OpenWrite(file);
        for(int i = 0;i < gamenews.Count();i++)
        {
            News news = gamenews.ElementAt(i);
            stream.Write(Encoding.UTF8.GetBytes(news.ToString(i+1)));
        }
        stream.WriteByte((byte)'\n');
        stream.Close();
    }
}