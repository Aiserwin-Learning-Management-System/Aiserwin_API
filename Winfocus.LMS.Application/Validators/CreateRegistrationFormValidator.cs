using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// Validator for CreateRegistrationFormValidator.
    /// </summary>
    public class CreateRegistrationFormValidator : AbstractValidator<CreateRegistrationFormDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateRegistrationFormValidator"/> class.
        /// </summary>
        public CreateRegistrationFormValidator()
        {
            RuleFor(x => x.StaffCategoryId)
                .NotEmpty();

            RuleFor(x => x.FormName)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x)
                .Must(x => (x.Groups?.Any() ?? false) || (x.StandaloneFields?.Any() ?? false))
                .WithMessage("At least one group or standalone field must be provided.");
        }
    }
}
