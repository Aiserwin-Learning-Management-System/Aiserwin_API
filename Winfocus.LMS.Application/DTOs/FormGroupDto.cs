using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO representing a field group added to the form.
    /// </summary>
    public class FormGroupDto
    {
        /// <summary>
        /// Gets or sets the field group identifier.
        /// </summary>
        public Guid FieldGroupId { get; set; }

        /// <summary>
        /// Gets or sets the display order of the group in the form.
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
