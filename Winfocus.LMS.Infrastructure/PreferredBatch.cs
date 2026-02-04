using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Winfocus.LMS.Infrastructure
{
    public class PreferredBatch :BaseClass
    {
        public string Name { get; set; }
        public Guid BatchId { get; set; }
        public virtual Batch Batch { get; set; }
    }
}
