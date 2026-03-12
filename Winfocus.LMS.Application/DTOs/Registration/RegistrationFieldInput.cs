namespace Winfocus.LMS.Application.DTOs.Registration
{
    using System.Text.Json.Serialization;
    using Microsoft.AspNetCore.Http;

    /// <summary>
    /// A single field value in the registration submission.
    /// </summary>
    public class RegistrationFieldInput
    {
        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        /// <value>
        /// The field identifier.
        /// </value>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the value.
        /// </summary>
        /// <value>
        /// The value.
        /// </value>
        public string? Value { get; set; }

        /// <summary>
        /// Gets or sets the file.
        /// </summary>
        /// <value>
        /// The file.
        /// </value>
        [JsonIgnore]
        public IFormFile? File { get; set; }
    }
}
