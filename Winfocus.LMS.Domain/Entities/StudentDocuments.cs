using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class StudentDocuments : BaseEntity
    {
        public string StudentPhoto { get; set; }
        public string StudentSignature { get; set; }
    }
}
