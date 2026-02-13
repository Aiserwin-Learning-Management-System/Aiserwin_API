namespace Winfocus.LMS.Application.Services
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// SubjectService.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.ISubjectService" />
    public sealed class SubjectService : ISubjectService
    {
        private readonly ISubjectRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public SubjectService(ISubjectRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets all active subjects.
        /// </summary>
        /// <returns>
        /// A list of subject DTOs.
        /// </returns>
        public async Task<IReadOnlyList<SubjectDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(Map).ToList();

        /// <summary>
        /// Gets a subject by identifier.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <returns>
        /// The subject DTO if found; otherwise, null.
        /// </returns>
        public async Task<SubjectDto?> GetByIdAsync(Guid id)
            => (await _repo.GetByIdAsync(id)) is { } s ? Map(s) : null;

        /// <summary>
        /// Gets subjects by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// A list of subject DTOs associated with the specified stream.
        /// </returns>
        public async Task<IReadOnlyList<SubjectDto>> GetByStreamAsync(Guid streamId)
            => (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();

        /// <summary>
        /// Gets the by course ids asynchronous.
        /// </summary>
        /// <param name="courseIds">The course ids.</param>
        /// <returns>Subject list.</returns>
        public async Task<IReadOnlyList<SubjectDto>> GetByCourseIdsAsync(List<Guid> courseIds)
        {
            var subjects = await _repo.GetByCourseIdsAsync(courseIds);
            return subjects.Select(s => new SubjectDto
            {
                Id = s.Id,
                SubjectName = s.SubjectName,
                SubjectCode = s.SubjectCode,
            }).ToList();
        }

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="request">The subject creation request.</param>
        /// <returns>
        /// The created subject DTO.
        /// </returns>
        public async Task<SubjectDto> CreateAsync(SubjectRequest request)
        {
            var subject = new Subject
            {
                SubjectName = request.subjectname,
                SubjectCode = request.subjectcode,
                CreatedAt = DateTime.UtcNow,
            };

            return Map(await _repo.AddAsync(subject));
        }

        /// <summary>
        /// Updates an existing subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="request">The subject update request.</param>
        /// <exception cref="KeyNotFoundException">Subject not found.</exception>
        /// <returns>
        /// A task that represents the asynchronous update operation.
        /// </returns>
        public async Task UpdateAsync(Guid id, SubjectRequest request)
        {
            var subject = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Subject not found");

            subject.SubjectName = request.subjectname;
            subject.SubjectCode = request.subjectcode;
            subject.UpdatedAt = DateTime.UtcNow;

            await _repo.UpdateAsync(subject);
        }

        /// <summary>
        /// Soft deletes a subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <returns>
        /// A task that represents the asynchronous delete operation.
        /// </returns>
        public async Task DeleteAsync(Guid id)
            => await _repo.SoftDeleteAsync(id);

        private static SubjectDto Map(Subject s) => new ()
        {
            Id = s.Id,
            SubjectName = s.SubjectName,
            SubjectCode = s.SubjectCode,
        };
    }
}
