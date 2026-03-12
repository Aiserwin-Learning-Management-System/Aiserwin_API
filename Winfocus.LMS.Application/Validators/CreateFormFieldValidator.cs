using FluentValidation;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Validators
{
    /// <summary>
    /// Validator for CreateFormFieldValidator.
    /// </summary>
    public class CreateFormFieldValidator : AbstractValidator<CreateFormFieldDto>
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="CreateFormFieldValidator"/> class.
        /// </summary>
        public CreateFormFieldValidator()
        {
            // FieldName validation
            RuleFor(x => x.FieldName)
                .NotEmpty().WithMessage("FieldName is required")
                .MaximumLength(100)
                .Matches(@"^[a-zA-Z0-9_]+$")
                .WithMessage("FieldName must be alphanumeric with underscore only");

            // DisplayLabel validation
            RuleFor(x => x.DisplayLabel)
                .NotEmpty().WithMessage("DisplayLabel is required")
                .MaximumLength(200);

            // FieldType validation
            RuleFor(x => x.FieldType)
                .IsInEnum()
                .WithMessage("Invalid FieldType");

            // Options required for Dropdown / Radio / Checkbox
            RuleFor(x => x.Options)
                .NotNull()
                .NotEmpty()
                .When(x => x.FieldType == FieldType.Dropdown ||
                           x.FieldType == FieldType.Radio ||
                           x.FieldType == FieldType.Checkbox)
                .WithMessage("Options are required for Dropdown, Radio and Checkbox fields");

            // Options not allowed for other types
            RuleFor(x => x.Options)
                .Must(o => o == null || !o.Any())
                .When(x => x.FieldType == FieldType.Text ||
                           x.FieldType == FieldType.Number ||
                           x.FieldType == FieldType.Date ||
                           x.FieldType == FieldType.FileUpload)
                .WithMessage("Options are not allowed for this field type");

            // MinLength / MaxLength only for Text / Textarea
            RuleFor(x => x.MinLength)
                .NotNull()
                .When(x => x.FieldType == FieldType.Text || x.FieldType == FieldType.Textarea);

            RuleFor(x => x.MaxLength)
                .NotNull()
                .When(x => x.FieldType == FieldType.Text || x.FieldType == FieldType.Textarea);

            RuleFor(x => x.MinLength)
                .Null()
                .When(x => x.FieldType != FieldType.Text && x.FieldType != FieldType.Textarea)
                .WithMessage("MinLength only allowed for Text or Textarea");

            RuleFor(x => x.MaxLength)
                .Null()
                .When(x => x.FieldType != FieldType.Text && x.FieldType != FieldType.Textarea)
                .WithMessage("MaxLength only allowed for Text or Textarea");

            // Regex validation
            RuleFor(x => x.ValidationRegex)
                .Must(BeValidRegex)
                .When(x => !string.IsNullOrEmpty(x.ValidationRegex))
                .WithMessage("Invalid regex pattern");
        }

        private bool BeValidRegex(string? pattern)
        {
            if (string.IsNullOrEmpty(pattern))
                return true;

            try
            {
                Regex.Match("", pattern);
                return true;
            }
            catch
            {
                return false;
            }
        }
    }
}
