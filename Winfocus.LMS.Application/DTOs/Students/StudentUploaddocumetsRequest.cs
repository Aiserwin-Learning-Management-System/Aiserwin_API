using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Text;

namespace Winfocus.LMS.Application.DTOs.Students
{
    public sealed record StudentUploaddocumentsRequest(

     [Required]
     IFormFile studentphoto,

     [Required]
     IFormFile signature,

     bool isAcceptedAgreement,
     bool isAcceptedTermsAndConditions);
}
