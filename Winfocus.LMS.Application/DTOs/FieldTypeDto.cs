using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a field type enumeration value.
    /// Used for API responses.
    /// </summary>
    public class FieldTypeDto
    {
        /// <summary>
        /// Gets or sets the enum numeric value.
        /// </summary>
        public int Id { get; set; }

        /// <summary>
        /// Gets or sets the enum name.
        /// </summary>
        public string Name { get; set; } = string.Empty;
    }
}
