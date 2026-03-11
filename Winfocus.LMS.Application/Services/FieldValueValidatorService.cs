namespace Winfocus.LMS.Application.Services
{
    using System.Text.RegularExpressions;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Validates submitted field values against their type rules.
    /// </summary>
    public class FieldValueValidatorService : IFieldValueValidatorService
    {
        private const string _defaultEmailPattern = @"^[^@\s]+@[^@\s]+\.[^@\s]+$";
        private const string _defaultPhonePattern = @"^\+?[\d\s\-]{7,15}$";

        /// <inheritdoc/>
        public List<string> Validate(FormField field, string? value, bool isRequired)
        {
            var errors = new List<string>();
            var isEmpty = string.IsNullOrWhiteSpace(value);

            if (isRequired && isEmpty)
            {
                errors.Add($"{field.DisplayLabel} is required.");
                return errors;
            }

            if (isEmpty)
            {
                return errors;
            }

            switch (field.FieldType)
            {
                case FieldType.Text:
                case FieldType.Textarea:
                    ValidateLength(field, value!, errors);
                    ValidateRegex(field, value!, errors);
                    break;

                case FieldType.Number:
                    ValidateNumber(field, value!, errors);
                    break;

                case FieldType.Date:
                    ValidateDate(field, value!, errors);
                    break;

                case FieldType.Email:
                    ValidateEmail(field, value!, errors);
                    break;

                case FieldType.Phone:
                    ValidatePhone(field, value!, errors);
                    break;

                case FieldType.Dropdown:
                case FieldType.Radio:
                    ValidateSingleOption(field, value!, errors);
                    break;

                case FieldType.Checkbox:
                    ValidateMultipleOptions(field, value!, errors);
                    break;

                case FieldType.FileUpload:
                    break;
            }

            return errors;
        }

        private void ValidateLength(FormField field, string value, List<string> errors)
        {
            if (field.MinLength.HasValue && value.Length < field.MinLength.Value)
            {
                errors.Add(
                    $"{field.DisplayLabel} must be at least {field.MinLength.Value} characters.");
            }

            if (field.MaxLength.HasValue && value.Length > field.MaxLength.Value)
            {
                errors.Add(
                    $"{field.DisplayLabel} must not exceed {field.MaxLength.Value} characters.");
            }
        }

        private void ValidateRegex(FormField field, string value, List<string> errors)
        {
            if (!string.IsNullOrEmpty(field.ValidationRegex))
            {
                try
                {
                    if (!Regex.IsMatch(value, field.ValidationRegex))
                    {
                        errors.Add($"{field.DisplayLabel} format is invalid.");
                    }
                }
                catch (RegexParseException)
                {
                    errors.Add($"{field.DisplayLabel} has an invalid validation pattern configured.");
                }
            }
        }

        private void ValidateNumber(FormField field, string value, List<string> errors)
        {
            if (!decimal.TryParse(value, out _))
            {
                errors.Add($"{field.DisplayLabel} must be a valid number.");
            }
            else
            {
                ValidateRegex(field, value, errors);
            }
        }

        private void ValidateDate(FormField field, string value, List<string> errors)
        {
            if (!DateTime.TryParse(value, out _))
            {
                errors.Add($"{field.DisplayLabel} must be a valid date.");
            }
        }

        private void ValidateEmail(FormField field, string value, List<string> errors)
        {
            var pattern = !string.IsNullOrEmpty(field.ValidationRegex)
                ? field.ValidationRegex
                : _defaultEmailPattern;

            try
            {
                if (!Regex.IsMatch(value, pattern))
                {
                    errors.Add($"{field.DisplayLabel} must be a valid email address.");
                }
            }
            catch (RegexParseException)
            {
                if (!Regex.IsMatch(value, _defaultEmailPattern))
                {
                    errors.Add($"{field.DisplayLabel} must be a valid email address.");
                }
            }
        }

        private void ValidatePhone(FormField field, string value, List<string> errors)
        {
            var pattern = !string.IsNullOrEmpty(field.ValidationRegex)
                ? field.ValidationRegex
                : _defaultPhonePattern;

            try
            {
                if (!Regex.IsMatch(value, pattern))
                {
                    errors.Add($"{field.DisplayLabel} must be a valid phone number.");
                }
            }
            catch (RegexParseException)
            {
                if (!Regex.IsMatch(value, _defaultPhonePattern))
                {
                    errors.Add($"{field.DisplayLabel} must be a valid phone number.");
                }
            }
        }

        private void ValidateSingleOption(FormField field, string value, List<string> errors)
        {
            var validOptions = field.FieldOptions
                .Where(o => o.IsActive)
                .Select(o => o.OptionValue)
                .ToList();

            if (validOptions.Count > 0 && !validOptions.Contains(value))
            {
                errors.Add(
                    $"{field.DisplayLabel}: '{value}' is not a valid option. " +
                    $"Valid options: {string.Join(", ", validOptions)}");
            }
        }

        private void ValidateMultipleOptions(FormField field, string value, List<string> errors)
        {
            var selectedValues = value
                .Split(',', StringSplitOptions.RemoveEmptyEntries | StringSplitOptions.TrimEntries);

            var validOptions = field.FieldOptions
                .Where(o => o.IsActive)
                .Select(o => o.OptionValue)
                .ToHashSet();

            if (validOptions.Count == 0)
            {
                return;
            }

            foreach (var selected in selectedValues)
            {
                if (!validOptions.Contains(selected))
                {
                    errors.Add(
                        $"{field.DisplayLabel}: '{selected}' is not a valid option.");
                }
            }
        }
    }
}
