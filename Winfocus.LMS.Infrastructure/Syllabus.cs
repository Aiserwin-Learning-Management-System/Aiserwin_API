using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Syllabus : BaseClass
    {
        public string SyllabusName { get; set; }
        public string SyllabusCode { get; set; }
        public Guid CenterId { get; set; }
        [ForeignKey("CenterId")]
        public Center Center { get; set; }
    }
}
