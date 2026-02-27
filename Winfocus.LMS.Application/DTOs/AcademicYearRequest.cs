using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs
{
    public sealed record AcademicYearRequest(string name,
     DateTime startdate, DateTime enddate, Guid userid);
}
