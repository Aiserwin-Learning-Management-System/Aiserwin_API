using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Subject :BaseEntity
    {
        [Required(ErrorMessage = "Subject Name is required")]
        public string SubjectName { get; set; }
        public string SubjectCode { get; set; }
        public Guid CourseId { get; set; }
        public Course Course { get; set; }
    }
}
