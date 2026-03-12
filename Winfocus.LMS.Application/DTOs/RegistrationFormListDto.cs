using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// Represents a summary view of a registration form.
    /// This DTO is used in the form listing API to return
    /// lightweight information about registration forms
    /// without including full group and field details.
    /// </summary>
    public class RegistrationFormListDto
    {
        /// <summary>
        /// Gets or sets the unique identifier of the registration form.
        /// </summary>
        public Guid Id { get; set; }

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
        /// Gets or sets the name of the staff category associated with the form.
        /// </summary>
        /// <remarks>
        /// This value indicates which staff type the form belongs to
        /// (e.g., Teacher, Accountant, Administrator).
        /// </remarks>
        public string StaffCategoryName { get; set; }

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

        /// <summary>
        /// Gets or sets a value indicating whether the form is active.
        /// </summary>
        /// <remarks>
        /// If <c>true</c>, the form is currently available for use.
        /// If <c>false</c>, the form has been soft deleted or disabled.
        /// </remarks>
        public bool IsActive { get; set; }
    }
}
