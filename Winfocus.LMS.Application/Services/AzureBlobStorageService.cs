namespace Winfocus.LMS.Application.Services
{
    using System;
    using System.IO;
    using System.Linq;
    using System.Threading.Tasks;
    using Azure.Storage.Blobs;
    using Azure.Storage.Blobs.Models;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Settings;

    /// <summary>
    /// Implements <see cref="IFileStorageService"/> using Azure Blob Storage.
    /// All files are stored as blobs within a single container, using the
    /// same relative path structure as the local file storage service.
    /// </summary>
    public class AzureBlobStorageService : IFileStorageService
    {
        private readonly BlobContainerClient _containerClient;
        private readonly FileUploadSettings _uploadSettings;
        private readonly ILogger<AzureBlobStorageService> _logger;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="AzureBlobStorageService"/> class.
        /// </summary>
        /// <param name="uploadSettings">
        /// File upload configuration including Azure connection string
        /// and container name.
        /// </param>
        /// <param name="logger">Logger instance.</param>
        /// <exception cref="InvalidOperationException">
        /// Thrown when Azure Blob connection string is not configured.
        /// </exception>
        public AzureBlobStorageService(
            IOptions<FileUploadSettings> uploadSettings,
            ILogger<AzureBlobStorageService> logger)
        {
            _uploadSettings = uploadSettings.Value;
            _logger = logger;

            if (string.IsNullOrWhiteSpace(
                _uploadSettings.AzureBlobConnectionString))
            {
                throw new InvalidOperationException(
                    "Azure Blob Storage connection string is not configured. "
                    + "Set FileUpload:AzureBlobConnectionString in "
                    + "appsettings.json.");
            }

            var blobServiceClient = new BlobServiceClient(
                _uploadSettings.AzureBlobConnectionString);

            _containerClient = blobServiceClient.GetBlobContainerClient(
                _uploadSettings.AzureBlobContainerName);

            // Ensure container exists (runs once on startup)
            _containerClient.CreateIfNotExists(
                PublicAccessType.Blob);

            _logger.LogInformation(
                "AzureBlobStorageService initialized. "
                + "Container: {ContainerName}, "
                + "Account: {AccountName}",
                _uploadSettings.AzureBlobContainerName,
                blobServiceClient.AccountName);
        }

        /// <inheritdoc/>
        public async Task<string> SaveFileAsync(
            IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty.");
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var blobPath = $"StudentFiles/{folderName}/{fileName}"
                .Replace("\\", "/");

            var blobClient = _containerClient.GetBlobClient(blobPath);

            var headers = new BlobHttpHeaders
            {
                ContentType = GetContentType(file.FileName),
            };

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions { HttpHeaders = headers });

            _logger.LogInformation(
                "File saved to blob. OriginalName: {OriginalName}, "
                + "BlobPath: {BlobPath}, Size: {Size} bytes",
                file.FileName, blobPath, file.Length);

            return blobPath;
        }

        /// <inheritdoc/>
        public async Task<string> UploadAsync(
            IFormFile file, string folder)
        {
            ValidateFile(file);

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var blobPath =
                $"{_uploadSettings.UploadBasePath}/{folder}/{uniqueFileName}"
                    .Replace("\\", "/");

            var blobClient = _containerClient.GetBlobClient(blobPath);

            var headers = new BlobHttpHeaders
            {
                ContentType = GetContentType(file.FileName),
            };

            using var stream = file.OpenReadStream();
            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions { HttpHeaders = headers });

            _logger.LogInformation(
                "File uploaded to blob. OriginalName: {OriginalName}, "
                + "StoredAs: {StoredName}, Size: {Size} bytes, "
                + "BlobPath: {BlobPath}",
                file.FileName, uniqueFileName, file.Length, blobPath);

            return blobPath;
        }

        /// <inheritdoc/>
        public async Task<string> SaveFileBase64Async(
            string base64File,
            string folderName,
            string fileNamePrefix)
        {
            if (string.IsNullOrWhiteSpace(base64File))
            {
                throw new ArgumentException("File content is empty.");
            }

            // Remove base64 metadata if exists
            // (e.g., "data:image/png;base64,")
            var parts = base64File.Split(',');
            var base64Data = parts.Length > 1 ? parts[1] : parts[0];

            // Detect extension from data URI if available
            var detectedExtension = DetectExtensionFromDataUri(
                base64File);

            byte[] fileBytes = Convert.FromBase64String(base64Data);

            var fileName =
                $"{fileNamePrefix}_{Guid.NewGuid()}{detectedExtension}";
            var blobPath = $"StudentFiles/{folderName}/{fileName}"
                .Replace("\\", "/");

            var blobClient = _containerClient.GetBlobClient(blobPath);

            var headers = new BlobHttpHeaders
            {
                ContentType = GetContentType(fileName),
            };

            using var stream = new MemoryStream(fileBytes);
            await blobClient.UploadAsync(
                stream,
                new BlobUploadOptions { HttpHeaders = headers });

            _logger.LogInformation(
                "Base64 file saved to blob. BlobPath: {BlobPath}, "
                + "Size: {Size} bytes",
                blobPath, fileBytes.Length);

            return blobPath;
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogWarning(
                    "DeleteAsync called with empty file path.");
                return;
            }

            var blobPath = filePath
                .Replace("\\", "/")
                .TrimStart('/');

            var blobClient = _containerClient.GetBlobClient(blobPath);

            var response = await blobClient.DeleteIfExistsAsync(
                DeleteSnapshotsOption.IncludeSnapshots);

            if (response.Value)
            {
                _logger.LogInformation(
                    "Blob deleted: {BlobPath}", blobPath);
            }
            else
            {
                _logger.LogWarning(
                    "Blob not found for deletion: {BlobPath}", blobPath);
            }
        }

        /// <inheritdoc/>
        public string GetFileUrl(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }

            var blobPath = filePath
                .Replace("\\", "/")
                .TrimStart('/');

            var blobClient = _containerClient.GetBlobClient(blobPath);

            return blobClient.Uri.ToString();
        }

        /// <summary>
        /// Validates file against configured size limits
        /// and allowed extensions.
        /// </summary>
        /// <param name="file">The file to validate.</param>
        private void ValidateFile(IFormFile file)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException(
                    "File is empty or not provided.");
            }

            if (file.Length > _uploadSettings.MaxFileSizeBytes)
            {
                var fileSizeMB = Math.Round(
                    file.Length / (1024.0 * 1024.0), 2);

                _logger.LogWarning(
                    "File rejected — size {ActualMB} MB exceeds "
                    + "limit {MaxMB} MB. FileName: {FileName}",
                    fileSizeMB,
                    _uploadSettings.MaxFileSizeMB,
                    file.FileName);

                throw new InvalidOperationException(
                    $"File size ({fileSizeMB} MB) exceeds the maximum "
                    + $"allowed size of "
                    + $"{_uploadSettings.MaxFileSizeMB} MB.");
            }

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();

            if (string.IsNullOrEmpty(extension))
            {
                throw new InvalidOperationException(
                    "File must have an extension.");
            }

            var allowedExtensions = _uploadSettings.AllowedExtensions
                .Select(e => e.ToLowerInvariant())
                .ToArray();

            if (!allowedExtensions.Contains(extension))
            {
                _logger.LogWarning(
                    "File rejected — extension '{Extension}' not "
                    + "allowed. FileName: {FileName}, "
                    + "Allowed: [{Allowed}]",
                    extension,
                    file.FileName,
                    string.Join(", ", allowedExtensions));

                throw new InvalidOperationException(
                    $"File extension '{extension}' is not allowed. "
                    + $"Allowed extensions: "
                    + $"{string.Join(", ", allowedExtensions)}");
            }
        }

        /// <summary>
        /// Maps common file extensions to MIME content types.
        /// Falls back to "application/octet-stream" for unknown types.
        /// </summary>
        /// <param name="fileName">File name with extension.</param>
        /// <returns>MIME content type string.</returns>
        private static string GetContentType(string fileName)
        {
            var extension = Path.GetExtension(fileName)
                .ToLowerInvariant();

            return extension switch
            {
                ".pdf" => "application/pdf",
                ".jpg" or ".jpeg" => "image/jpeg",
                ".png" => "image/png",
                ".gif" => "image/gif",
                ".bmp" => "image/bmp",
                ".webp" => "image/webp",
                ".doc" => "application/msword",
                ".docx" => "application/vnd.openxmlformats-"
                    + "officedocument.wordprocessingml.document",
                ".xls" => "application/vnd.ms-excel",
                ".xlsx" => "application/vnd.openxmlformats-"
                    + "officedocument.spreadsheetml.sheet",
                ".txt" => "text/plain",
                ".csv" => "text/csv",
                _ => "application/octet-stream",
            };
        }

        /// <summary>
        /// Attempts to detect the file extension from a base64 data URI.
        /// Falls back to ".png" if detection fails.
        /// </summary>
        /// <param name="base64String">
        /// The full base64 string, possibly with data URI prefix.
        /// </param>
        /// <returns>File extension including the dot.</returns>
        private static string DetectExtensionFromDataUri(
            string base64String)
        {
            if (!base64String.StartsWith("data:", StringComparison.OrdinalIgnoreCase))
            {
                return ".png"; // default fallback
            }

            // Extract MIME type from "data:image/png;base64,..."
            var mimeEnd = base64String.IndexOf(';');
            if (mimeEnd <= 5)
            {
                return ".png";
            }

            var mimeType = base64String[5..mimeEnd].ToLowerInvariant();

            return mimeType switch
            {
                "image/png" => ".png",
                "image/jpeg" or "image/jpg" => ".jpg",
                "image/gif" => ".gif",
                "image/bmp" => ".bmp",
                "image/webp" => ".webp",
                "application/pdf" => ".pdf",
                _ => ".png",
            };
        }
    }
}
