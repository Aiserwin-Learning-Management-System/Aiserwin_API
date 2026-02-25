namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// IFeeRepository – contract for fee-related data access.
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
        /// Gets the student fee selection by identifier asynchronous.
        /// </summary>
        /// <param name="selectionId">The selection identifier.</param>
        /// <returns>Task&lt;StudentFeeSelection?&gt;.</returns>
        Task<StudentFeeSelection?> GetStudentFeeSelectionByIdAsync(Guid selectionId);

        /// <summary>
        /// Gets all student fee selections for a student.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;List&lt;StudentFeeSelection&gt;&gt;.</returns>
        Task<List<StudentFeeSelection>> GetStudentFeeSelectionsByStudentAsync(Guid studentId);

        /// <summary>
        /// Gets the fee plan by identifier asynchronous.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task&lt;FeePlan?&gt;.</returns>
        Task<FeePlan?> GetFeePlanByIdAsync(Guid feePlanId);

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        Task SaveChangesAsync();
    }
}
