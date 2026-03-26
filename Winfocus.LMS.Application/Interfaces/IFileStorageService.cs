namespace Winfocus.LMS.Application.Interfaces
{
    using Microsoft.AspNetCore.Http;

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

        /// <summary>
        /// Uploads a file with validation (size + extension) to the configured
        /// upload path inside wwwroot. Used for registration form file fields.
        /// </summary>
        /// <param name="file">The file to upload.</param>
        /// <param name="folder">Sub-folder within the upload base path.</param>
        /// <returns>Relative path of the saved file (URL-friendly).</returns>
        /// <exception cref="ArgumentException">Thrown when file is null or empty.</exception>
        /// <exception cref="InvalidOperationException">
        /// Thrown when file exceeds size limit or has disallowed extension.
        /// </exception>
        Task<string> UploadAsync(IFormFile file, string folder);

        /// <summary>
        /// Deletes a previously uploaded file from disk.
        /// </summary>
        /// <param name="filePath">The relative file path returned by UploadAsync.</param>
        /// <returns>A task representing the async operation.</returns>
        Task DeleteAsync(string filePath);

        /// <summary>
        /// Gets the publicly accessible URL for an uploaded file.
        /// </summary>
        /// <param name="filePath">The relative file path returned by UploadAsync.</param>
        /// <returns>URL-formatted path suitable for API responses.</returns>
        string GetFileUrl(string filePath);

        /// <summary>
        /// SaveFileBase64Async.
        /// </summary>
        /// <param name="base64File"></param>
        /// <param name="folderName"></param>
        /// <param name="fileNamePrefix"></param>
        /// <returns></returns>
        Task<string> SaveFileBase64Async(string base64File, string folderName, string fileNamePrefix);

        /// <summary>
        /// Extracts and returns the relative blob path from an existing Azure Storage URL.
        /// No upload is performed — the path is parsed and returned directly.
        /// Used when preserving an existing file without re-uploading.
        /// </summary>
        /// <param name="azureUrl">Full Azure blob URL (e.g., https://account.blob.core.windows.net/container/StudentFiles/Photos/xxx.jpg).</param>
        /// <returns>Relative blob path (e.g., StudentFiles/Photos/xxx.jpg).</returns>
        string ExtractBlobPathFromUrl(string azureUrl);
    }
}
