using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Student : BaseEntity
    {
        public Guid StudentAcademicId { get; set; }
        [ForeignKey("StudentAcademicId")]
        public virtual StudentAcademicDetails AcademicDetails { get; set; }
        public Guid StudentPersonalId { get; set; }
        [ForeignKey("StudentPersonalId")]
        public virtual StudentPersonalDetails PersonalDetails { get; set; }
        public Guid StudentDocumentsId { get; set; }
        [ForeignKey("StudentDocumentsId")]
        public virtual StudentDocuments StudentDocuments { get; set; }

    }
}
