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
        public async Task<CommonResponse<List<GradeDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Grdaes");
            var grades = await _repository.GetAllAsync();
            var mapped = grades.Select(Map).ToList();
            if (mapped.Any())
            {
                return CommonResponse<List<GradeDto>>.SuccessResponse("Fetching all Grades", mapped);
            }
            else
            {
                return CommonResponse<List<GradeDto>>.FailureResponse("Grades not found");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<GradeDto>> GetByIdAsync(Guid id)
        {
            var grades = await _repository.GetByIdAsync(id);
            var mapped = grades == null ? null : Map(grades);
            if (mapped != null)
            {
                return CommonResponse<GradeDto>.SuccessResponse("Fetching the grade", mapped);
            }
            else
            {
                return CommonResponse<GradeDto>.FailureResponse("batch timing not found");
            }
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
        public async Task<GradeDto> UpdateAsync(Guid id, GradeRequest request)
        {
            var grade = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Grade not found");

            grade.Name = request.name;
            grade.UpdatedBy = request.userId;
            grade.UpdatedAt = DateTime.UtcNow;

            return Map(await _repository.UpdateAsync(grade));
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
           return await _repository.DeleteAsync(id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="syllabusid">The identifier.</param>
        /// <returns>GradeDto.</returns>
        public async Task<CommonResponse<List<GradeDto>>> GetBySyllabusIdAsync(Guid syllabusid)
        {
            var grades = await _repository.GetBySyllabusIdAsync(syllabusid);
            var mapped = Map(grades);
            if (mapped != null)
            {
                return CommonResponse<List<GradeDto>>.SuccessResponse("Fetching the grade by syllabus", mapped);
            }
            else
            {
                return CommonResponse<List<GradeDto>>.FailureResponse("grade not found");
            }
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
