using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO representing a standalone field.
    /// </summary>
    public class StandaloneFieldDto
    {
        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets whether the field is required.
        /// </summary>
        public bool IsRequired { get; set; }
    }
}
