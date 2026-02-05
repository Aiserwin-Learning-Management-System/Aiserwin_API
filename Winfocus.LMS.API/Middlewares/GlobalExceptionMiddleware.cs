using System.Net;
using System.Text.Json;
using Microsoft.AspNetCore.Http;
using Microsoft.Extensions.Logging;

namespace Winfocus.LMS.API.Middleware
{
    /// <summary>
    /// Middleware to handle all unhandled exceptions globally.
    /// Converts exceptions into standardized API error responses.
    /// </summary>
    public sealed class GlobalExceptionMiddleware
    {
        private readonly RequestDelegate _next;
        private readonly ILogger<GlobalExceptionMiddleware> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GlobalExceptionMiddleware"/> class.
        /// </summary>
        /// <param name="next">Next middleware in the pipeline.</param>
        /// <param name="logger">Logger instance.</param>
        public GlobalExceptionMiddleware(
            RequestDelegate next,
            ILogger<GlobalExceptionMiddleware> logger)
        {
            _next = next;
            _logger = logger;
        }

        /// <summary>
        /// Invokes the middleware.
        /// </summary>
        /// <param name="context">HTTP context.</param>
        /// <returns>A <see cref="Task"/> representing the asynchronous operation.</returns>
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

        /// <summary>
        /// Handles the exception and writes a standardized response.
        /// </summary>
        private async Task HandleExceptionAsync(HttpContext context, Exception exception)
        {
            var correlationId = context.TraceIdentifier;

            _logger.LogError(
                exception,
                "Unhandled exception occurred. CorrelationId: {CorrelationId}, Path: {Path}, User: {User}",
                correlationId,
                context.Request.Path,
                context.User?.Identity?.Name ?? "Anonymous");

            var response = context.Response;
            response.ContentType = "application/json";

            var statusCode = exception switch
            {
                ArgumentException => HttpStatusCode.BadRequest,
                UnauthorizedAccessException => HttpStatusCode.Unauthorized,
                KeyNotFoundException => HttpStatusCode.NotFound,
                _ => HttpStatusCode.InternalServerError
            };

            response.StatusCode = (int)statusCode;

            var errorResponse = new ErrorResponse
            {
                CorrelationId = correlationId,
                StatusCode = response.StatusCode,
                Message = statusCode == HttpStatusCode.InternalServerError
                    ? "An unexpected error occurred. Please contact support."
                    : exception.Message,
            };

            await response.WriteAsync(JsonSerializer.Serialize(errorResponse));
        }
    }

    /// <summary>
    /// Standard API error response model.
    /// </summary>
    public sealed class ErrorResponse
    {
        /// <summary>
        /// Gets or sets the correlation identifier for tracing logs.
        /// </summary>
        public string CorrelationId { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the HTTP status code.
        /// </summary>
        public int StatusCode { get; set; }

        /// <summary>
        /// Gets or sets the error message.
        /// </summary>
        public string Message { get; set; } = string.Empty;
    }
}
