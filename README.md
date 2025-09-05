# NASA APOD Viewer (C# WPF / .NET 8)

This document describes the C# WPF port of the original Python project. The original Python README is preserved at the repository root (or previous README file) as requested.

## Features
- Fetch current day APOD via NASA API
- Display image, title, date, explanation
- Save image manually
- Append metadata entries to JSON file
- Simple modular structure (ApiService, ApodData, Config, MainWindow)

## Files (C# Port)
```
NasaAPODViewer/
  ApiService.cs
  ApodData.cs
  Config.cs
  MainWindow.xaml
  MainWindow.xaml.cs
  Program.cs
  NasaAPODViewer.csproj
  README_WPF.md
```

## Build & Run
```
dotnet build
 dotnet run --project NasaAPODViewer
```
Click "Fetch Picture" then optionally "Save Picture".

## API Key
Edit `Config.cs`:
```csharp
public const string ApiKey = "YOUR_REAL_KEY";
```

## Data File
Created automatically at:
```
bin/Debug/net8.0-windows/data/finalproject_data_collection.json
```
Contains a JSON array of APOD metadata objects.

## Testing Persistence
See `TESTING.md` for manual and optional headless test guidance.

## Future Enhancements
- Date selector for historical APODs
- Duplicate suppression
- Local image cache
- Dark / light themes
- Unit tests with mocked HttpClient

## License / Attribution
Uses NASA APOD API—respect usage guidelines.
