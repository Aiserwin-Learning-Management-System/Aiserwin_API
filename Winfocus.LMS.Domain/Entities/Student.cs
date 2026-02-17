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
        public Guid StudentAcademicDetailsId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's academic details.
        /// </summary>
        public StudentAcademicDetails AcademicDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's personal details.
        /// </summary>
        public Guid StudentPersonalDetailsId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's personal details.
        /// </summary>
        public StudentPersonalDetails StudentPersonalDetails { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the student's documents record.
        /// </summary>
        public Guid StudentDocumentsId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the student's documents.
        /// </summary>
        public StudentDocuments StudentDocuments { get; set; } = null!;

        /// <summary>
        /// Gets or sets a value indicating whether gets or sets the identifier of the student's registration from.
        /// </summary>
        public bool Isscholershipstudent { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the Status.
        /// </summary>
        public string Status { get; set; } = null!;

        /// <summary>
        /// Gets or sets the student academic couses.
        /// </summary>
        /// <value>
        /// The student academic couses.
        /// </value>
        public ICollection<StudentAcademicCouses> StudentAcademicCouses { get; set; }
    = new List<StudentAcademicCouses>();

        /// <summary>
        /// Gets or sets the student BatchTimingMTFs.
        /// </summary>
        /// <value>
        /// The student BatchTimingMTFs.
        /// </value>
        public ICollection<StudentBatchTimingMTF> StudentBatchTimingMTFs { get; set; }
            = new List<StudentBatchTimingMTF>();

        /// <summary>
        /// Gets or sets the student BatchTimingSaturdays.
        /// </summary>
        /// <value>
        /// The student BatchTimingSaturdays.
        /// </value>
        public ICollection<StudentBatchTimingSaturday> StudentBatchTimingSaturdays { get; set; }
            = new List<StudentBatchTimingSaturday>();

        /// <summary>
        /// Gets or sets the student BatchTimingSundays.
        /// </summary>
        /// <value>
        /// The student BatchTimingSundays.
        /// </value>
        public ICollection<StudentBatchTimingSunday> StudentBatchTimingSundays { get; set; }
            = new List<StudentBatchTimingSunday>();

    }
}
