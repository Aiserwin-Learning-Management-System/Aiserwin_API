using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a selectable field option.
    /// </summary>
    public class FieldOptionDto
    {
        public Guid Id { get; set; }

        public string OptionValue { get; set; } = string.Empty;

        public int DisplayOrder { get; set; }
    }
}
