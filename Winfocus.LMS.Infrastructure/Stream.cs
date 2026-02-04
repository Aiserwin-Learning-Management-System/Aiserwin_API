using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Stream : BaseClass
    {
        public string StreamName { get; set; }
        public string StreamCode { get; set; }
        public Guid GradeId { get; set; }
        [ForeignKey("GradeId")]
        public Grade Grade { get; set; }
    }
}
