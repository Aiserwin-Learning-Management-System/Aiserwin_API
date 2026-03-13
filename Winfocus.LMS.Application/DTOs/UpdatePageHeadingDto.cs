using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents the request model used to update the heading
    /// information of a specific page.
    /// </summary>
    public class UpdatePageHeadingDto
    {
        /// <summary>
        /// Gets or sets the main heading displayed at the top of the page.
        /// </summary>
        public string MainHeading { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sub heading displayed below the main heading.
        /// </summary>
        public string SubHeading { get; set; } = string.Empty;
    }
}
