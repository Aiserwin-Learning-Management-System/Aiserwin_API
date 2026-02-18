namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using Winfocus.LMS.Domain.Common;

    /// <summary>
    /// Represents the academic details associated with a student, including course, grade, timings, and related navigation properties.
    /// </summary>
    public class StudentAcademicDetails : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Country Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated mode of study.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated mode of study.
        /// </summary>
        public ModeOfStudy ModeOfStudy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated state.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated state.
        /// </summary>
        public State State { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated center.
        /// </summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated center.
        /// </summary>
        public Centre Center { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Syllabus Syllabus { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Grade Grade { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Streams Stream { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Subject Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the  batch  selection.
        /// </summary>
        public Guid BatchId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the preferred batch time selection.
        /// </summary>
        public Guid PreferredBatchTimeId { get; set; }

        /// <summary>
        /// Gets or sets the past year performance details (textual).
        /// </summary>
        public string PastYearPerformance { get; set; } = null!;

        /// <summary>
        /// Gets or sets the name of the past school attended.
        /// </summary>
        public string PastSchoolName { get; set; } = null!;

        /// <summary>
        /// Gets or sets the location of the past school attended.
        /// </summary>
        public string PastSchoolLocation { get; set; } = null!;

        /// <summary>
        /// Gets or sets the emirate associated with the student's record.
        /// </summary>
        public string Emirates { get; set; } = null!;
    }
}
