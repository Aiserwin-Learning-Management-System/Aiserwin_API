namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines CRUD operations for <see cref="StudentAcademicDetails"/> entities.
    /// </summary>
    public interface IStudentAcademicdeatilsRepository
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
    }
}
