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
                Name = request.coursename,
                SubjectId = request.subjectid,
                GradeId = request.gradeid,
                CourseDescription = request.cousedescription,
                CourseUrl = request.courseurl,
                MaxStudent = request.maxstudent,
                AcademicYear = request.academicyear,
                CreatedAt = DateTime.UtcNow,
                Status = request.status,
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
            var course = await _repo.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Course not found");

            course.Name = request.coursename;
            course.SubjectId = request.subjectid;
            course.GradeId = request.gradeid;
            course.CourseDescription = request.cousedescription;
            course.CourseUrl = request.courseurl;
            course.MaxStudent = request.maxstudent;
            course.AcademicYear = request.academicyear;
            course.UpdatedAt = DateTime.UtcNow;

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
            Name = c.Name,
            GradeId = c.GradeId,
            CourseDescription = c.CourseDescription,
            CourseUrl = c.CourseUrl,
            MaxStudent = c.MaxStudent,
            AcademicYear = c.AcademicYear,
            Status = c.Status,
            Subject = new SubjectDto
            {
                Id = c.Subject.Id,
                Name = c.Subject.Name,
            },
        };
    }
}
