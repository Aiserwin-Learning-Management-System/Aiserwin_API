namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// ISyllabusRepository.
    /// </summary>
    public interface ISyllabusRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Syllabus.</returns>
        Task<IReadOnlyList<Syllabus>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">centerId.</param>
        /// <returns>Syllabus.</returns>
        Task<Syllabus?> GetByIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>Syllabus.</returns>
        Task<Syllabus> AddAsync(Syllabus syllabus);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="syllabus">The syllabus.</param>
        /// <returns>Syllabus.</returns>
        Task<Syllabus> UpdateAsync(Syllabus syllabus);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Existses the by name asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>bool.</returns>
        Task<bool> ExistsByNameAsync(string name);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>syllabuses.</returns>
        IQueryable<Syllabus> Query(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="centerId">The identifier.</param>
        /// <returns>Grade.</returns>
        Task<List<Syllabus>> GetByCenterIdAsync(Guid centerId);
    }
}
