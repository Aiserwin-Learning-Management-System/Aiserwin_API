namespace Winfocus.LMS.Application.Services
{
    using Org.BouncyCastle.Utilities.IO;
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
        public async Task<CommonResponse<List<CourseDto>>> GetAllAsync()
        {
            var courses = (await _repo.GetAllAsync()).Select(Map).ToList();
            return CommonResponse<List<CourseDto>>.SuccessResponse("Courses retrieved successfully", courses);
        }

        /// <summary>
        /// Gets course by identifier.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>
        /// CourseDto.
        /// </returns>
        public async Task<CommonResponse<CourseDto>> GetByIdAsync(Guid id)
        {
            var courses = (await _repo.GetByIdAsync(id)) is { } c ? Map(c) : null;
            if (courses == null)
            {
                return CommonResponse<CourseDto>.FailureResponse("Course not found");

            }
            else
            {
                return CommonResponse<CourseDto>.SuccessResponse("Courses retrieved successfully", courses);
            }
        }

           // => (await _repo.GetByIdAsync(id)) is { } c ? Map(c) : null;

        /// <summary>
        /// Gets courses by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// List of CourseDto.
        /// </returns>
        public async Task<CommonResponse<List<CourseDto>>> GetByStreamAsync(Guid streamId)
        {
            var courses = (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();
            if (courses == null || courses.Count() == 0)
            {
                return CommonResponse<List<CourseDto>>.FailureResponse("No courses found for the given stream");
            }
            else
            {
                return CommonResponse<List<CourseDto>>.SuccessResponse("Courses retrieved successfully", courses);
            }

        }

        //=> (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();

        /// <summary>
        /// Gets courses by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>
        /// List of CourseDto.
        /// </returns>
        public async Task<CommonResponse<List<CourseDto>>> GetBySubjectAsync(Guid subjectId)
        {
            var courses = (await _repo.GetBySubjectAsync(subjectId)).Select(Map).ToList();
            if (courses == null || courses.Count() == 0)
            {
                return CommonResponse<List<CourseDto>>.FailureResponse("No courses found for the given subject");
            }
            else
            {
                return CommonResponse<List<CourseDto>>.SuccessResponse("Courses retrieved successfully", courses);
            }
        }

        // => (await _repo.GetBySubjectAsync(subjectId)).Select(Map).ToList();

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The course request.</param>
        /// <returns>
        /// Created CourseDto.
        /// </returns>
        public async Task<CommonResponse<CourseDto>> CreateAsync(CourseRequest request)
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
            var created = Map(await _repo.AddAsync(course));
            return CommonResponse<CourseDto>.SuccessResponse("Course created successfully", created);

            // return Map(await _repo.AddAsync(course));
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
        public async Task<CommonResponse<CourseDto>> UpdateAsync(Guid id, CourseRequest request)
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

            var updated = Map(await _repo.UpdateAsync(course));
            return CommonResponse<CourseDto>.SuccessResponse("Course updated successfully", updated);
        }

        /// <summary>
        /// Soft deletes a course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            bool res = await _repo.SoftDeleteAsync(id);
            if (res)
            {
                return CommonResponse<bool>.SuccessResponse("Course deleted successfully", res);
            }
            else
            {
                 return CommonResponse<bool>.FailureResponse("Course not found or could not be deleted");
            }
        }

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
