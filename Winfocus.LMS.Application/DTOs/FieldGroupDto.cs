using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// FieldGroup Dto.
    /// </summary>
    public class FieldGroupDto
    {
        /// <summary>
        /// Gets or sets the GroupName.
        /// </summary>
        public string GroupName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the Description.
        /// </summary>
        /// <value>
        /// The Description.
        /// </value>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the DisplayOrder.
        /// </summary>
        /// <value>
        /// The DisplayOrder.
        /// </value>
        public int DisplayOrder { get; set; }
    }
}
