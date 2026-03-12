using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a section inside the form.
    /// Can be a group or standalone section.
    /// </summary>
    public class FormSectionDto
    {
        /// <summary>
        /// Gets or sets section type (group / standalone).
        /// </summary>
        public string Type { get; set; } = null!;

        /// <summary>
        /// Gets or sets GroupId.
        /// </summary>
        public Guid? GroupId { get; set; }

        /// <summary>
        /// Gets or sets GroupName.
        /// </summary>
        public string GroupName { get; set; } = null!;

        /// <summary>
        /// Gets or sets DisplayOrder.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets section type (group / standalone).
        /// </summary>
        public List<FormFieldListDto> Fields { get; set; } = null!;
    }
}
