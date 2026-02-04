using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class State : BaseEntity
    {
        [Required(ErrorMessage = "State Name is required")]
        public string StateName { get; set; }
        public string StateCode { get; set; }
        public Guid CoutryId { get; set; }
        public Country Country { get; set; }
    }
}
