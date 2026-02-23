namespace Winfocus.LMS.Application.Services
{
    using System.Text.RegularExpressions;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Generates unique, sequential usernames for students using their first name
    /// and the current registration year.
    /// </summary>
    public sealed class UsernameGeneratorService : IUsernameGeneratorService
    {
        private readonly IUserRepository _userRepository;

        private readonly ILogger<UsernameGeneratorService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="UsernameGeneratorService"/> class.
        /// </summary>
        /// <param name="userRepository">Repository for user data access.</param>
        /// <param name="logger">Structured logger.</param>
        public UsernameGeneratorService(
            IUserRepository userRepository,
            ILogger<UsernameGeneratorService> logger)
        {
            _userRepository = userRepository;
            _logger = logger;
        }

        /// <summary>
        /// Generates a unique username by combining the student's first name,
        /// the year's last 2 digits, and a sequential count of existing users
        /// who share the same prefix in that year.
        /// </summary>
        /// <param name="fullName">The student's full name. Must not be null or empty.</param>
        /// <param name="registrationYear">
        /// Optional registration year. Defaults to <see cref="DateTime.UtcNow.Year"/> if null.
        /// </param>
        /// <returns>
        /// A unique username string in the format <c>{firstname}_{YY}{NN}</c>.
        /// </returns>
        /// <exception cref="ArgumentException">
        /// Thrown if <paramref name="fullName"/> is null, empty, or contains no valid letters.
        /// </exception>
        public async Task<string> GenerateAsync(string fullName, int? registrationYear = null)
        {
            try
            {
                _logger.LogInformation(
                    "Generating username for FullName='{FullName}', Year={Year}",
                    fullName,
                    registrationYear);

                if (string.IsNullOrWhiteSpace(fullName))
                {
                    _logger.LogWarning("Username generation failed: FullName is null or empty.");
                    throw new ArgumentException("Full name must not be null or empty.", nameof(fullName));
                }

                var firstName = fullName
                    .Trim()
                    .Split(' ', StringSplitOptions.RemoveEmptyEntries)
                    .FirstOrDefault() ?? string.Empty;

                var normalizedFirstName = Regex.Replace(firstName, @"[^a-zA-Z]", "").ToLowerInvariant();

                if (string.IsNullOrEmpty(normalizedFirstName))
                {
                    _logger.LogWarning(
                        "Username generation failed: no valid letters found in first name '{FirstName}'.",
                        firstName);
                    throw new ArgumentException(
                        $"Could not extract a valid first name from '{fullName}'.",
                        nameof(fullName));
                }

                _logger.LogDebug(
                    "Normalized first name: '{NormalizedFirstName}'",
                    normalizedFirstName);

                var year = registrationYear ?? DateTime.UtcNow.Year;
                var yearSuffix = (year % 100).ToString("D2");

                _logger.LogDebug(
                    "Registration year: {Year}, yearSuffix: '{YearSuffix}'",
                    year, 
                    yearSuffix);

                var prefix = $"{normalizedFirstName}_{yearSuffix}";
                var existingCount = await _userRepository
                    .CountUsernamesByPrefixAndYearAsync(normalizedFirstName, year);

                _logger.LogDebug(
                    "Existing users with prefix '{Prefix}': {Count}",
                    prefix,
                    existingCount);

                var sequence = (existingCount + 1).ToString("D2");

                var username = $"{normalizedFirstName}_{yearSuffix}{sequence}";

                _logger.LogInformation(
                    "Generated username '{Username}' for FullName='{FullName}' (Year={Year}, Sequence={Sequence})", 
                    username,
                    fullName,
                    year,
                    sequence);

                return username;
            }
            catch (ArgumentException)
            {
                throw;
            }
            catch (Exception ex)
            {
                _logger.LogError(
                    ex,
                    "Unexpected error generating username for FullName='{FullName}'",
                    fullName);
                throw;
            }
        }
    }
}
