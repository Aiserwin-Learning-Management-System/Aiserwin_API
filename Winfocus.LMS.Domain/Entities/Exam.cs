namespace Winfocus.LMS.Domain.Entities
{
    using System;
    using System.Collections.Generic;
    using Winfocus.LMS.Domain.Common;
    using Winfocus.LMS.Domain.Enums;
{
    /// <summary>
    /// Represents a Exam in the system.
    /// </summary>
    public class Exam : BaseEntity
    {
        /// <summary>
        /// Gets or sets the identifier of the country where the centre is located.
        /// </summary>
        public Guid CountryId { get; set; }

        /// <summary>
        /// Gets or sets the country.
        /// </summary>
        public Country Country { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the modeofstudy.
        /// </summary>
        public Guid ModeOfStudyId { get; set; }

        /// <summary>
        /// Gets or sets the modeOfStudy.
        /// </summary>
        public ModeOfStudy ModeOfStudy { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the state.
        /// </summary>
        public Guid StateId { get; set; }

        /// <summary>
        /// Gets or sets the state.
        /// </summary>
        public State State { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated Center.
        /// </summary>
        public Guid CenterId { get; set; }

        /// <summary>
        /// Gets or sets the identifier of the associated Center.
        /// </summary>
        public Center Center { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated syllabus.
        /// </summary>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the associated syllabus.
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
        /// Gets or sets the identifier of the associated courses.
        /// </summary>
        public Guid CourseId { get; set; }

        /// <summary>
        /// Gets or sets the associated courses.
        /// </summary>
        public Course Course { get; set; } = null!;

        /// <summary>
        /// Gets or sets the identifier of the associated subject.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the associated subject.
        /// </summary>
        public Subject Subject { get; set; } = null!;

        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public ExamUnit Unit { get; set; } = default!;

        /// <summary>
        /// Gets or sets the foreign key to the exam chapter.
        /// </summary>
        public Guid ChapterId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the exam chapter.
        /// </summary>
        public ExamChapter Chapter { get; set; } = default!;

        /// <summary>
        /// Gets or sets the foreign key to the QuestionType.
        /// </summary>
        public Guid ResourceTypeId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the QuestionType.
        /// </summary>
        public ContentResourceType ResourceType { get; set; } = default!;

        /// <summary>
        /// Gets or sets the foreign key to the QuestionType.
        /// </summary>
        public Guid QuestionTypeId { get; set; }

        /// <summary>
        /// Gets or sets the navigation property to the QuestionType.
        /// </summary>
        public QuestionTypeConfig QuestionType { get; set; } = default!;

        /// <summary>
        /// Gets or sets an ExamTitle.
        /// </summary>
        public string? ExamTitle { get; set; }

        /// <summary>
        /// Gets or sets an Exam QuestionNumber.
        /// </summary>
        public string? ExamQuestionNumber { get; set; }

        /// <summary>
        /// Gets or sets an ExamDate.
        /// </summary>
        public DateTime ExamDate { get; set; }

        /// <summary>
        /// Gets or sets an Exam Duration.
        /// </summary>
        public string? ExamDuration { get; set; }

        /// <summary>
        /// Gets or sets an total mark.
        /// </summary>
        public double TotalMark { get; set; }

        /// <summary>
        /// Gets or sets an passing mark.
        /// </summary>
        public double PassingMark { get; set; }

        /// <summary>
        /// Gets or sets the current status of the exam.
        /// </summary>
        public ExamStatus Status { get; set; }
    }
}
