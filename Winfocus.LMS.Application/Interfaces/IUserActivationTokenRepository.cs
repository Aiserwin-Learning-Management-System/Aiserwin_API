namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// IUserActivationTokenRepository.
    /// </summary>
    public interface IUserActivationTokenRepository
    {
        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task AddAsync(UserActivationToken token);

        /// <summary>
        /// Gets the by token asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>UserActivationToken.</returns>
        Task<UserActivationToken?> GetByTokenAsync(string token);

        /// <summary>
        /// Invalidates the user tokens asynchronous.
        /// </summary>
        /// <param name="userId">The user identifier.</param>
        /// <param name="purpose">The purpose.</param>
        /// <returns>Task.</returns>
        Task InvalidateUserTokensAsync(Guid userId, TokenPurpose purpose);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="token">The token.</param>
        /// <returns>Task.</returns>
        Task UpdateAsync(UserActivationToken token);
    }
}
