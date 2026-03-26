namespace Winfocus.LMS.Application.Common.Exceptions
{
    /// <summary>
    /// AppException – custom application exception used by the global exception middleware.
    /// </summary>
    /// <seealso cref="System.Exception" />
    public class AppException : Exception
    {
        /// <summary>
        /// Gets the HTTP status code.
        /// </summary>
        /// <value>The status code.</value>
        public int StatusCode { get; }

        /// <summary>
        /// Gets the machine-readable error code.
        /// </summary>
        /// <value>The error code.</value>
        public string ErrorCode { get; }

        /// <summary>
        /// Gets the additional error details.
        /// </summary>
        /// <value>The errors.</value>
        public object? Errors { get; }

        /// <summary>
        /// Initializes a new instance of the <see cref="AppException"/> class.
        /// </summary>
        /// <param name="message">The human-readable message.</param>
        /// <param name="errorCode">The machine-readable error code.</param>
        /// <param name="statusCode">The HTTP status code.</param>
        /// <param name="errors">Optional additional error details.</param>
        public AppException(
            string message,
            int statusCode = 500,
            string errorCode = "INTERNAL_ERROR",
            object? errors = null)
            : base(message)
        {
            StatusCode = statusCode;
            ErrorCode = errorCode;
            Errors = errors;
        }
    }
}
