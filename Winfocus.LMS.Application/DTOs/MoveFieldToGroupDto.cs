using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request DTO used to move a form field to a different group.
    /// </summary>
    public class MoveFieldToGroupDto
    {
        /// <summary>
        /// Gets or sets the target field group identifier.
        /// If NULL, the field becomes standalone.
        /// </summary>
        public Guid? FieldGroupId { get; set; }
    }
}
