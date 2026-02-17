namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// IFeeRepository.
    /// </summary>
    public interface IFeeRepository
    {
        /// <summary>
        /// Gets the student with courses asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;Student?&gt;.</returns>
        Task<Student?> GetStudentWithCoursesAsync(Guid studentId);

        /// <summary>
        /// Gets the courses by grade asynchronous.
        /// </summary>
        /// <param name="gradeId">The grade identifier.</param>
        /// <returns>Task&lt;List&lt;Course&gt;&gt;.</returns>
        Task<List<Course>> GetCoursesByGradeAsync(Guid gradeId);

        /// <summary>
        /// Adds the student fee selection asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        Task AddStudentFeeSelectionAsync(StudentFeeSelection entity);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SaveChangesAsync();
    }
}
