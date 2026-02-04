using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Center : BaseEntity
    {
        [Required(ErrorMessage = "Center Name is required")]
        public string CenterName { get; set; }
        public string CenterCode { get; set; }
        public Guid CoutryId { get; set; }
        public virtual Country Country { get; set; }

    }
}
