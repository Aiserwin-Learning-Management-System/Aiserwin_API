using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Course :BaseClass
    {
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public Guid StreamId { get; set; }
        [ForeignKey("StreamId")]
        public virtual Stream Stream { get; set; }
    }
}
