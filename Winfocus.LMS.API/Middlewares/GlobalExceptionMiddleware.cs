namespace Winfocus.LMS.API.Middleware
{
    using System.Text.Json;
    using Winfocus.LMS.Application.Common.Exceptions;

    /// <summary>
    /// GlobalExceptionMiddleware.
    /// </summary>
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;
        private readonly IWebHostEnvironment _environment;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">The next.</param>
        /// <param name="logger">The logger.</param>
        /// <param name="environment">The environment.</param>
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger,
            IWebHostEnvironment environment)
        {
            _next = next;
            _logger = logger;
            _environment = environment;
        }

        /// <summary>
        /// Invokes the asynchronous.
        /// </summary>
        /// <param name="context">The context.</param>
        /// <returns>Task.</returns>
        public async Task InvokeAsync(HttpContext context)
        {
            try
            {
                await _next(context);
            }
            catch (Exception ex)
            {
                await HandleExceptionAsync(context, ex);
            }
        }

        private async Task HandleExceptionAsync(
            HttpContext context,
            Exception exception)
        {
            var correlationId = context.TraceIdentifier;

            _logger.LogError(
                exception,
                "Unhandled exception. CorrelationId: {CorrelationId}, Path: {Path}, User: {User}",
                correlationId,
                context.Request.Path,
                context.User?.Identity?.Name ?? "Anonymous");

            context.Response.ContentType = "application/json";

            int statusCode = StatusCodes.Status500InternalServerError;
            string message = "An unexpected error occurred.";
            string errorCode = "INTERNAL_SERVER_ERROR";
            object? errors = null;

            if (exception is AppException appException)
            {
                statusCode = appException.StatusCode;
                message = appException.Message;
                errorCode = appException.ErrorCode;
                errors = appException.Errors;
            }
            else if (exception is UnauthorizedAccessException)
            {
                statusCode = StatusCodes.Status401Unauthorized;
                message = exception.Message;
                errorCode = "UNAUTHORIZED";
            }
            else if (exception is KeyNotFoundException)
            {
                statusCode = StatusCodes.Status404NotFound;
                message = exception.Message;
                errorCode = "NOT_FOUND";
            }
            else if (exception is ArgumentException)
            {
                statusCode = StatusCodes.Status400BadRequest;
                message = exception.Message;
                errorCode = "BAD_REQUEST";
            }

            context.Response.StatusCode = statusCode;

            var response = new ErrorResponse
            {
                CorrelationId = correlationId,
                StatusCode = statusCode,
                ErrorCode = errorCode,
                Message = statusCode == 500 && !_environment.IsDevelopment()
                    ? "An unexpected error occurred. Please contact support."
                    : message,
                Errors = errors,
                Timestamp = DateTime.UtcNow,
            };

            var options = new JsonSerializerOptions
            {
                PropertyNamingPolicy = JsonNamingPolicy.CamelCase,
            };

            await context.Response.WriteAsync(
                JsonSerializer.Serialize(response, options));
        }
    }

    /// <summary>
    /// ErrorResponse.
    /// </summary>
    public sealed class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the correlation identifier.
        /// </summary>
        /// <value>
        /// The correlation identifier.
        /// </value>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the message.
        /// </summary>
        /// <value>
        /// The message.
        /// </value>
        public string Message { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public object? Errors { get; set; }

        /// <summary>
        /// Gets or sets the timestamp.
        /// </summary>
        /// <value>
        /// The timestamp.
        /// </value>
        public DateTime Timestamp { get; set; }
    }
}
