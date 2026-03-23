using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// ExamUnitDto.
    /// </summary>
    public class ExamUnitDto : BaseClassDTO
    {
        /// <summary>
        ///  Gets or sets the identifier.
        /// </summary>
        public Guid SubjectId { get; set; }

        /// <summary>
        ///  Gets or sets the Name.
        /// </summary>
        public string Name { get; set; } = null!;

        /// <summary>
        ///  Gets or sets the UnitNumber.
        /// </summary>
        public int UnitNumber { get; set; }

        /// <summary>
        ///  Gets or sets the Description.
        /// </summary>
        public string? Description { get; set; }

        /// <summary>
        ///  Gets or sets the Subject.
        /// </summary>
        public ExamSubjectDto? Subject { get; set; }

        /// <summary>
        /// Gets or sets the grade name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string GradeName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the subject name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string SubjectName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the syllabus name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string SyllabusName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the sayllabus identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid GradeId { get; set; }
    }
}
