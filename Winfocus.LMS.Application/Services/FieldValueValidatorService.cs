using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Service responsible for managing Validations form fields.
    /// </summary>
    public class FieldValueValidatorService : IFieldValueValidatorService
    {
        private const int MaxFileSize = 5 * 1024 * 1024; // 5MB

        /// <summary>
        /// ValidateAsync.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        public async Task<bool> ValidateAsync(FormField field, object? value)
        {
            if (value == null)
                return !field.IsRequired;

            switch (field.FieldType)
            {
                case FieldType.Text:
                    return ValidateText(field, value.ToString());

                case FieldType.Number:
                    return ValidateNumber(value.ToString());

                case FieldType.Date:
                    return ValidateDate(value.ToString());

                case FieldType.Email:
                    return ValidateEmail(value.ToString());

                case FieldType.Phone:
                    return ValidatePhone(value.ToString());

                case FieldType.Dropdown:
                case FieldType.Radio:
                    return ValidateOption(field, value.ToString());

                case FieldType.Checkbox:
                    return ValidateCheckbox(field, value);

                case FieldType.FileUpload:
                    return ValidateFile(value);

                default:
                    return false;
            }
        }

        private bool ValidateText(FormField field, string? value)
        {
            if (string.IsNullOrWhiteSpace(value))
                return false;

            if (field.MinLength.HasValue && value.Length < field.MinLength)
                return false;

            if (field.MaxLength.HasValue && value.Length > field.MaxLength)
                return false;

            if (!string.IsNullOrEmpty(field.ValidationRegex))
            {
                if (!Regex.IsMatch(value, field.ValidationRegex))
                    return false;
            }

            return true;
        }

        private bool ValidateNumber(string? value)
        {
            return decimal.TryParse(value, out _);
        }

        private bool ValidateDate(string? value)
        {
            return DateTime.TryParse(value, out _);
        }

        private bool ValidateEmail(string? value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return Regex.IsMatch(value,
                @"^[^@\s]+@[^@\s]+\.[^@\s]+$",
                RegexOptions.IgnoreCase);
        }

        private bool ValidatePhone(string? value)
        {
            return Regex.IsMatch(value ?? "", @"^\+?[0-9]{10,15}$");
        }

        private bool ValidateOption(FormField field, string? value)
        {
            if (string.IsNullOrEmpty(value))
                return false;

            return field.FieldOptions.Any(o => o.OptionValue == value);
        }

        private bool ValidateCheckbox(FormField field, object value)
        {
            var values = value as IEnumerable<string>;

            if (values == null)
                return false;

            return values.All(v =>
                field.FieldOptions.Any(o => o.OptionValue == v));
        }

        private bool ValidateFile(object value)
        {
            var file = value as IFormFile;

            if (file == null)
                return false;

            var allowedExtensions = new[] { ".jpg", ".png", ".pdf" };

            var extension = Path.GetExtension(file.FileName).ToLower();

            if (!allowedExtensions.Contains(extension))
                return false;

            if (file.Length > MaxFileSize)
                return false;

            return true;
        }



    }
}
