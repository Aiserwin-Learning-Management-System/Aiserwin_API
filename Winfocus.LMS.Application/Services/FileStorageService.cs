namespace Winfocus.LMS.Application.Services
{
    using System;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Microsoft.Extensions.Logging;
    using Microsoft.Extensions.Options;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Application.Settings;

    /// <summary>
    /// Handles physical file storage operations.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;
        private readonly FileUploadSettings _uploadSettings;
        private readonly ILogger<FileStorageService> _logger;
        private readonly string _rootPath;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorageService"/> class.
        /// </summary>
        /// <param name="environment">Hosting environment for path resolution.</param>
        /// <param name="uploadSettings">File upload configuration settings.</param>
        /// <param name="logger">Logger instance.</param>
        public FileStorageService(
              IWebHostEnvironment environment,
              IOptions<FileUploadSettings> uploadSettings,
              ILogger<FileStorageService> logger)
        {
            _environment = environment;
            _uploadSettings = uploadSettings.Value;
            _logger = logger;

            _rootPath = string.IsNullOrEmpty(_uploadSettings.StorageRootPath)
                ? _environment.ContentRootPath
                : _uploadSettings.StorageRootPath;

            _logger.LogInformation(
                "FileStorageService initialized. RootPath: {RootPath}",
                _rootPath);
        }

        /// <inheritdoc/>
        public async Task<string> SaveFileAsync(
            IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty.");
            }

            var uploadsFolder = Path.Combine(
               _rootPath,
               "StudentFiles",
               folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(
                filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine("StudentFiles", folderName, fileName)
                .Replace("\\", "/");
        }

        /// <inheritdoc/>
        public async Task<string> UploadAsync(
            IFormFile file, string folder)
        {
            ValidateFile(file);

            var targetDirectory = Path.Combine(
                _rootPath,
                _uploadSettings.UploadBasePath,
                folder);

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            var extension = Path.GetExtension(file.FileName)
                .ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(targetDirectory, uniqueFileName);

            using (var stream = new FileStream(
                fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            var relativePath = Path.Combine(
                _uploadSettings.UploadBasePath,
                folder,
                uniqueFileName).Replace("\\", "/");

            _logger.LogInformation(
                "File uploaded successfully. OriginalName: {OriginalName}, "
                + "StoredAs: {StoredName}, Size: {Size} bytes, Path: {Path}",
                file.FileName, uniqueFileName, file.Length, relativePath);

            return relativePath;
        }

        /// <summary>
        /// SaveFileBase64Async.
        /// </summary>
        /// <param name="base64File"></param>
        /// <param name="folderName"></param>
        /// <param name="fileNamePrefix"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentException"></exception>
        public async Task<string> SaveFileBase64Async(string base64File, string folderName, string fileNamePrefix)
        {
            if (string.IsNullOrWhiteSpace(base64File))
            {
                throw new ArgumentException("File content is empty.");
            }

            // Remove base64 metadata if exists (data:image/png;base64,)
            var parts = base64File.Split(',');
            var base64Data = parts.Length > 1 ? parts[1] : parts[0];

            byte[] fileBytes = Convert.FromBase64String(base64Data);

            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "StudentFiles",
                folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{fileNamePrefix}_{Guid.NewGuid()}.png";

            var filePath = Path.Combine(uploadsFolder, fileName);

            await File.WriteAllBytesAsync(filePath, fileBytes);

            return Path.Combine("StudentFiles", folderName, fileName)
                .Replace("\\", "/");
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogWarning(
                    "DeleteAsync called with empty file path.");
                return Task.CompletedTask;
            }

            var fullPath = Path.Combine(_rootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation(
                    "File deleted: {FilePath}", filePath);
            }
            else
            {
                _logger.LogWarning(
                    "File not found for deletion: {FilePath}", filePath);
            }

            return Task.CompletedTask;
        }

        /// <inheritdoc/>
        public string GetFileUrl(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                return string.Empty;
            }

            var url = "/" + filePath
                .Replace("\\", "/")
                .TrimStart('/');
            return url;
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
                    + $"allowed size of {_uploadSettings.MaxFileSizeMB} MB.");
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
                    + "allowed. FileName: {FileName}, Allowed: [{Allowed}]",
                    extension,
                    file.FileName,
                    string.Join(", ", allowedExtensions));

                throw new InvalidOperationException(
                    $"File extension '{extension}' is not allowed. "
                    + $"Allowed extensions: "
                    + $"{string.Join(", ", allowedExtensions)}");
            }
        }
    }
}
