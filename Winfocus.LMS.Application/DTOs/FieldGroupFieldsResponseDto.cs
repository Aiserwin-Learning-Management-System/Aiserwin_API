using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// FieldGroupFieldsResponseDto fields.
    /// </summary>
    public class FieldGroupFieldsResponseDto : BaseClassDTO
    {
        /// <summary>Gets or sets s
        ///  Gets or sets name .
        /// </summary>
        public string GroupName { get; set; } = string.Empty;

        /// <summary>
        ///  Gets or sets Description .
        /// </summary>
        public string Description { get; set; } = string.Empty;

        /// <summary>
        ///  Gets or sets DisplayOrder .
        /// </summary>
        public int DisplayOrder { get; set; }

        /// <summary>
        ///  Gets or sets FieldCount .
        /// </summary>
        public int FieldCount { get; set; }

        /// <summary>
        ///  Gets or sets Fields .
        /// </summary>
        public List<FormField_FieldGroupDto> Fields { get; set; } = new();
    }
}
