using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents the response model containing heading details 
    /// for a specific admin page.
    /// </summary>
    public class PageHeadingResponseDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the unique key that identifies the page.
        /// This key is used to retrieve or update the heading information.
        /// </summary>
        public string PageKey { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the main heading displayed at the top of the page.
        /// </summary>
        public string MainHeading { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the sub heading displayed below the main heading.
        /// </summary>
        public string SubHeading { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the module name to which the page belongs.
        /// For example: Administration, Students, Courses, etc.
        /// </summary>
        public string ModuleName { get; set; } = string.Empty;
    }
}
