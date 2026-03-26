namespace Winfocus.LMS.Application.Interfaces
{
    using System;
    using System.Collections.Generic;
    using System.Text;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="BatchTimingMTF"/> entities.
    /// </summary>
    public interface IBatchTimingMTFRepository
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTF.</returns>
        Task<IReadOnlyList<BatchTimingMTF>> GetAllAsync(Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>BatchTimingMTF.</returns>
        Task<BatchTimingMTF?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>BatchTimingMTF.</returns>
        Task<BatchTimingMTF?> GetByIdCenterIdAsync(Guid id, Guid centerId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="state">The state.</param>
        /// <returns>BatchtimingMTF.</returns>
        Task<BatchTimingMTF> AddAsync(BatchTimingMTF state);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="batchtiming">The state.</param>
        /// <returns>BatchTimingMTF.</returns>
        Task<BatchTimingMTF> UpdateAsync(BatchTimingMTF batchtiming);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>task.</returns>
        Task<bool> DeleteAsync(Guid id, Guid centerId);

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>BatchTiming.</returns>
        Task<List<BatchTimingMTF>> GetBySubjectIdAsync(Guid subjectid);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="batchTimingMTF">The batchTimingMTF.</param>
        /// <returns>batchTimingMTF.</returns>
        Task<SubjectBatchTimingMTF> BatchTimingSubjectCreate(SubjectBatchTimingMTF batchTimingMTF);

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>batch timing.</returns>
        IQueryable<BatchTimingMTF> Query(Guid centerId);
    }
}
