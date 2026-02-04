using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class StudentAcademicDetails : BaseClass
    {
        public Guid CountryId { get; set; }
        //[ForeignKey("CountryId")]
        //public virtual Countries Country { get; set; }
        public Guid ModeOfStudyId { get; set; }
        [ForeignKey("ModeOfStudyId")]
        public virtual ModeOfStudy ModeOfStudy { get; set; }
        public Guid StateId { get; set; }
        [ForeignKey("StateId")]
        public State State { get; set; }
        public Guid CenterId { get; set; }
        [ForeignKey("CenterId")]
        public virtual Center Center { get; set; }
        public Guid SyllabusId { get; set; }
        [ForeignKey("SyllabusId")]
        public virtual Syllabus Syllabus { get; set; }
        public Guid GradeId { get; set; }
        [ForeignKey("GradeId")]
        public virtual Grade Grade { get; set; }
        public Guid StreamId { get; set; }
        [ForeignKey("StreamId")]
        public virtual Stream Stream { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public virtual Course Course { get; set; }
        public Guid SubjectId { get; set; }
        [ForeignKey("SubjectId")]
        public virtual Subject Subject { get; set; }
        //public Guid BatchId { get; set; }
        //[ForeignKey("BatchId")]
        //public virtual Batch Batch { get; set; }
        public Guid BatchTimingMTFId { get; set; }
        [ForeignKey("BatchTimingMTFId")]
        public virtual BatchTimingMTF BatchTimingMTF { get; set; }
        public Guid BatchTimingSaturdayId { get; set; }
        [ForeignKey("BatchTimingSaturdayId")]
        public virtual BatchTimingSaturday BatchTimingSaturday { get; set; }
        public Guid BatchTimingSundayId { get; set; }
        [ForeignKey("BatchTimingSundayId")]
        public virtual BatchTimingSunday BatchTimingSunday { get; set; }
        public Guid PreferredBatchId { get; set; }
        [ForeignKey("PreferredBatchId")]
        public virtual PreferredBatch GetPreferredBatch { get; set; }
        public string PastYearPerformance { get; set; }
        public string PastSchoolName { get; set; }
        public string PastSchoolLocation { get; set; }
        public string Emirates { get; set; }
    }
}
