using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class PreferredBatch : BaseEntity
    {
        [Required(ErrorMessage = "Preferred Batch is required")]
        public string Name { get; set; }
        public Guid BatchId { get; set; }
        public virtual Batch Batch { get; set; }
    }
}
