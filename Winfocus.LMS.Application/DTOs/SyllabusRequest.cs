namespace Winfocus.LMS.Application.DTOs
{
    using System.ComponentModel.DataAnnotations;

    public sealed record SyllabusRequest
    {
        /// <summary>
        /// Gets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        [Required]
        [StringLength(200, MinimumLength = 1)]
        public string Name { get; init; } = null!;

        /// <summary>
        /// Gets the user identifier.
        /// </summary>
        /// <value>
        /// The user identifier.
        /// </value>
        public Guid UserId { get; init; }

        /// <summary>
        /// Gets the Ceneter identifier.
        /// </summary>
        /// <value>
        /// The Ceneter identifier.
        /// </value>
        public Guid CeneterId { get; init; }
    }
}
