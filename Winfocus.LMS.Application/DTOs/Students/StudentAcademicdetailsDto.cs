namespace Winfocus.LMS.Application.DTOs.Students
{
    using Winfocus.LMS.Application.DTOs.Masters;

    /// <summary>
    /// Represents the academic details associated with a student, including course, grade, timings, and related navigation properties.
    /// </summary>
    public class StudentAcademicdetailsDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated country.
        /// </summary>
        public CountryDto1 Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated mode of study.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated mode of study.
        /// </summary>
        public ModeOfStudyDto ModeOfStudy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated state.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated state.
        /// </summary>
        public StateDto State { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated center.
        /// </summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated center.
        /// </summary>
        public CenterDto1 Center { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public SyllabusDto Syllabus { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public Guid GradeId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated grade.
        /// </summary>
        public GradeDto Grade { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public Guid StreamId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated stream.
        /// </summary>
        public StreamDto Stream { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public SubjectDto Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated batch timing for MTF.
        /// </summary>
        public Guid BatchTimingMTFId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated batch timing for MTF.
        /// </summary>
        public BatchTimingMTFDto BatchTimingMTF { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated batch timing for Saturday.
        /// </summary>
        public Guid BatchTimingSaturdayId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated batch timing for Saturday.
        /// </summary>
        public BatchTimingSaturdayDto BatchTimingSaturday { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated batch timing for Sunday.
        /// </summary>
        public Guid BatchTimingSundayId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated batch timing for Sunday.
        /// </summary>
        public BatchTimingSundayDto BatchTimingSunday { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the preferred batch time selection.
        /// </summary>
        public Guid PreferredBatchTime { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the preferred batch time selection.
        /// </summary>
        public PreferredBatchDto PreferredBatch { get; set; } = null!;

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

        /// <summary>
        /// Gets or sets the identifier of the academic year id.
        /// </summary>
        public Guid AcademicYearId { get; set; }
    }
}
