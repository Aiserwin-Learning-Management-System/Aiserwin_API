namespace Winfocus.LMS.Application.Common.Helpers
{
    /// <summary>
    /// ValidationHelper.
    /// </summary>
    public static class ValidationHelper
    {
        /// <summary>
        /// Adds the error.
        /// </summary>
        /// <param name="errors">The errors.</param>
        /// <param name="key">The key.</param>
        /// <param name="message">The message.</param>
        public static void AddError(
            IDictionary<string, string[]> errors,
            string key,
            string message)
        {
            if (errors.ContainsKey(key))
            {
                var existing = errors[key].ToList();
                existing.Add(message);
                errors[key] = existing.ToArray();
            }
            else
            {
                errors[key] = new[] { message };
            }
        }
    }
}
