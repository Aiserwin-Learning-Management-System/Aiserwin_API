using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Teacher
{
    /// <summary>
    /// DTO used for creating or updating teaching tools.
    /// </summary>
    public class TeachingToolsDto
    {
        /// <summary>
        /// Gets or sets the display name of the tool.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display description of the tool.
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the userid.
        /// </summary>
        public Guid Userid { get; set; }
    }

    /// <summary>
    /// DTO used for returning teaching tools data.
    /// </summary>
    public class TeachingToolsResponseDto
    {
        /// <summary>
        /// Gets or sets the display name of the tool.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the display name of the tool.
        /// </summary>
        public string Name { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets the display description of the tool.
        /// </summary>
        public string Description { get; set; } = string.Empty;
    }
}
