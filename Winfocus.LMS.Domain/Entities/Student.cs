namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.ComponentModel.DataAnnotations.Schema;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents a student and references to their related details entities.
    /// </summary>
    public class Student : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the student's registration number.
        /// </summary>
        public string RegistrationNumber { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's academic details.
        /// </summary>
        public Guid StudentAcademicId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's academic details.
        /// </summary>
        public StudentAcademicDetails AcademicDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's personal details.
        /// </summary>
        public Guid StudentPersonalId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's personal details.
        /// </summary>
        public StudentPersonalDetails PersonalDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's documents record.
        /// </summary>
        public Guid StudentDocumentsId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's documents.
        /// </summary>
        public StudentDocuments StudentDocuments { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student academic couses.
        /// </summary>
        /// <value>
        /// The student academic couses.
        /// </value>
        public ICollection<StudentAcademicCouses> StudentAcademicCouses { get; set; }
    = new List<StudentAcademicCouses>();
    }
}
