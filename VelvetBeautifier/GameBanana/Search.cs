using System.Text.Json.Serialization;
using ThemModdingHerds.VelvetBeautifier.Utilities;

namespace ThemModdingHerds.VelvetBeautifier.GameBanana;

public class Search
{
    public class Metadata
    {
        [JsonPropertyName("_nRecordCount")]
        public int RecordCount { get; set; }
        [JsonPropertyName("_bIsComplete")]
        public bool IsComplete { get; set; }
        [JsonPropertyName("_nPerpage")]
        public int RecordsPerPage { get; set; }
    }
    public class Record
    {
        public class PreviewMedia
        {
            public class Image
            {
                [JsonPropertyName("_sType")]
                public string Type { get; set; } = string.Empty;
                [JsonPropertyName("_sBaseUrl")]
                public string BaseUrl { get; set; } = string.Empty;
                [JsonPropertyName("_sCaption")]
                public string Caption { get; set; } = string.Empty;
                [JsonPropertyName("_sFile")]
                // 220
                public string Filename { get; set; } = string.Empty;
                [JsonPropertyName("_sFile220")]
                public string Filename220 { get; set; } = string.Empty;
                [JsonPropertyName("_hFile220")]
                public int Height220 { get; set; }
                [JsonPropertyName("_wFile220")]
                public int Width220 { get; set; }
                // 530
                [JsonPropertyName("_sFile530")]
                public string Filename530 { get; set; } = string.Empty;
                [JsonPropertyName("_hFile530")]
                public int Height530 { get; set; }
                [JsonPropertyName("_wFile530")]
                public int Width530 { get; set; }
                // 100
                [JsonPropertyName("_sFile100")]
                public string Filename100 { get; set; } = string.Empty;
                [JsonPropertyName("_hFile100")]
                public int Height100 { get; set; }
                [JsonPropertyName("_wFile100")]
                public int Width100 { get; set; }
                // 800
                [JsonPropertyName("_sFile800")]
                public string Filename800 { get; set; } = string.Empty;
                [JsonPropertyName("_hFile800")]
                public int Height800 { get; set; }
                [JsonPropertyName("_wFile800")]
                public int Width800 { get; set; }
            }
            [JsonPropertyName("_aImages")]
            public List<Image> Images { get; set; } = [];
        }
        public class Submitter
        {
            [JsonPropertyName("_idRow")]
            public int Id { get; set; }
            [JsonPropertyName("_sName")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("_bIsOnline")]
            public bool IsOnline { get; set; }
            [JsonPropertyName("_bHasRipe")]
            public bool HasRipe { get; set; }
            [JsonPropertyName("_sProfileUrl")]
            public string Url { get; set; } = string.Empty;
            [JsonPropertyName("_sAvatarUrl")]
            public string AvatarUrl { get; set; } = string.Empty;
        }
        public class Game
        {
            [JsonPropertyName("_idRow")]
            public int Id { get; set; }
            [JsonPropertyName("_sName")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("_sProfileUrl")]
            public string Url { get; set; } = string.Empty;
            [JsonPropertyName("_sIconUrl")]
            public string IconUrl { get; set; } = string.Empty;
        }
        public class Category
        {
            [JsonPropertyName("_sName")]
            public string Name { get; set; } = string.Empty;
            [JsonPropertyName("_sProfileUrl")]
            public string Url { get; set; } = string.Empty;
            [JsonPropertyName("_IconUrl")]
            public string IconUrl { get; set; } = string.Empty;
        }
        [JsonPropertyName("_idRow")]
        public int Id { get; set; }
        [JsonPropertyName("_sModelName")]
        public string ModelName { get; set; } = string.Empty;
        [JsonPropertyName("_sSingularTitle")]
        public string SingularTitle { get; set; } = string.Empty;
        [JsonPropertyName("_sIconClasses")]
        public string IconClasses { get; set; } = string.Empty;
        [JsonPropertyName("_sName")]
        public string Name { get; set; } = string.Empty;
        [JsonPropertyName("_sProfileUrl")]
        public string Url { get; set; } = string.Empty;
        [JsonPropertyName("_tsDateAdded")]
        public int DateAdded { get; set; }
        [JsonPropertyName("_tsDateModified")]
        public int DateModified { get; set; }
        [JsonPropertyName("_bHasFiles")]
        public bool HasFiles { get; set; }
        [JsonPropertyName("_aTags")]
        public List<object> Tags = [];
        [JsonPropertyName("_aPreviewMedia")]
        public PreviewMedia Previews { get; set; } = new();
        [JsonPropertyName("_aSubmitter")]
        public Submitter User { get; set; } = new();
        [JsonPropertyName("_aGame")]
        public Game ForGame { get; set; } = new();
        [JsonPropertyName("_aRootCategory")]
        public Category RootCategory { get; set; } = new();
        [JsonPropertyName("_sVersion")]
        public string Version { get; set; } = string.Empty;
        [JsonPropertyName("_bIsObsolete")]
        public bool IsObsolete { get; set; }
        [JsonPropertyName("_sInitialVisibility")]
        public string InitialVisibility { get; set; } = string.Empty;
        [JsonPropertyName("_bHasContentRatings")]
        public bool HasContentRatings { get; set; }
        [JsonPropertyName("_nLikeCount")]
        public int Likes { get; set; }
        [JsonPropertyName("_nPostCount")]
        public int Posts { get; set; }
        [JsonPropertyName("_bWasFeatured")]
        public bool WasFeatured { get; set; }
        [JsonPropertyName("_nViewCount")]
        public int Views { get; set; }
        [JsonPropertyName("_bIsOwnedByAccessor")]
        public bool IsOwnedByAccessor { get; set; }
    }
    [JsonPropertyName("_aMetadata")]
    public Metadata Details { get; set; } = new();
    [JsonPropertyName("_aRecords")]
    public List<Record> Records { get; set; } = [];
    public static List<Record> FetchMods()
    {
        List<Record> records = [];
        int page = 1;
        while (true)
        {
            Search? result = DownloadManager.GetJSON<Search>(Utils.CreateSearchRequestUrl(page));
            if (result == null)
                break;
            records.AddRange(result.Records);
            if (result.Details.IsComplete)
                break;
        }
        return records;
    }
}