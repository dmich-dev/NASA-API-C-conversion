using System.Net.Http;
using Newtonsoft.Json;

namespace NasaApod.Core;

public class ApiService
{
    private readonly HttpClient _httpClient = new();
    private const string ApiKey = "DEMO_KEY"; // TODO: externalize
    private static string ApiUrl => $"https://api.nasa.gov/planetary/apod?api_key={ApiKey}";

    public async Task<ApodData?> FetchApodDataAsync()
    {
        try
        {
            using var response = await _httpClient.GetAsync(ApiUrl, HttpCompletionOption.ResponseHeadersRead);
            response.EnsureSuccessStatusCode();
            var json = await response.Content.ReadAsStringAsync();
            var data = JsonConvert.DeserializeObject<ApodData>(json);
            if (data != null)
            {
                data.FetchDate = DateTime.UtcNow.ToString("o");
            }
            return data;
        }
        catch
        {
            return null;
        }
    }

    public async Task<byte[]?> DownloadImageAsync(string imageUrl)
    {
        try
        {
            return await _httpClient.GetByteArrayAsync(imageUrl);
        }
        catch
        {
            return null;
        }
    }

    public void SaveDataToFile(ApodData data)
    {
        try
        {
            var dir = Path.Combine(AppContext.BaseDirectory, "apod-cache");
            Directory.CreateDirectory(dir);
            var file = Path.Combine(dir, $"{data.Date}.json");
            File.WriteAllText(file, JsonConvert.SerializeObject(data, Formatting.Indented));
        }
        catch { /* swallow in core lib */ }
    }
}