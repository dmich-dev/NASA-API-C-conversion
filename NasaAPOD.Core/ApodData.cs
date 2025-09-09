using Newtonsoft.Json;

namespace NasaApod.Core;

public class ApodData
{
    public string Title { get; set; } = string.Empty;
    public string Explanation { get; set; } = string.Empty;
    public string Date { get; set; } = string.Empty;

    [JsonProperty("url")] 
    public string ImageUrl { get; set; } = string.Empty;

    [JsonProperty("media_type")] 
    public string MediaType { get; set; } = string.Empty;

    public string FetchDate { get; set; } = string.Empty;
}