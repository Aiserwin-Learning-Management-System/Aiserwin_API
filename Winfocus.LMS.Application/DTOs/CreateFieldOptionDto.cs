using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a selectable option for a form field.
    /// Used when creating dropdown, checkbox, or radio fields.
    /// </summary>
    public class CreateFieldOptionDto
    {
        /// <summary>
        /// Gets or sets the option display value.
        /// </summary>
        /// <example>Male.</example>
        public string OptionValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display order of the option.
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
