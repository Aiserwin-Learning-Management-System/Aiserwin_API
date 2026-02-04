using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Grade : BaseEntity
    {
        [Required(ErrorMessage = "Grade Name is required")]
        public string GradeName { get; set; }
        public string GradeCode { get; set; }
        public Guid SyllabusId { get; set; }
        public Syllabus Syllabus { get; set; }
    }
}
