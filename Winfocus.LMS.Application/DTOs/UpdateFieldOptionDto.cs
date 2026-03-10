using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO used to update existing field options.
    /// </summary>
    public class UpdateFieldOptionDto
    {
        /// <summary>
        /// Gets or sets the option identifier.
        /// </summary>
        public Guid? Id { get; set; }

        /// <summary>
        /// Gets or sets the option value.
        /// </summary>
        public string OptionValue { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display order.
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        /// Gets or sets whether the option is active.
        /// </summary>
        public bool IsActive { get; set; }
    }
}
