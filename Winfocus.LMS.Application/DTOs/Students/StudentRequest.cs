using System;
using System.Collections.Generic;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Students
{
    public sealed record StudentRequest(StudentAcademicdetailsRequest academicdetails, StudentPersonaldetailsRequest personaldetails, StudentUploaddocumetsRequest docdetails);
}
