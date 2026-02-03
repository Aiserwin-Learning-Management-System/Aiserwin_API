using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class Country : BaseEntity
    {
        public string Name { get; set; } = null!; 
        public string Code { get; set; } = null!; 
        public ICollection<Centre> Centres { get; set; } = new List<Centre>(); }
    }
