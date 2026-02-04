using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Common;

namespace Winfocus.LMS.Domain.Entities
{
    public class StudentPersonalDetails : BaseEntity
    {
        public string FullName { get; set; }
        public string EmailAddress { get; set; }
        public DateTime DOB {  get; set; }  
        public string MobileWhatsapp { get; set; }
        public string MobileBotim { get; set; }
        public string MobileComera { get; set; }
        public string AreaName { get; set; }
        public string DistrictOrLocation { get; set; }
        public string Emirates { get; set; }
    }
}
