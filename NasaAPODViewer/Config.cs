using System;
using System.IO;

namespace NasaAPODViewer
{
    public static class Config
    {
        public const string ApiKey = "T8RPfyvEf6Zn8riLQ0efTZUQreaMpEEKpQr4utyb";
        public static string ApiUrl => $"https://api.nasa.gov/planetary/apod?api_key={ApiKey}";
        public static string BaseDir => AppDomain.CurrentDomain.BaseDirectory;
        public static string DataFile => Path.Combine(BaseDir, "data", "finalproject_data_collection.json");
    }
}