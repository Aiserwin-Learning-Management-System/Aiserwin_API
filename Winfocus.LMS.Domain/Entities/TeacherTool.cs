namespace Winfocus.LMS.Domain.Entities
{
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents the association between a TeacherRegistration and a Tool.
    /// </summary>
    public class TeacherTool : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the teacher registration.
        /// </summary>
        public Guid TeacherRegistrationId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the tool.
        /// </summary>
        public Guid ToolId { get; set; }

        /// <summary>
        /// Gets or sets the teacher registration associated with this mapping.
        /// </summary>
        public TeacherRegistration TeacherRegistration { get; set; } = null!;

        /// <summary>
        /// Gets or sets the tool associated with this mapping.
        /// </summary>
        public Tool Tool { get; set; } = null!;
    }
}