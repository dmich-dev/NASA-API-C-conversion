using System;
using System.IO;
using System.Threading.Tasks;
using Avalonia.Media.Imaging;
using CommunityToolkit.Mvvm.ComponentModel;
using CommunityToolkit.Mvvm.Input;
using NasaApod.Core;

namespace NasaApod.Avalonia.ViewModels;

public partial class ApodViewModel : ObservableObject
{
    private readonly ApiService _api = new();

    [ObservableProperty] private string? title;
    [ObservableProperty] private string? date;
    [ObservableProperty] private string? explanation;
    [ObservableProperty] private Bitmap? image;
    [ObservableProperty] private bool isBusy;
    [ObservableProperty] private string? status;

    private byte[]? _imageBytes;
    private ApodData? _data;

    [RelayCommand]
    public async Task FetchAsync()
    {
        if (IsBusy) return;
        IsBusy = true;
        Status = "Fetching...";
        try
        {
            _data = await _api.FetchApodDataAsync();
            if (_data == null)
            {
                Status = "Fetch failed.";
                return;
            }
            Title = _data.Title;
            Date = _data.Date;
            Explanation = _data.Explanation;
            if (_data.MediaType?.Equals("image", StringComparison.OrdinalIgnoreCase) == true && !string.IsNullOrWhiteSpace(_data.ImageUrl))
            {
                _imageBytes = await _api.DownloadImageAsync(_data.ImageUrl);
                if (_imageBytes != null)
                {
                    using var ms = new MemoryStream(_imageBytes);
                    Image = new Bitmap(ms);
                    Status = "Image loaded.";
                }
                else
                {
                    Image = null;
                    Status = "Image download failed.";
                }
            }
            else
            {
                Image = null;
                _imageBytes = null;
                Status = "Not an image.";
            }
            _api.SaveDataToFile(_data);
        }
        finally
        {
            IsBusy = false;
        }
    }

    [RelayCommand]
    public async Task SaveImageAsync()
    {
        if (_imageBytes == null)
        {
            Status = "No image.";
            return;
        }
        var fileName = $"APOD-{Date ?? "unknown"}.jpg";
        var path = Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.MyPictures), fileName);
        await File.WriteAllBytesAsync(path, _imageBytes);
        Status = $"Saved: {path}";
    }
}
