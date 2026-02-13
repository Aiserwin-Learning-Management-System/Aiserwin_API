using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a standard API response wrapper.
    /// </summary>
    /// <typeparam name="T">Type of response data.</typeparam>
    public class CommonResponse<T>
    {
        /// <summary>
        /// Gets or sets a value indicating whether indicates whether the request was successful.
        /// </summary>
        public bool Success { get; set; }

        /// <summary>
        /// Gets or sets the response message.
        /// </summary>
        public string Message { get; set; }

        /// <summary>
        /// Gets or sets the response data.
        /// </summary>
        public T Data { get; set; }

        /// <summary>
        /// Initializes a new instance of the <see cref="CommonResponse{T}"/> class.
        /// Prevents direct instantiation of the response.
        /// </summary>
        private CommonResponse() { }

        /// <summary>
        /// Creates a successful response.
        /// </summary>
        /// <param name="message">Success message.</param>
        /// <param name="data">Response data.</param>
        /// <returns>Successful response instance.</returns>
        public static CommonResponse<T> SuccessResponse(string message, T data)
        {
            return new CommonResponse<T>
            {
                Success = true,
                Message = message,
                Data = data
            };
        }

        /// <summary>
        /// Creates a failure response.
        /// </summary>
        /// <param name="message">Failure message.</param>
        /// <returns>Failure response instance.</returns>
        public static CommonResponse<T> FailureResponse(string message)
        {
            return new CommonResponse<T>
            {
                Success = false,
                Message = message,
                Data = default,
            };
        }
    }

}
