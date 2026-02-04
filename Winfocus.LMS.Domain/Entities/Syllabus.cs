using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Syllabus : BaseEntity
    {
        [Required(ErrorMessage = "Syllabus Name is required")]
        public string SyllabusName { get; set; }
        public string SyllabusCode { get; set; }
        public Guid CenterId { get; set; }
        public Center Center { get; set; }
    }
}
