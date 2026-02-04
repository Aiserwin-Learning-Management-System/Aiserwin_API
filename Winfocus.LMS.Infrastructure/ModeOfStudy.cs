using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations.Schema;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class ModeOfStudy :BaseClass
    {
        public string ModeName { get; set; }
        public Guid CountryId { get; set; }
        //[ForeignKey("CountryId")]
        //public Countries Country { get; set; }
    }
}
