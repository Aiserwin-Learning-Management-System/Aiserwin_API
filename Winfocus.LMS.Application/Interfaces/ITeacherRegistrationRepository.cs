using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="TeacherRegistration"/> entities.
    /// </summary>
    public interface ITeacherRegistrationRepository
    {
        /// <summary>
        /// Adds a new <see cref="TeacherRegistration"/> to the data store.
        /// </summary>
        /// <param name="entity">The teacher registration entity to add.</param>
        /// <returns>.</returns>
        Task<TeacherRegistration> AddAsync(TeacherRegistration entity);

        /// <summary>
        /// Gets a teacher registration by identifier.
        /// </summary>
        /// <param name="id">The teacher registration identifier.</param>
        /// <returns>The matching when not found.</returns>
        Task<TeacherRegistration?> GetByIdAsync(Guid id);

        /// <summary>
        /// Gets all teacher registrations.
        /// </summary>
        /// <returns>Read-only list of entities.</returns>
        Task<IReadOnlyList<TeacherRegistration>> GetAllAsync();

        /// <summary>
        /// Updates an existing <see cref="TeacherRegistration"/> in the data store.
        /// </summary>
        /// <param name="entity">The teacher registration entity with updated values.</param>
        /// <returns>The updated entity, when the entity does not exist.</returns>
        Task<TeacherRegistration?> UpdateAsync(TeacherRegistration entity);

        /// <summary>
        /// Marks the entity as deleted but does not physically remove it from the database.
        /// </summary>
        /// <param name="id">The identifier of the teacher registration to soft delete.</param>
        /// <returns>.</returns>
        Task<bool> SoftDeleteAsync(Guid id);
    }
}
