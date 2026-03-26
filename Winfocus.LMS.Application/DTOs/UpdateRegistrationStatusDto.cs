using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// UpdateRegistrationStatusDto.
    /// </summary>
    public class UpdateRegistrationStatusDto
    {
        /// <summary>
        /// Gets or sets status.
        /// </summary>
        public RegistrationStatus Status { get; set; }

        /// <summary>
        /// Gets or sets Remarks.
        /// </summary>
        public string? Remarks { get; set; }
    }
}
