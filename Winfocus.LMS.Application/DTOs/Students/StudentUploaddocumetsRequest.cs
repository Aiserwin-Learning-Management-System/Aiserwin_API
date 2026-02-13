using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Students
{
    public sealed record StudentUploaddocumetsRequest(

     [Required]
     string studentphoto,

     [Required]
     string signature,

     bool isAcceptedTersms,
     bool isAcceptedAgreement);
}
