using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Student : BaseClass
    {
        public Guid StudentAcademicId { get; set; }
        public virtual StudentAcademicDetails AcademicDetails { get; set; }
        public Guid StudentPersonalId { get; set; }
        public virtual StudentPersonalDetails PersonalDetails { get; set; }
        public Guid StudentDocumentsId { get; set; }
        public virtual StudentDocuments StudentDocuments { get; set; }
        public ICollection<BatchTimingMTF> StudentBatchTimingMTF { get; set; }
        public ICollection<BatchTimingSaturday> StudentBatchTimingSaturday { get; set; }
        public ICollection<BatchTimingSunday> StudentBatchTimingSunday { get; set; }
    }
}
