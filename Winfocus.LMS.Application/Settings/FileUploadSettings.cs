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
        /// <example>10.</example>
        public int MaxFileSizeMB { get; set; } = 10;

        /// <summary>
        /// Gets or sets the list of allowed file extensions (including dot).
        /// </summary>
        /// <example>[".pdf", ".jpg", ".png", ".doc", ".docx"].</example>
        public string[] AllowedExtensions { get; set; } = new[]
        {
            ".pdf", ".jpg", ".jpeg", ".png", ".doc", ".docx",
        };

        /// <summary>
        /// Gets or sets the base path inside wwwroot for registration uploads.
        /// </summary>
        /// <example>uploads/registrations.</example>
        public string UploadBasePath { get; set; } = "uploads/registrations";

        /// <summary>
        /// Gets the maximum file size bytes.
        /// </summary>
        /// <value>
        /// Calculated maximum file size in bytes.
        /// </value>
        public long MaxFileSizeBytes => MaxFileSizeMB * 1024L * 1024L;
    }
}
