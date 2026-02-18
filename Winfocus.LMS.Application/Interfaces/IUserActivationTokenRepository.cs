namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

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
        /// <returns>Task.</returns>
        Task InvalidateUserTokensAsync(Guid userId);
    }
}
