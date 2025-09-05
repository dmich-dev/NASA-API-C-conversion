using System;
using System.Net.Http;
using System.Threading.Tasks;
using Newtonsoft.Json;
using System.Windows;
using System.IO;
using System.Collections.Generic;

namespace NasaAPODViewer
{
    public class ApiService
    {
        private readonly HttpClient _httpClient;

        public ApiService()
        {
            _httpClient = new HttpClient();
        }

        public async Task<ApodData?> FetchApodDataAsync()
        {
            try
            {
                var response = await _httpClient.GetAsync(Config.ApiUrl);
                response.EnsureSuccessStatusCode();
                var jsonString = await response.Content.ReadAsStringAsync();
                var data = JsonConvert.DeserializeObject<ApodData>(jsonString);
                return data;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to fetch APOD data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public async Task<byte[]?> DownloadImageAsync(string imageUrl)
        {
            try
            {
                var response = await _httpClient.GetAsync(imageUrl);
                response.EnsureSuccessStatusCode();
                return await response.Content.ReadAsByteArrayAsync();
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to download image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                return null;
            }
        }

        public void SaveDataToFile(ApodData data)
        {
            try
            {
                // Ensure directory exists
                Directory.CreateDirectory(Path.GetDirectoryName(Config.DataFile));

                List<ApodData> existingData;

                // Read existing data
                try
                {
                    if (File.Exists(Config.DataFile))
                    {
                        string json = File.ReadAllText(Config.DataFile);
                        existingData = JsonConvert.DeserializeObject<List<ApodData>>(json) ?? new List<ApodData>();
                    }
                    else
                    {
                        existingData = new List<ApodData>();
                    }
                }
                catch (Exception)
                {
                    existingData = new List<ApodData>();
                }

                // Add new data
                existingData.Add(data);

                // Write data back to file
                string updatedJson = JsonConvert.SerializeObject(existingData, Formatting.Indented);
                File.WriteAllText(Config.DataFile, updatedJson);
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to save data: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }
    }
}