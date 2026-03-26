namespace Winfocus.LMS.Domain.Enums
{
    /// <summary>
    /// Defines the supported input types for dynamic form fields.
    /// Stored as INT in the database.
    /// </summary>
    public enum FieldType
    {
        /// <summary>Single-line text input.</summary>
        Text = 0,

        /// <summary>Multi-line text area.</summary>
        Textarea = 1,

        /// <summary>Numeric input.</summary>
        Number = 2,

        /// <summary>Date picker.</summary>
        Date = 3,

        /// <summary>Dropdown select (uses FieldOptions).</summary>
        Dropdown = 4,

        /// <summary>Checkbox (uses FieldOptions for multiple).</summary>
        Checkbox = 5,

        /// <summary>Radio button group (uses FieldOptions).</summary>
        Radio = 6,

        /// <summary>File upload input.</summary>
        FileUpload = 7,

        /// <summary>Email input with built-in validation.</summary>
        Email = 8,

        /// <summary>Phone number input.</summary>
        Phone = 9,
    }
}
