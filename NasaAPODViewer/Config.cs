using System;
using System.IO;

namespace NasaAPODViewer
{
    public static class Config
    {
        public const string ApiKey = "T8RPfyvEf6Zn8riLQ0efTZUQreaMpEEKpQr4utyb";
        public static string ApiUrl => $"https://api.nasa.gov/planetary/apod?api_key={ApiKey}";

        // Application name for per-user storage
        public static string AppName => "NasaAPODViewer";

        // Per-user data directory under %LOCALAPPDATA%/NasaAPODViewer
        public static string DataDir => Path.Combine(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData), AppName);

        // JSON log file storing fetched APOD entries
        public static string DataFile => Path.Combine(DataDir, "apod_log.json");
    }
}