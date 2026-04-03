namespace Winfocus.LMS.Application.DTOs.Teacher
{
    using System;

    /// <summary>
    /// DTO for teacher professional details.
    /// </summary>
    public class TeacherProfessionalDetailDto
    {
        /// <summary>
        /// Gets or sets the highest qualification.
        /// </summary>
        public string HighestQualification { get; set; } = string.Empty;

        /// <summary>
        /// Gets or sets total teaching experience in years.
        /// </summary>
        public int TotalTeachingExperience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has online teaching experience.
        /// </summary>
        public bool HasOnlineTeachingExperience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher has offline teaching experience.
        /// </summary>
        public bool HasOfflineTeachingExperience { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the teacher is available for demo classes.
        /// </summary>
        public bool IsAvailableForDemoClass { get; set; }

        /// <summary>
        /// Gets or sets the computer literacy level (enum as int).
        /// </summary>
        public int ComputerLiteracy { get; set; }
    }
}
