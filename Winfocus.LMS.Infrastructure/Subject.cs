using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Subject : BaseClass
    {
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public Guid CourseId { get; set; }
        [ForeignKey("CourseId")]
        public Course Course { get; set; }
    }
}
