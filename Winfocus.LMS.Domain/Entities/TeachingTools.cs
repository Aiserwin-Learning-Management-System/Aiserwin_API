using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents Teaching Tools.
    /// </summary>
    public class TeachingTools : BaseEntity
    {
        /// <summary>
        /// Gets or sets the display name of the tool.
        /// </summary>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets thedescription of the tool.
        /// </summary>
        public string Description { get; set; } = default!;
    }
}
