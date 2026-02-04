using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class Center : BaseClass
    {
        public string CenterName { get; set; }
        public string CenterCode { get; set; }
        public Guid StateId { get; set; }
        [ForeignKey("StateId")]
        public virtual State State { get; set; }
    }
}
