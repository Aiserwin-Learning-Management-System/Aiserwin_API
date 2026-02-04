using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class State : BaseClass
    {
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public Guid CoutryId { get; set; }
       // [ForeignKey("CoutryId")]
       // public Countries Country { get; set; }
    }
}
