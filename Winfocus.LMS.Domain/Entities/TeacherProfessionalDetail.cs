using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Common;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Domain.Entities
{
    /// <summary>
    /// Represents the professional and academic details of a teacher.
    /// </summary>
    public class TeacherProfessionalDetail : BaseEntity
    {
        /// <summary>
        /// Gets or sets the associated teacher identifier.
        /// </summary>
        public Guid TeacherId { get; set; }

        /// <summary>
        /// Gets or sets the highest qualification.
        /// </summary>
        public string HighestQualification { get; set; } = null!;

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
        /// Gets or sets the computer literacy level.
        /// </summary>
        public ComputerLiteracy ComputerLiteracy { get; set; }

        /// <summary>
        /// Gets or sets the preferred subjects.
        /// </summary>
        public ICollection<TeacherPreferredSubject> PreferredSubjects { get; set; } = new List<TeacherPreferredSubject>();

        /// <summary>
        /// Gets or sets the lms tools.
        /// </summary>
        public ICollection<TeacherTool> TeacherTools { get; set; } = new List<TeacherTool>();

        /// <summary>
        /// Gets or sets the preferred syllabuses.
        /// </summary>
        public ICollection<TeacherSyllabus> PreferredSyllabuses { get; set; } = new List<TeacherSyllabus>();

        /// <summary>
        /// Gets or sets the preferred grades.
        /// </summary>
        public ICollection<TeacherPreferredGrade> PreferredGrades { get; set; } = new List<TeacherPreferredGrade>();

        /// <summary>
        /// Gets or sets the preferred grades.
        /// </summary>
        public ICollection<TeacherLanguage> TeacherLanguage { get; set; } = new List<TeacherLanguage>();

        /// <summary>
        /// Gets or sets the TeacherAvailability.
        /// </summary>
        public ICollection<TeacherAvailability> TeacherAvailability { get; set; } = new List<TeacherAvailability>();
    }
}
