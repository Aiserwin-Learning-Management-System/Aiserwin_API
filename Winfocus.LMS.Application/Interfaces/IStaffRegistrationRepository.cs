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
    }
}
