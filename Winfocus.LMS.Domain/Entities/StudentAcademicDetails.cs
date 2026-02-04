using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class StudentAcademicDetails : BaseEntity
    {
        public Guid CountryId { get; set; }
        public virtual Country Country { get; set; }
        public Guid ModeOfStudyId { get; set; }
        public virtual ModeOfStudy ModeOfStudy { get; set; }
        public Guid StateId { get; set; }
        public State State { get; set; }
        public Guid CenterId { get; set; }
        public virtual Center Center { get; set; }
        public Guid SyllabusId { get; set; }
        public virtual Syllabus Syllabus { get; set; }
        public Guid GradeId { get; set; }
        public virtual Grade Grade { get; set; }
        public Guid StreamId { get; set; }
        public virtual Stream Stream { get; set; }
        public Guid CourseId { get; set; }
        public virtual Course Course { get; set; }
        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
        //public Guid BatchId { get; set; }
        //public virtual Batch Batch { get; set; }
        public Guid BatchTimingMTFId { get; set; }
        public virtual BatchTimingMTF BatchTimingMTF { get; set; }
        public Guid BatchTimingSaturdayId { get; set; }
        public virtual BatchTimingSaturday BatchTimingSaturday { get; set; }
        public Guid BatchTimingSundayId { get; set; }
        public virtual BatchTimingSunday BatchTimingSunday { get; set; }
        public Guid PreferredBatchTime { get; set; }
        public virtual PreferredBatch GetPreferredBatch { get; set; }
        public string PastYearPerformance {  get; set; }
        public string PastSchoolName { get; set; }
        public string PastSchoolLocation { get; set; }
        public string Emirates { get; set; }
    }
}
