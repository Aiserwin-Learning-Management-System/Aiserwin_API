namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Data access for staff registrations and related form lookups.
    /// </summary>
    public interface IStaffRegistrationRepository
    {
        /// <summary>
        /// Gets the form with fields asynchronous.
        /// </summary>
        /// <param name="formId">The form identifier.</param>
        /// <returns>
        /// The form with fields asynchronous.
        Task<RegistrationForm?> GetFormWithFieldsAsync(Guid formId);

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns>
        /// The asynchronous.
        Task<StaffRegistration> AddAsync(StaffRegistration registration);

        /// <summary>
        /// Gets the by identifier with details asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>
        /// The by identifier with details asynchronous.
        Task<StaffRegistration?> GetByIdWithDetailsAsync(Guid id);

        /// <summary>
        /// Queries this instance.
        /// </summary>
        /// <returns>
        /// The queryable registrations.
        IQueryable<StaffRegistration> Query();

        /// <summary>
        /// Updates the status asynchronous.
        /// </summary>
        /// <param name="registration">The registration.</param>
        /// <returns></returns>
        Task UpdateStatusAsync(StaffRegistration registration);

        /// <summary>
        /// Gets a staff registration by the linked user account identifier.
        /// Used when the logged-in operator wants their own data.
        /// </summary>
        /// <param name="userId">The user identifier from JWT.</param>
        /// <returns>The registration if found; otherwise null.</returns>
        Task<StaffRegistration?> GetByUserIdAsync(Guid userId);
    }
}
