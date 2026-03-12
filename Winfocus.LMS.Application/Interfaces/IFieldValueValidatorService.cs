using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Service responsible for managing Validations form fields.
    /// </summary>
    public interface IFieldValueValidatorService
    {
        /// <summary>
        /// ValidateAsync.
        /// </summary>
        /// <param name="field"></param>
        /// <param name="value"></param>
        /// <returns></returns>
        Task<bool> ValidateAsync(FormField field, object? value);
    }
}
