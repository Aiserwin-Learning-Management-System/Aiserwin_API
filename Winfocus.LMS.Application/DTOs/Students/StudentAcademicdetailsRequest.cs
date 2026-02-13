namespace Winfocus.LMS.Application.DTOs.Students
{
    public sealed record StudentAcademicdetailsRequest(Guid countryId, Guid stateId, Guid modeOfStudyId, Guid centerId,
        Guid syllabusId, Guid gradeId, Guid streamId, List<Guid> courseId, Guid subjectId, List<Guid> batchTimingMTFIds,
        List<Guid> batchTimingstaurdayIds, List<Guid> batchTimingSundayIds);
}
