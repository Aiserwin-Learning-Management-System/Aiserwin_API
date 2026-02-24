namespace Winfocus.LMS.Application.Services
{
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// GradeService.
    /// </summary>
    public sealed class GradeService : IGradeService
    {
        private readonly IGradeRepository _repository;
        private readonly ILogger<GradeService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="GradeService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        /// <param name="logger">The logger.</param>
        public GradeService(
            IGradeRepository repository,
            ILogger<GradeService> logger)
        {
            _repository = repository;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>GradeDto.</returns>
        public async Task<IReadOnlyList<GradeDto>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all syllabuses");
            var grades = await _repository.GetAllAsync();
            return grades.Select(Map).ToList();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<GradeDto?> GetByIdAsync(Guid id)
        {
            var grades = await _repository.GetByIdAsync(id);
            return grades == null ? null : Map(grades);
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>GradeDto.</returns>
        /// <exception cref="InvalidOperationException">grade code already exists.</exception>
        public async Task<GradeDto> CreateAsync(GradeRequest request)
        {
            var grades = new Grade
            {
                Name = request.name,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = request.userId,
                SyllabusId = request.syllabusid,
            };

            var created = await _repository.AddAsync(grades);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Grade not found.</exception>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Guid id, GradeRequest request)
        {
            var grade = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Grade not found");

            grade.Name = request.name;
            grade.UpdatedBy = request.userId;
            grade.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(grade);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task DeleteAsync(Guid id)
        {
            await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<List<GradeDto>> GetBySyllabusIdAsync(Guid syllabusid)
        {
            var grades = await _repository.GetBySyllabusIdAsync(syllabusid);
            return Map(grades);
        }

        private static List<GradeDto> Map(IEnumerable<Grade> grades)
        {
            return grades.Select(Map).ToList();
        }

        private static GradeDto Map(Grade c) =>
     new GradeDto
     {
         Id = c.Id,
         Name = c.Name,
         SyllabusId = c.SyllabusId,
         UpdatedBy = c.UpdatedBy,
         UpdatedAt = c.UpdatedAt,
         CreatedBy = c.CreatedBy,
         CreatedAt = c.CreatedAt,
         Syllabus = c.Syllabus == null ? null : new SyllabusDto
         {
             Id = c.Syllabus.Id,
             Name = c.Syllabus.Name,
         },
     };
}
}
