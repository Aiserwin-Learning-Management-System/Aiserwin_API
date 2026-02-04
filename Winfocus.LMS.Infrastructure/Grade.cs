using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Grade : BaseClass
    {
        public string GradeName { get; set; }
        public string GradeCode { get; set; }
        public Guid SyllabusId { get; set; }
        [ForeignKey("SyllabusId")]
        public Syllabus Syllabus { get; set; }
    }
}
