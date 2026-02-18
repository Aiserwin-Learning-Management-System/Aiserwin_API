namespace Winfocus.LMS.Application.Common.Exceptions
{
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// ValidationException.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Common.Exceptions.AppException" />
    public sealed class ValidationException : AppException
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="ValidationException"/> class.
        /// </summary>
        /// <param name="message">The message.</param>
        /// <param name="errors">The errors.</param>
        public ValidationException(
            string message,
            IDictionary<string, string[]> errors)
            : base(
                message,
                StatusCodes.Status400BadRequest,
                "VALIDATION_ERROR",
                errors)
        {
        }
    }
}
