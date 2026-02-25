namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// Course service contract.
    /// </summary>
    public interface ICourseService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>CourseDto.</returns>
        Task<CommonResponse<List<CourseDto>>> GetAllAsync();

        /// <summary>
        /// Gets course by identifier.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>CourseDto.</returns>
        Task<CommonResponse<CourseDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets courses by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>List of CourseDto.</returns>
        Task<CommonResponse<List<CourseDto>>> GetByStreamAsync(Guid streamId);

        /// <summary>
        /// Gets courses by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>List of CourseDto.</returns>
        Task<CommonResponse<List<CourseDto>>> GetBySubjectAsync(Guid subjectId);

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The course request.</param>
        /// <returns>Created CourseDto.</returns>
        Task<CommonResponse<CourseDto>> CreateAsync(CourseRequest request);

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <param name="request">The course request.</param>
        /// <returns>Task.</returns>
        Task<CommonResponse<CourseDto>> UpdateAsync(Guid id, CourseRequest request);

        /// <summary>
        /// Soft deletes a course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>Task.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered courses with pagination support.
        /// </summary>
        /// <param name="centreId">Centre identifier.</param>
        /// <param name="syllabusId">Syllabus identifier.</param>
        /// <param name="gradeId">Grade identifier.</param>
        /// <param name="streamId">Stream identifier.</param>
        /// <param name="subjectsId">Subject identifier.</param>
        /// <param name="startDate">Filter courses created after this date.</param>
        /// <param name="endDate">Filter courses created before this date.</param>
        /// <param name="active">Filter by active status.</param>
        /// <param name="searchText">Search keyword.</param>
        /// <param name="limit">Number of records to return.</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order (asc or desc).</param>
        /// <returns>Paginated course result.</returns>
        Task<CommonResponse<PagedResult<CourseDto>>> GetFilteredAsync(
            Guid? centreId,
            Guid? syllabusId,
            Guid? gradeId,
            Guid? streamId,
            Guid? subjectsId,
            DateTime? startDate,
            DateTime? endDate,
            bool? active,
            string? searchText,
            int limit,
            int offset,
            string sortOrder);
    }
}
