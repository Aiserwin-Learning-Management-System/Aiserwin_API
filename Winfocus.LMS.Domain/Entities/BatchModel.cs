using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Batch : BaseEntity
    {
        [Required(ErrorMessage = "Batch Name is required")] 
        public string BatchName { get; set; }
        public string BatchCode { get; set; }
        public Guid SubjectId { get; set; }
        public virtual Subject Subject { get; set; }
    }
}
