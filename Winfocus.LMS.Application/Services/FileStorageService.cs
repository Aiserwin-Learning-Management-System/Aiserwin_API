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
        }

        /// <inheritdoc/>
        public async Task<string> SaveFileAsync(IFormFile file, string folderName)
        {
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty.");
            }

            var uploadsFolder = Path.Combine(
                _environment.ContentRootPath,
                "StudentFiles",
                folderName);

            if (!Directory.Exists(uploadsFolder))
            {
                Directory.CreateDirectory(uploadsFolder);
            }

            var fileName = $"{Guid.NewGuid()}_{file.FileName}";
            var filePath = Path.Combine(uploadsFolder, fileName);

            using var stream = new FileStream(filePath, FileMode.Create);
            await file.CopyToAsync(stream);

            return Path.Combine("StudentFiles", folderName, fileName)
                .Replace("\\", "/");
        }

        /// <inheritdoc/>
        public async Task<string> UploadAsync(IFormFile file, string folder)
        {
            // ── Validate file ────────────────────────────────────
            ValidateFile(file);

            // ── Build target directory path ──────────────────────
            // Saves to: wwwroot/uploads/registrations/{folder}/
            var targetDirectory = Path.Combine(
                _environment.WebRootPath,
                _uploadSettings.UploadBasePath,
                folder);

            if (!Directory.Exists(targetDirectory))
            {
                Directory.CreateDirectory(targetDirectory);
            }

            // ── Generate unique file name ────────────────────────
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();
            var uniqueFileName = $"{Guid.NewGuid()}{extension}";
            var fullPath = Path.Combine(targetDirectory, uniqueFileName);

            // ── Save file to disk ────────────────────────────────
            using (var stream = new FileStream(fullPath, FileMode.Create))
            {
                await file.CopyToAsync(stream);
            }

            // ── Build relative path for storage in DB ────────────
            var relativePath = Path.Combine(
                _uploadSettings.UploadBasePath,
                folder,
                uniqueFileName).Replace("\\", "/");

            _logger.LogInformation(
                "File uploaded successfully. OriginalName: {OriginalName}, " +
                "StoredAs: {StoredName}, Size: {Size} bytes, Path: {Path}",
                file.FileName, uniqueFileName, file.Length, relativePath);

            return relativePath;
        }

        /// <inheritdoc/>
        public Task DeleteAsync(string filePath)
        {
            if (string.IsNullOrWhiteSpace(filePath))
            {
                _logger.LogWarning("DeleteAsync called with empty file path.");
                return Task.CompletedTask;
            }

            var fullPath = Path.Combine(_environment.WebRootPath, filePath);

            if (File.Exists(fullPath))
            {
                File.Delete(fullPath);
                _logger.LogInformation("File deleted: {FilePath}", filePath);
            }
            else
            {
                _logger.LogWarning("File not found for deletion: {FilePath}", filePath);
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

            // Ensure forward slashes and leading slash for URL
            var url = "/" + filePath.Replace("\\", "/").TrimStart('/');
            return url;
        }

        /// <summary>
        /// Validates file against configured size limits and allowed extensions.
        /// </summary>
        /// <param name="file">The file to validate.</param>
        /// <exception cref="ArgumentException">File is null or empty.</exception>
        /// <exception cref="InvalidOperationException">
        /// File exceeds size limit or has disallowed extension.
        /// </exception>
        private void ValidateFile(IFormFile file)
        {
            // ── Null / empty check ───────────────────────────────
            if (file == null || file.Length == 0)
            {
                throw new ArgumentException("File is empty or not provided.");
            }

            // ── Size check ───────────────────────────────────────
            if (file.Length > _uploadSettings.MaxFileSizeBytes)
            {
                var fileSizeMB = Math.Round(file.Length / (1024.0 * 1024.0), 2);

                _logger.LogWarning(
                    "File rejected — size {ActualMB} MB exceeds limit {MaxMB} MB. " +
                    "FileName: {FileName}",
                    fileSizeMB, _uploadSettings.MaxFileSizeMB, file.FileName);

                throw new InvalidOperationException(
                    $"File size ({fileSizeMB} MB) exceeds the maximum " +
                    $"allowed size of {_uploadSettings.MaxFileSizeMB} MB.");
            }

            // ── Extension check ──────────────────────────────────
            var extension = Path.GetExtension(file.FileName).ToLowerInvariant();

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
                    "File rejected — extension '{Extension}' not allowed. " +
                    "FileName: {FileName}, Allowed: [{Allowed}]",
                    extension, file.FileName,
                    string.Join(", ", allowedExtensions));

                throw new InvalidOperationException(
                    $"File extension '{extension}' is not allowed. " +
                    $"Allowed extensions: {string.Join(", ", allowedExtensions)}");
            }
        }
    }
}
