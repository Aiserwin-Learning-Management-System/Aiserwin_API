using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Course : BaseEntity
    {
        [Required(ErrorMessage = "Course Name is required")]
        public string CourseName { get; set; }
        public string CourseCode { get; set; }
        public Guid StreamId { get; set; }
        public virtual Stream Stream { get; set; }
    }
}
