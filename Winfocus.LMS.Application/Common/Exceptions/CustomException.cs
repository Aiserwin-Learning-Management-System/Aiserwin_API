namespace Winfocus.LMS.Application.Common.Exceptions
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// CustomException.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Common.Exceptions.AppException" />
    public sealed class CustomException : AppException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CustomException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="statusCode">The status code.</param>
        /// <param name="errorCode">The error code.</param>
        /// <param name="errors">The errors.</param>
        public CustomException(
            string message,
            int statusCode = StatusCodes.Status400BadRequest,
            string errorCode = "CUSTOM_ERROR",
            object? errors = null)
            : base(message, statusCode, errorCode, errors)
        {
        }
    }
}
