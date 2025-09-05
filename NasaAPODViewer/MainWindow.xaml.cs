using System;
using System.Diagnostics;
using System.IO;
using System.Threading.Tasks;
using System.Windows;
using System.Windows.Media.Imaging;
using Microsoft.Win32;

namespace NasaAPODViewer
{
    public partial class MainWindow : Window
    {
        private readonly ApiService _apiService = new();
        private byte[]? _currentImageBytes;

        public MainWindow()
        {
            InitializeComponent();
        }

        private async void FetchButton_Click(object sender, RoutedEventArgs e)
        {
            await FetchApodAsync();
        }

        private async Task FetchApodAsync()
        {
            var apodData = await _apiService.FetchApodDataAsync();
            if (apodData != null)
            {
                await UpdateUIAsync(apodData);
            }
        }

        private async Task UpdateUIAsync(ApodData apodData)
        {
            TitleLabel.Content = apodData.Title;
            DateLabel.Content = apodData.Date;
            ExplanationText.Text = apodData.Explanation;
            Debug.WriteLine($"APOD media_type={apodData.MediaType} url={apodData.ImageUrl}");

            if (string.Equals(apodData.MediaType, "image", StringComparison.OrdinalIgnoreCase) &&
                !string.IsNullOrWhiteSpace(apodData.ImageUrl))
            {
                _currentImageBytes = await _apiService.DownloadImageAsync(apodData.ImageUrl);
                if (_currentImageBytes != null)
                {
                    DisplayImage(_currentImageBytes);
                }
                else
                {
                    ImageDisplay.Source = null;
                    MessageBox.Show("Failed to download the image.", "Image Error", MessageBoxButton.OK, MessageBoxImage.Warning);
                }
            }
            else
            {
                _currentImageBytes = null;
                ImageDisplay.Source = null;
                MessageBox.Show("Today's APOD is not an image (it may be a video).", "Info", MessageBoxButton.OK, MessageBoxImage.Information);
            }

            _apiService.SaveDataToFile(apodData);
        }

        private void DisplayImage(byte[] imageBytes)
        {
            try
            {
                using var imageStream = new MemoryStream(imageBytes);
                var bitmapImage = new BitmapImage();
                bitmapImage.BeginInit();
                bitmapImage.CacheOption = BitmapCacheOption.OnLoad;
                bitmapImage.StreamSource = imageStream;
                bitmapImage.EndInit();
                bitmapImage.Freeze();
                ImageDisplay.Source = bitmapImage;
            }
            catch (Exception ex)
            {
                MessageBox.Show($"Failed to display image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
            }
        }

        private void SaveButton_Click(object sender, RoutedEventArgs e)
        {
            SaveCurrentImage();
        }

        private void SaveCurrentImage()
        {
            if (_currentImageBytes == null)
            {
                MessageBox.Show("No image to save.", "Warning", MessageBoxButton.OK, MessageBoxImage.Warning);
                return;
            }

            var saveDialog = new SaveFileDialog
            {
                Filter = "JPEG Files (*.jpg)|*.jpg|PNG Files (*.png)|*.png|All files (*.*)|*.*",
                DefaultExt = ".jpg"
            };

            if (saveDialog.ShowDialog() == true)
            {
                try
                {
                    File.WriteAllBytes(saveDialog.FileName, _currentImageBytes);
                    MessageBox.Show($"Image saved as {saveDialog.FileName}", "Success", MessageBoxButton.OK, MessageBoxImage.Information);
                }
                catch (Exception ex)
                {
                    MessageBox.Show($"Failed to save image: {ex.Message}", "Error", MessageBoxButton.OK, MessageBoxImage.Error);
                }
            }
        }
    }
}