namespace Winfocus.LMS.Application.DTOs.Students
{
    using System;
    using System.Collections.Generic;
    using System.ComponentModel.DataAnnotations;
    using System.Text;
    using Winfocus.LMS.Domain.Enums;

    public sealed record StudentPersonaldetailsRequest(

        [Required]
        [MaxLength(150)]
        [MinLength(3)]
        string fullname,

        [Required]
        [EmailAddress]
        [MaxLength(150)]
        string emailaddress,

        [Required]
        DateTime dob,

        [Required]
        [Phone]
        [MaxLength(20)]
        string mobilewhatsapp,

        [MaxLength(20)]
        string mobilebotim,

        [MaxLength(20)]
        string mobilecomera,

        [Required]
        [MaxLength(100)]
        string areaname,

        [Required]
        [MaxLength(100)]
        string districtorlocation,

        [Required]
        [MaxLength(50)]
        string emirates,

        [Required]
        Gender gender
    );
}
