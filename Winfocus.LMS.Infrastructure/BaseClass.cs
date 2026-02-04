using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class BaseClass 
    {
        public Guid Id { get; set; }

        public DateTime CreatedAt { get; set; }
        public Guid CreatedBy { get; set; }

        public DateTime? UpdatedAt { get; set; }
        public Guid? UpdatedBy { get; set; }

        public bool IsActive { get; set; } = true;
    }
}
