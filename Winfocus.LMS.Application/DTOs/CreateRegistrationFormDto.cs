using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO used to create a new registration form.
    /// Admin can select groups and standalone fields.
    /// </summary>
    public class CreateRegistrationFormDto
    {
        /// <summary>
        /// Gets or sets the staff category identifier.
        /// </summary>
        public Guid StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the name of the form.
        /// </summary>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets the description of the form.
        /// </summary>
        public string Description { get; set; }

        /// <summary>
        /// Gets or sets the selected field groups.
        /// </summary>
        public List<FormGroupDto> Groups { get; set; }

        /// <summary>
        /// Gets or sets standalone fields not belonging to any group.
        /// </summary>
        public List<StandaloneFieldDto> StandaloneFields { get; set; }
    }
}
