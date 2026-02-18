namespace Winfocus.LMS.Application.Services
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Microsoft.AspNetCore.Hosting;
    using Microsoft.AspNetCore.Http;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Handles physical file storage operations.
    /// </summary>
    public class FileStorageService : IFileStorageService
    {
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="FileStorageService"/> class.
        /// </summary>
        /// <param name="environment">Hosting environment used to determine application root path.</param>
        public FileStorageService(IWebHostEnvironment environment)
        {
            _environment = environment;
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
    }
}
