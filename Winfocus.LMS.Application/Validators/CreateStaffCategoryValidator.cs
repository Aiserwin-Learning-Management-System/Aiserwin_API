using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// Validator for StaffCategoryRequestDto.
    /// </summary>
    public class CreateStaffCategoryValidator : AbstractValidator<StaffCategoryRequestDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateStaffCategoryValidator"/> class.
        /// </summary>
        public CreateStaffCategoryValidator()
        {
            RuleFor(x => x.name)
                .NotEmpty().WithMessage("Name is required")
                .MaximumLength(100).WithMessage("Name must not exceed 100 characters")
                .Matches("^[a-zA-Z0-9 ]*$")
                .WithMessage("Name must not contain special characters");

            RuleFor(x => x.description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");
        }
    }
}
