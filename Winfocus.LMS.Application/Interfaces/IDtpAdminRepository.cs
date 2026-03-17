namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// IDtpAdminRepository.
    /// </summary>
    public interface IDtpAdminRepository
    {
        /// <summary>
        /// Gets the active registration form for DTP staff category
        /// with all form fields and their options.
        /// </summary>
        /// <returns></returns>
        Task<RegistrationForm?> GetDtpRegistrationFormAsync();

        /// <summary>
        /// Gets DTP operator registrations with all values.
        /// </summary>
        /// <returns></returns>
        Task<List<StaffRegistration>> GetDtpRegistrationsAsync();

        /// <summary>
        /// Gets a single registration by ID with values.
        /// </summary>
        /// <param name="registrationId">The registration identifier.</param>
        /// <returns></returns>
        Task<StaffRegistration?> GetRegistrationByIdAsync(Guid registrationId);

        /// <summary>
        /// Gets the DTP staff category.
        /// </summary>
        /// <returns></returns>
        Task<StaffCategory?> GetDtpCategoryAsync();

        /// <summary>
        /// Saves changes to database.
        /// </summary>
        /// <returns></returns>
        Task SaveChangesAsync();
    }
}
