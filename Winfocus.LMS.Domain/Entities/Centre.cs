using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Centre : BaseEntity
    {
        public string Name { get; set; } = null!; 
        public string Type { get; set; } = "Offline"; // Offline/Online/Hybrid
        public Guid CountryId { get; set; } 
        public Country Country { get; set; } = null!; }
    }
