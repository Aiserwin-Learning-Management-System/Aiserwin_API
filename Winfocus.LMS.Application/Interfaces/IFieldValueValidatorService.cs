namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Validates submitted field values against their type rules,
    /// regex patterns, length constraints, and allowed options.
    /// </summary>
    public interface IFieldValueValidatorService
    {
        /// <summary>
        /// Validates a value against the field's type and rules.
        /// </summary>
        /// <param name="field">The field definition (with FieldOptions loaded).</param>
        /// <param name="value">The submitted string value.</param>
        /// <param name="isRequired">Whether this field is required on the current form.</param>
        /// <returns>List of validation error messages. Empty if valid.</returns>
        List<string> Validate(FormField field, string? value, bool isRequired);
    }
}
