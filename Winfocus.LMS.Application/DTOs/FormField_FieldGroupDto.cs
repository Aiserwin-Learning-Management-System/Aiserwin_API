using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// FormField_FieldGroupDto fields.
    /// </summary>
    public class FormField_FieldGroupDto
    {
        /// <summary>
        ///  Gets or sets name .
        /// </summary>
        public Guid FieldId { get; set; }

        /// <summary>
        ///  Gets or sets name .
        /// </summary>
        public string FieldName { get; set; } = string.Empty;

        /// <summary>
        ///  Gets or sets name .
        /// </summary>
        public string DisplayLabel { get; set; } = string.Empty;

        /// <summary>
        ///  Gets or sets name .
        /// </summary>
        public FieldType FieldType { get; set; }

        /// <summary>
        ///  Gets or sets a value indicating whether gets or sets IsRequired .
        /// </summary>
        public bool IsRequired { get; set; }

        /// <summary>
        ///  Gets or sets name .
        /// </summary>
        public int DisplayOrder { get; set; }
    }
}
