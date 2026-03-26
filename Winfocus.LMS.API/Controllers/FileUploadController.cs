namespace Winfocus.LMS.API.Controllers
{
    using Asp.Versioning;
    using Microsoft.AspNetCore.Authorization;
    using Microsoft.AspNetCore.Mvc;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Test controller for verifying file upload functionality.
    /// Remove or disable this controller after testing is complete.
    /// </summary>
    [ApiVersion("1.0")]
    [Route("api/v{version:apiVersion}/file-upload-test")]
    [ApiController]
    [AllowAnonymous]
    public class FileUploadTestController : ControllerBase
    {
        private readonly IFileStorageService _fileStorageService;
        private readonly ILogger<FileUploadTestController> _logger;

        /// <summary>
        /// Initializes a new instance of the
        /// <see cref="FileUploadTestController"/> class.
        /// </summary>
        /// <param name="fileStorageService">File storage service.</param>
        /// <param name="logger">Logger.</param>
        public FileUploadTestController(
            IFileStorageService fileStorageService,
            ILogger<FileUploadTestController> logger)
        {
            _fileStorageService = fileStorageService;
            _logger = logger;
        }

        /// <summary>
        /// Test 1: Upload a file using SaveFileAsync.
        /// Simulates student photo/signature upload.
        /// </summary>
        /// <param name="file">File to upload.</param>
        /// <param name="folderName">
        /// Folder name (e.g., "Photos", "Signatures", "TestFolder").
        /// </param>
        /// <returns>Upload result with file path and URL.</returns>
        [HttpPost("save-file")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> TestSaveFile(
            IFormFile file,
            [FromQuery] string folderName = "TestUploads")
        {
            try
            {
                _logger.LogInformation(
                    "TestSaveFile started. FileName: {FileName}, "
                    + "Folder: {Folder}, Size: {Size} bytes",
                    file?.FileName, folderName, file?.Length);

                var savedPath = await _fileStorageService
                    .SaveFileAsync(file!, folderName);

                var fileUrl = _fileStorageService
                    .GetFileUrl(savedPath);

                _logger.LogInformation(
                    "TestSaveFile completed. SavedPath: {Path}, "
                    + "FileUrl: {Url}",
                    savedPath, fileUrl);

                return Ok(new
                {
                    success = true,
                    message = "File uploaded successfully via SaveFileAsync",
                    data = new
                    {
                        savedPath,
                        fileUrl,
                        fileName = file!.FileName,
                        fileSize = file.Length,
                        contentType = file.ContentType,
                        folder = folderName,
                        serviceType = _fileStorageService
                            .GetType().Name,
                    },
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TestSaveFile failed.");
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name,
                });
            }
        }

        /// <summary>
        /// Test 2: Upload a file using UploadAsync.
        /// Simulates staff registration file upload with validation.
        /// </summary>
        /// <param name="file">File to upload.</param>
        /// <param name="folder">
        /// Sub-folder (e.g., "test-registration-123").
        /// </param>
        /// <returns>Upload result with file path and URL.</returns>
        [HttpPost("upload")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> TestUpload(
            IFormFile file,
            [FromQuery] string folder = "test-folder")
        {
            try
            {
                _logger.LogInformation(
                    "TestUpload started. FileName: {FileName}, "
                    + "Folder: {Folder}, Size: {Size} bytes",
                    file?.FileName, folder, file?.Length);

                var savedPath = await _fileStorageService
                    .UploadAsync(file!, folder);

                var fileUrl = _fileStorageService
                    .GetFileUrl(savedPath);

                _logger.LogInformation(
                    "TestUpload completed. SavedPath: {Path}, "
                    + "FileUrl: {Url}",
                    savedPath, fileUrl);

                return Ok(new
                {
                    success = true,
                    message = "File uploaded successfully "
                        + "via UploadAsync (with validation)",
                    data = new
                    {
                        savedPath,
                        fileUrl,
                        fileName = file!.FileName,
                        fileSize = file.Length,
                        contentType = file.ContentType,
                        folder,
                        serviceType = _fileStorageService
                            .GetType().Name,
                    },
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    errorType = "ValidationError",
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    errorType = "FileRejected",
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TestUpload failed.");
                return StatusCode(500, new
                {
                    success = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name,
                });
            }
        }

        /// <summary>
        /// Test 3: Upload a base64-encoded file using SaveFileBase64Async.
        /// </summary>
        /// <param name="request">Base64 file content and metadata.</param>
        /// <returns>Upload result with file path and URL.</returns>
        [HttpPost("save-base64")]
        public async Task<IActionResult> TestSaveBase64(
            [FromBody] Base64UploadTestRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "TestSaveBase64 started. Folder: {Folder}, "
                    + "Prefix: {Prefix}, Base64Length: {Length}",
                    request.FolderName,
                    request.FileNamePrefix,
                    request.Base64File?.Length);

                var savedPath = await _fileStorageService
                    .SaveFileBase64Async(
                        request.Base64File!,
                        request.FolderName,
                        request.FileNamePrefix);

                var fileUrl = _fileStorageService
                    .GetFileUrl(savedPath);

                _logger.LogInformation(
                    "TestSaveBase64 completed. SavedPath: {Path}, "
                    + "FileUrl: {Url}",
                    savedPath, fileUrl);

                return Ok(new
                {
                    success = true,
                    message = "Base64 file uploaded successfully",
                    data = new
                    {
                        savedPath,
                        fileUrl,
                        folder = request.FolderName,
                        prefix = request.FileNamePrefix,
                        serviceType = _fileStorageService
                            .GetType().Name,
                    },
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TestSaveBase64 failed.");
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name,
                });
            }
        }

        /// <summary>
        /// Test 4: Delete a previously uploaded file.
        /// </summary>
        /// <param name="filePath">
        /// The relative path returned by upload endpoints
        /// (e.g., "StudentFiles/TestUploads/guid_file.jpg").
        /// </param>
        /// <returns>Deletion result.</returns>
        [HttpDelete("delete")]
        public async Task<IActionResult> TestDelete(
            [FromQuery] string filePath)
        {
            try
            {
                _logger.LogInformation(
                    "TestDelete started. FilePath: {FilePath}",
                    filePath);

                await _fileStorageService.DeleteAsync(filePath);

                _logger.LogInformation(
                    "TestDelete completed. FilePath: {FilePath}",
                    filePath);

                return Ok(new
                {
                    success = true,
                    message = "File deleted successfully",
                    deletedPath = filePath,
                    serviceType = _fileStorageService
                        .GetType().Name,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "TestDelete failed.");
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                    exceptionType = ex.GetType().Name,
                });
            }
        }

        /// <summary>
        /// Test 5: Get the public URL for a file path.
        /// </summary>
        /// <param name="filePath">
        /// Relative file path
        /// (e.g., "StudentFiles/Photos/guid_file.jpg").
        /// </param>
        /// <returns>The resolved URL.</returns>
        [HttpGet("get-url")]
        public IActionResult TestGetUrl(
            [FromQuery] string filePath)
        {
            try
            {
                var fileUrl = _fileStorageService
                    .GetFileUrl(filePath);

                return Ok(new
                {
                    success = true,
                    inputPath = filePath,
                    resolvedUrl = fileUrl,
                    serviceType = _fileStorageService
                        .GetType().Name,
                });
            }
            catch (Exception ex)
            {
                return BadRequest(new
                {
                    success = false,
                    message = ex.Message,
                });
            }
        }

        /// <summary>
        /// Test 6: Health check — shows which storage provider is active.
        /// </summary>
        /// <returns>Current storage configuration.</returns>
        [HttpGet("health")]
        public IActionResult HealthCheck()
        {
            return Ok(new
            {
                success = true,
                message = "File upload service is running",
                storageProvider = _fileStorageService
                    .GetType().Name,
                timestamp = DateTime.UtcNow,
            });
        }

        /// <summary>
        /// Test 7: Full round-trip test.
        /// Uploads a file → Gets URL → Deletes file.
        /// </summary>
        /// <param name="file">File to test with.</param>
        /// <returns>Results of each step.</returns>
        [HttpPost("full-test")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> FullRoundTripTest(
            IFormFile file)
        {
            var results = new List<object>();

            try
            {
                // Step 1: Upload
                _logger.LogInformation(
                    "FullTest Step 1: Uploading file {FileName}",
                    file?.FileName);

                var savedPath = await _fileStorageService
                    .SaveFileAsync(file!, "FullTest");

                results.Add(new
                {
                    step = 1,
                    action = "SaveFileAsync",
                    status = "SUCCESS",
                    savedPath,
                });

                // Step 2: Get URL
                var fileUrl = _fileStorageService
                    .GetFileUrl(savedPath);

                results.Add(new
                {
                    step = 2,
                    action = "GetFileUrl",
                    status = "SUCCESS",
                    fileUrl,
                });

                // Step 3: Delete
                await _fileStorageService.DeleteAsync(savedPath);

                results.Add(new
                {
                    step = 3,
                    action = "DeleteAsync",
                    status = "SUCCESS",
                    deletedPath = savedPath,
                });

                // Step 4: Verify URL after deletion
                var urlAfterDelete = _fileStorageService
                    .GetFileUrl(savedPath);

                results.Add(new
                {
                    step = 4,
                    action = "GetFileUrl (after delete)",
                    status = "SUCCESS",
                    note = "URL still resolves but file "
                        + "should return 404",
                    fileUrl = urlAfterDelete,
                });

                _logger.LogInformation(
                    "FullTest completed successfully. "
                    + "All 4 steps passed.");

                return Ok(new
                {
                    success = true,
                    message = "Full round-trip test PASSED "
                        + "— Upload → URL → Delete all working",
                    serviceType = _fileStorageService
                        .GetType().Name,
                    steps = results,
                });
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "FullTest failed.");

                results.Add(new
                {
                    step = "FAILED",
                    action = ex.Source,
                    status = "ERROR",
                    message = ex.Message,
                });

                return BadRequest(new
                {
                    success = false,
                    message = $"Full test failed: {ex.Message}",
                    serviceType = _fileStorageService
                        .GetType().Name,
                    stepsCompleted = results,
                });
            }
        }
    }

    /// <summary>
    /// Request model for base64 file upload test.
    /// </summary>
    public class Base64UploadTestRequest
    {
        /// <summary>
        /// Gets or sets the base64 encoded file content.
        /// Can include data URI prefix
        /// (e.g., "data:image/png;base64,...").
        /// </summary>
        public string? Base64File { get; set; }

        /// <summary>
        /// Gets or sets the target folder name.
        /// </summary>
        public string FolderName { get; set; } = "TestBase64";

        /// <summary>
        /// Gets or sets the file name prefix.
        /// </summary>
        public string FileNamePrefix { get; set; } = "test";
    }
}
