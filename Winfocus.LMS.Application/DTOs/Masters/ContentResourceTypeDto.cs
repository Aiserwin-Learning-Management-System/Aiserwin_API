using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// ContentResourceTypeDto.
    /// </summary>
    public class ContentResourceTypeDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the display name of the ContentResourceType.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        public string Description { get; set; } = null!;

        /// <summary>
        /// Gets or sets the chapter identifier.
        /// </summary>
        /// <value>
        /// The chapter identifier.
        /// </value>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the chapter.
        /// </summary>
        /// <value>
        /// The chapter.
        /// </value>
        public ExamChapterDto? Chapter { get; set; }
    }
}
