namespace Winfocus.LMS.Application.DTOs.Registration
{
    /// <summary>
    /// Request to submit a staff registration form.
    /// Supports both JSON and multipart/form-data.
    /// </summary>
    public class SubmitRegistrationRequest
    {
        /// <summary>
        /// Gets or sets the form identifier.
        /// </summary>
        /// <value>
        /// The form identifier.
        /// </value>
        public Guid FormId { get; set; }

        /// <summary>
        /// Gets or sets the values.
        /// </summary>
        /// <value>
        /// The values.
        /// </value>
        public List<RegistrationFieldInput> Values { get; set; } = new ();
    }
}
