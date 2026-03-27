namespace Winfocus.LMS.Application.Services
{
    using System.Collections.Generic;
    using System.Threading.Tasks;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs.DtpAdmin;
    using Winfocus.LMS.Application.Interfaces;

    /// <summary>
    /// Implementation of <see cref="ITeachersService"/>.
    /// </summary>
    public class TeachersService : ITeachersService
    {
        private readonly ITeachersRepository _repo;
        private readonly ILogger<TeachersService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeachersService"/> class.
        /// </summary>
        /// <param name="repo">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public TeachersService(ITeachersRepository repo, ILogger<TeachersService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <param name="category">The category.</param>
        /// <returns>list of TeachersByCategoryDto.</returns>
        public async Task<List<TeachersByCategoryDto>> GetTeachersByCategoryAsync(string? category = null)
        {
            _logger.LogDebug("Loading teachers by category: {Category}", category);
            var result = await _repo.GetTeachersByCategoryAsync(category);
            return result;
        }
    }
}
