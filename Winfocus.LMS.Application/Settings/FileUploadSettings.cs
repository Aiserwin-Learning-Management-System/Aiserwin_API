namespace Winfocus.LMS.Application.Settings
{
    /// <summary>
    /// Configuration settings for file upload validation and storage paths.
    /// Bound from appsettings.json section "FileUpload".
    /// </summary>
    public class FileUploadSettings
    {
        /// <summary>
        /// Configuration section name in appsettings.json.
        /// </summary>
        public const string SectionName = "FileUpload";

        /// <summary>
        /// Gets or sets the maximum allowed file size in megabytes.
        /// </summary>
        public int MaxFileSizeMB { get; set; } = 10;

        /// <summary>
        /// Gets or sets the list of allowed file extensions (including dot).
        /// </summary>
        public string[] AllowedExtensions { get; set; } = new[]
        {
            ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx",
        };

        /// <summary>
        /// Gets or sets the base path inside wwwroot for registration uploads.
        /// </summary>
        public string UploadBasePath { get; set; } = "uploads/registrations";

        /// <summary>
        /// Gets or sets the storage root path for local file storage.
        /// Empty = use ContentRootPath (local dev).
        /// Set to "D:\home\data" or "/home/data" on Azure App Service.
        /// </summary>
        public string StorageRootPath { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the storage provider type.
        /// Valid values: "Local", "AzureBlob".
        /// Defaults to "Local" for backward compatibility.
        /// </summary>
        public string StorageProvider { get; set; } = "Local";

        /// <summary>
        /// Gets or sets the Azure Blob Storage connection string.
        /// Required when <see cref="StorageProvider"/> is "AzureBlob".
        /// </summary>
        public string AzureBlobConnectionString { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the Azure Blob Storage container name.
        /// Required when <see cref="StorageProvider"/> is "AzureBlob".
        /// </summary>
        public string AzureBlobContainerName { get; set; } = "lms-files";

        /// <summary>
        /// Gets the maximum file size in bytes (calculated).
        /// </summary>
        public long MaxFileSizeBytes => MaxFileSizeMB * 1024L * 1024L;

        /// <summary>
        /// Gets a value indicating whether Azure Blob Storage is configured.
        /// </summary>
        public bool UseAzureBlob =>
            StorageProvider.Equals("AzureBlob", StringComparison.OrdinalIgnoreCase);
    }
}
