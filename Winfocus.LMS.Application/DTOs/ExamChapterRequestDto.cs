using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.DTOs
{
    /// <summary>
    /// ExamChapterRequestDto.
    /// </summary>
    public sealed record class ExamChapterRequestDto(string name, string description, int chapterNumber, Guid unitId, Guid userid);
}
