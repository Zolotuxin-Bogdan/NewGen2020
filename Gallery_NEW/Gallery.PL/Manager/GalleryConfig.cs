using System;
using System.Configuration;

namespace Gallery.PL.Manager
{
    public static class GalleryConfig
    {
        public static string key_pathForSave { get; } = "PathForSave";
        public static string key_imageTypes { get; } = "SupportedPhotoFormat";

        private const string default_pathForSave = "/Images/Upload/";
        private const string default_imageTypes = "image/jpeg; image/png";

        public static string GetPathForSave()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var path = default_pathForSave;
            if (!string.IsNullOrEmpty(appSettings[key_pathForSave]))
            {
                 path = appSettings[key_pathForSave];
            }
            return path;
        }

        public static string GetImageTypes()
        {
            var appSettings = ConfigurationManager.AppSettings;
            var imageTypes = default_imageTypes;
            if (!string.IsNullOrEmpty(appSettings[key_imageTypes]))
            {
                imageTypes = appSettings[key_pathForSave];
            }
            return imageTypes;
        }

        public static string GetDbConnectionString()
        {
            var connectionString = ConfigurationManager.ConnectionStrings["sql"] ?? throw new ArgumentException("SQL");
            return connectionString.ConnectionString;
        }
    }
}