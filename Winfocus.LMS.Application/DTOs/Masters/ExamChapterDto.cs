using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs.Masters
{
    /// <summary>
    /// Represents a chapter under a unit.
    /// Example: "Electric Charges and Fields" under Electrostatics.
    /// Bottom level of the 5-level exam hierarchy.
    /// </summary>
    public class ExamChapterDto : BaseClassDTO
    {
        /// <summary>
        /// Gets or sets the unit identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid UnitId { get; set; }

        /// <summary>
        /// Gets or sets the name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string Name { get; set; } = default!;

        /// <summary>
        /// Gets or sets the chapter number.
        /// </summary>
        /// <value>
        /// The chapter number.
        /// </value>
        public int ChapterNumber { get; set; }

        /// <summary>
        /// Gets or sets the description.
        /// </summary>
        /// <value>
        /// The description.
        /// </value>
        public string? Description { get; set; }

        /// <summary>
        /// Gets or sets the unit.
        /// </summary>
        /// <value>
        /// The unit.
        /// </value>
        public ExamUnitDto Unit { get; set; }

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
        /// Gets or sets the unit name.
        /// </summary>
        /// <value>
        /// The name.
        /// </value>
        public string UnitName { get; set; } = default!;

        /// <summary>
        /// Gets or sets the sayllabus identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid SyllabusId { get; set; }

        /// <summary>
        /// Gets or sets the subject identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid SubjectId { get; set; }

        /// <summary>
        /// Gets or sets the grade identifier.
        /// </summary>
        /// <value>
        /// The unit identifier.
        /// </value>
        public Guid GradeId { get; set; }

    }
}
