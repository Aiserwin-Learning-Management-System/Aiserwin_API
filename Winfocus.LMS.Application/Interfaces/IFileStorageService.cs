using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Provides functionality for storing files in the system.
    /// </summary>
    public interface IFileStorageService
    {
        /// <summary>
        /// Saves a file to the specified folder and returns the stored file path.
        /// </summary>
        /// <param name="file">File to be saved.</param>
        /// <param name="folderName">Target folder name inside the StudentFiles directory.</param>
        /// <returns>
        /// Relative path of the saved file.
        /// </returns>
        Task<string> SaveFileAsync(IFormFile file, string folderName);
    }
}
