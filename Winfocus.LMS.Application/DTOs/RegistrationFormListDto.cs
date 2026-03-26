using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a summary view of a registration form.
    /// This DTO is used in the form listing API to return
    /// lightweight information about registration forms
    /// without including full group and field details.
    /// </summary>
    public class RegistrationFormListDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the name of the registration form.
        /// </summary>
        /// <remarks>
        /// This value represents the display name used by administrators
        /// to identify the form (e.g., "Teacher Registration Form").
        /// </remarks>
        public string FormName { get; set; }

        /// <summary>
        /// Gets or sets the staff category this form is designed for.
        /// </summary>
        public Guid StaffCategoryId { get; set; }

        /// <summary>
        /// Gets or sets the staff category this form belongs to.
        /// </summary>
        public StaffCategoryDto StaffCategory { get; set; } = null!;

        /// <summary>
        /// Gets or sets the total number of field groups included in the form.
        /// </summary>
        /// <remarks>
        /// This value represents how many groups are configured
        /// for the form (e.g., Personal Information, Address, Education).
        /// </remarks>
        public int GroupCount { get; set; }

        /// <summary>
        /// Gets or sets the total number of fields included in the form.
        /// </summary>
        /// <remarks>
        /// This count includes both:
        /// - Fields coming from selected groups
        /// - Standalone fields added directly to the form.
        /// </remarks>
        public int FieldCount { get; set; }
    }
}
