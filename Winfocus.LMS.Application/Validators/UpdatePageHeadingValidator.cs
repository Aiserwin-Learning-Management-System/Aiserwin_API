using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// Validator for UpdatePageHeadingValidator.
    /// </summary>
    public class UpdatePageHeadingValidator : AbstractValidator<UpdatePageHeadingDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="UpdatePageHeadingValidator"/> class.
        /// </summary>
        public UpdatePageHeadingValidator()
        {
            RuleFor(x => x.MainHeading)
                .NotEmpty()
                .MaximumLength(200);

            RuleFor(x => x.SubHeading)
                .NotEmpty()
                .MaximumLength(200);
        }
    }
}
