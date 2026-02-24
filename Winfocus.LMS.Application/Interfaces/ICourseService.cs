namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;

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
    }
}
