using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Stream : BaseEntity
    {
        [Required(ErrorMessage = "Stream Name is required")]
        public string StreamName { get; set; }
        public string StreamCode { get; set; }
        public Guid GradeId  { get; set; }
        public Grade Grade { get; set; }
    }
}
