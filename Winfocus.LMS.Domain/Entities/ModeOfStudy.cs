namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a mode of study (for example, full-time or part-time) and its country association.
    /// </summary>
    public class ModeOfStudy : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the mode of study.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated state.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated state.
        /// </summary>
        public State State { get; set; } = null!;
    }
}
