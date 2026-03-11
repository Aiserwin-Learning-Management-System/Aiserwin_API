using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// Validator for CreateFieldGroupDtoValidator.
    /// </summary>
    public class CreateFieldGroupDtoValidator : AbstractValidator<CreateFieldGroupRequest>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFieldGroupDtoValidator"/> class.
        /// </summary>
        public CreateFieldGroupDtoValidator()
        {
            RuleFor(x => x.groupName)
               .MaximumLength(150)
               .WithMessage("groupName must not exceed 150 characters");

            RuleFor(x => x.description)
                .MaximumLength(500)
                .WithMessage("Description must not exceed 500 characters");
        }
    }
}
