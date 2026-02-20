namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// IUsernameGeneratorService.
    /// </summary>
    public interface IUsernameGeneratorService
    {
        /// <summary>
        /// Generates a unique username for a student based on their full name and the registration year.
        /// </summary>
        /// <param name="fullName">
        /// The student's full name (e.g., "Arjun Kumar"). The first word is used as the base.
        /// </param>
        /// <param name="registrationYear">
        /// The year of registration (e.g., 2026). Defaults to the current year if not provided.
        /// </param>
        /// <returns>
        /// A guaranteed-unique username string (e.g., <c>arjun_2601</c>).
        /// </returns>
        Task<string> GenerateAsync(string fullName, int? registrationYear = null);
    }
}
