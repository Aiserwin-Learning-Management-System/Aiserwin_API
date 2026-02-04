using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class ModeOfStudy : BaseEntity
    {
        [Required(ErrorMessage = "Mode of Study is required")]
        public string ModeName { get; set; }
        public Guid CountryId { get; set; }
        public Country Country { get; set; }
    }
}
