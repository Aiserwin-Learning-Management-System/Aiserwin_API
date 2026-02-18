namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines CRUD operations for <see cref="StudentAcademicDetails"/> entities.
    /// </summary>
    public interface IStudentAcademicdetailsRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentAcademicDetails.</returns>
        Task<IReadOnlyList<StudentAcademicDetails>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentAcademicDetails.</returns>
        Task<StudentAcademicDetails?> GetByIdAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentAcademicDetails">The StudentAcademicDetails.</param>
        /// <returns>studentAcademicDetails.</returns>
        Task<StudentAcademicDetails> AddAsync(StudentAcademicDetails studentAcademicDetails);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="studentAcademicDetails">The state.</param>
        /// <returns>studentAcademicDetails.</returns>
        Task UpdateAsync(StudentAcademicDetails studentAcademicDetails);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="studentDocuments">The StudentDocuments.</param>
        /// <returns>StudentDocuments.</returns>
        Task<StudentDocuments>  AddUploadedDocuments(StudentDocuments studentDocuments);

        /// <summary>
        /// Adds multiple student academic course mappings.
        /// </summary>
        /// <param name="courses">List of student academic course entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddCourseRangeAsync(List<StudentAcademicCouses> courses);

        /// <summary>
        /// Adds multiple student academic batchtimingmtf mappings.
        /// </summary>
        /// <param name="batchmtfs">List of student academic batchtiming mtfs entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBatchtimingmtfRangeAsync(List<StudentBatchTimingMTF> batchmtfs);

        /// <summary>
        /// Adds multiple student academic batchtiming saturdays mappings.
        /// </summary>
        /// <param name="batchsaturdays">List of student academic batchtiming saturdays entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBatchtimingsaturdayRangeAsync(List<StudentBatchTimingSaturday> batchsaturdays);

        /// <summary>
        /// Adds multiple student academic batchtiming sundays mappings.
        /// </summary>
        /// <param name="batchsundays">List of student academic batchtiming sundays entities.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBatchtimingsundayRangeAsync(List<StudentBatchTimingSunday> batchsundays);
    }
}
