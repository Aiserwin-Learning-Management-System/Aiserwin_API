using System.ComponentModel.DataAnnotations;

namespace Winfocus.LMS.Application.DTOs.Students
{
    public sealed record StudentAcademicdetailsRequest([Required] Guid countryId, [Required] Guid stateId, [Required] Guid modeOfStudyId, [Required] Guid centerId,
[Required] Guid syllabusId, [Required] Guid gradeId, [Required] Guid streamId, [Required] List<Guid> courseId, [Required] Guid subjectId, [Required] List<Guid> batchTimingMTFIds,
[Required] List<Guid> batchTimingstaurdayIds, [Required] List<Guid> batchTimingSundayIds, Guid batchId, string preferredtime, string pastyearperformance, string pastschoolname, string pastschoollocation, string emirates, Guid userid);
}
