using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Request for creating or updating a FieldGroup.
    /// </summary>
    public sealed record CreateFieldGroupRequest([Required] string groupName, string description, int displayOrder);
}
