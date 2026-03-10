using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents minimal field group information.
    /// </summary>
    public class FieldGroupDto
    {
        /// <summary>
        /// Gets or sets the GroupId.
        /// </summary>
        public Guid GroupId { get; set; }

        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>

        public string GroupName { get; set; } = string.Empty;
    }
}
