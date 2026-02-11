namespace Winfocus.LMS.Application.Services
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// CourseService.
    /// </summary>
    public sealed class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        public CourseService(ICourseRepository repo)
        {
            _repo = repo;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>
        /// CourseDto.
        /// </returns>
        public async Task<IReadOnlyList<CourseDto>> GetAllAsync()
            => (await _repo.GetAllAsync()).Select(Map).ToList();

        /// <summary>
        /// Gets course by identifier.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>
        /// CourseDto.
        /// </returns>
        public async Task<CourseDto?> GetByIdAsync(Guid id)
            => (await _repo.GetByIdAsync(id)) is { } c ? Map(c) : null;

        /// <summary>
        /// Gets courses by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// List of CourseDto.
        /// </returns>
        public async Task<IReadOnlyList<CourseDto>> GetByStreamAsync(Guid streamId)
            => (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();

        /// <summary>
        /// Gets courses by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>
        /// List of CourseDto.
        /// </returns>
        public async Task<IReadOnlyList<CourseDto>> GetBySubjectAsync(Guid subjectId)
            => (await _repo.GetBySubjectAsync(subjectId)).Select(Map).ToList();

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The course request.</param>
        /// <returns>
        /// Created CourseDto.
        /// </returns>
        public async Task<CourseDto> CreateAsync(CourseRequest request)
        {
            var course = new Course
            {
                CourseName = request.coursename,
                CourseCode = request.coursecode,
                CourseSubjects = request.subjectids
                    .Distinct()
                    .Select(id => new CourseSubject { SubjectId = id })
                    .ToList(),
                CreatedAt = DateTime.UtcNow,
            };

            return Map(await _repo.AddAsync(course));
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <param name="request">The course request.</param>
        /// <exception cref="KeyNotFoundException">Course not found.</exception>
        /// <returns>
        /// Updated CourseDto.
        /// </returns>
        public async Task UpdateAsync(Guid id, CourseRequest request)
        {
            var course = await _repo.GetByIdWithSubjectsAsync(id)
                ?? throw new KeyNotFoundException("Course not found");

            course.CourseName = request.coursename;
            course.CourseCode = request.coursecode;
            course.UpdatedAt = DateTime.UtcNow;

            var requested = request.subjectids.Distinct().ToList();

            var remove = course.CourseSubjects
                .Where(cs => !requested.Contains(cs.SubjectId))
                .ToList();

            foreach (var r in remove)
            {
                course.CourseSubjects.Remove(r);
            }

            var existing = course.CourseSubjects.Select(cs => cs.SubjectId).ToList();

            foreach (var sid in requested.Where(x => !existing.Contains(x)))
            {
                course.CourseSubjects.Add(new CourseSubject
                {
                    CourseId = course.Id,
                    SubjectId = sid,
                });
            }

            await _repo.UpdateAsync(course);
        }

        /// <summary>
        /// Soft deletes a course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task DeleteAsync(Guid id)
            => await _repo.SoftDeleteAsync(id);

        private static CourseDto Map(Course c) => new ()
        {
            Id = c.Id,
            CourseName = c.CourseName,
            CourseCode = c.CourseCode,
            Subjects = c.CourseSubjects.Select(cs => new SubjectDto
            {
                Id = cs.Subject.Id,
                SubjectName = cs.Subject.SubjectName,
                SubjectCode = cs.Subject.SubjectCode,
            }).ToList(),
        };
    }
}
