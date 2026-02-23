namespace Winfocus.LMS.Application.Common.Exceptions
{
    /// <summary>
    /// AppException.
    /// </summary>
    /// <seealso cref="System.Exception" />
    /// <remarks>
    /// Initializes a new instance of the <see cref="AppException"/> class.
    /// </remarks>
    /// <param name="message">The message.</param>
    /// <param name="statusCode">The status code.</param>
    /// <param name="errorCode">The error code.</param>
    /// <param name="errors">The errors.</param>
    public abstract class AppException(
        string message,
        int statusCode,
        string errorCode,
        object? errors = null) : Exception(message)
    {
        /// <summary>
        /// Gets the status code.
        /// </summary>
        /// <value>
        /// The status code.
        /// </value>
        public int StatusCode { get; } = statusCode;

        /// <summary>
        /// Gets the error code.
        /// </summary>
        /// <value>
        /// The error code.
        /// </value>
        public string ErrorCode { get; } = errorCode;

        /// <summary>
        /// Gets the errors.
        /// </summary>
        /// <value>
        /// The errors.
        /// </value>
        public object? Errors { get; } = errors;
    }
}
