using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Links a <see cref="FieldGroupDto"/> to a <see cref="RegistrationFormGroupsDto"/>,
    /// defining which field groups appear on the form and in what order.
    /// Lightweight junction entity — no audit fields needed.
    /// </summary>
    public class RegistrationFormGroupsDto
    {
        /// <summary>
        /// Gets or sets the unique identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the parent registration form.
        /// </summary>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the field group being linked to the form.
        /// </summary>
        public Guid FieldGroupId { get; set; }

        /// <summary>
        /// Gets or sets the display order of this group within the form.
        /// Lower values appear first.
        /// </summary>
        public int DisplayOrder { get; set; } = 0;

        /// <summary>
        /// Gets or sets the parent registration form.
        /// </summary>
        public RegistrationForm RegistrationForm { get; set; } = null!;

        /// <summary>
        /// Gets or sets the linked field group definition.
        /// </summary>
        public FieldGroupDto FieldGroup { get; set; } = null!;
    }
}
