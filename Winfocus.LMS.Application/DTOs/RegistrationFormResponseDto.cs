using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// DTO returned when fetching a registration form.
    /// </summary>
    public class RegistrationFormResponseDto
    {
        /// <summary>
        /// Gets or sets the field identifier.
        /// </summary>
        public Guid Id { get; set; }

        /// <summary>
        /// Gets or sets the StaffCategoryId.
        /// </summary>
        public Guid StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the staff category this form belongs to.
        /// </summary>
        public StaffCategoryDto StaffCategory { get; set; } = null!;

        /// <summary>
        /// Gets or sets the FormName.
        /// </summary>
        public string FormName { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the IsActive.
        /// </summary>
        public bool IsActive { get; set; }

        /// <summary>
        /// Gets or sets form sections (groups + standalone).
        /// </summary>
        public List<FormSectionDto> Sections { get; set; }
    }
}
