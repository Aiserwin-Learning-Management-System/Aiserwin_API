namespace Winfocus.LMS.Application.DTOs.Students
{
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Represents the personal details associated with a student.
    /// </summary>
    public class StudentPersonaldetailsdto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the full name of the student.
        /// </summary>
        public string FullName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student's email address.
        /// </summary>
        public string EmailAddress { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student's date of birth.
        /// </summary>
        public DateTime DOB { get; set; }

        /// <summary>
        /// Gets or sets the student's WhatsApp mobile number.
        /// </summary>
        public string MobileWhatsapp { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student's Botim mobile number.
        /// </summary>
        public string MobileBotim { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student's Comera mobile number.
        /// </summary>
        public string MobileComera { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the area where the student resides.
        /// </summary>
        public string AreaName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the district or location for the student.
        /// </summary>
        public string DistrictOrLocation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the emirate associated with the student's record.
        /// </summary>
        public string Emirates { get; set; } = null!;

        /// <summary>
        /// Gets or sets the type of the Gender (Male, Female).
        /// </summary>
        public Gender Gender { get; set; }
    }
}
