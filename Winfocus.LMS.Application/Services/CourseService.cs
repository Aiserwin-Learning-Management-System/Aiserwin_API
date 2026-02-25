namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Org.BouncyCastle.Utilities.IO;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Domain.Enums;

    /// <summary>
    /// CourseService.
    /// </summary>
    public sealed class CourseService : ICourseService
    {
        private readonly ICourseRepository _repo;
        private readonly ILogger<CourseService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="CourseService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="logger">The logger.</param>
        public CourseService(ICourseRepository repo, ILogger<CourseService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>
        /// CourseDto.
        /// </returns>
        public async Task<CommonResponse<List<CourseDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all courses started.");
                var courses = (await _repo.GetAllAsync()).Select(Map).ToList();
                _logger.LogInformation(
                  "Successfully retrieved {CourseCount} courses.",
                  courses.Count);

                return CommonResponse<List<CourseDto>>.SuccessResponse("Courses retrieved successfully", courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "An error occurred while fetching courses.");
                return CommonResponse<List<CourseDto>>.FailureResponse($"An error occurred while fetching courses: {ex.Message}");
            }
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
            try
            {
                _logger.LogInformation("Fetching course with Id: {CourseId}", id);
                var courses = (await _repo.GetByIdAsync(id)) is { } c ? Map(c) : null;
                if (courses == null)
                {
                    _logger.LogWarning("Course not found for Id: {CourseId}", id);
                    return CommonResponse<CourseDto>.FailureResponse("Course not found");
                }

                _logger.LogInformation("Course retrieved successfully for Id: {CourseId}", id);

                return CommonResponse<CourseDto>.SuccessResponse("Courses retrieved successfully", courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while fetching course with Id: {CourseId}", id);
                return CommonResponse<CourseDto>.FailureResponse($"An error occurred while fetching the course: {ex.Message}");
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
            try
            {
                _logger.LogInformation("Fetching courses for StreamId: {StreamId}", streamId);
                var courses = (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();
                if (courses == null || courses.Count() == 0)
                {
                    _logger.LogWarning("No courses found for StreamId: {StreamId}", streamId);

                    return CommonResponse<List<CourseDto>>.FailureResponse("No courses found for the given stream");
                }
                else
                {
                    _logger.LogInformation("Successfully retrieved {CourseCount} courses for StreamId: {StreamId}",
                      courses.Count,
                      streamId);
                    return CommonResponse<List<CourseDto>>.SuccessResponse("Courses retrieved successfully", courses);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                   "Error occurred while fetching courses for StreamId: {StreamId}",
                   streamId);
                return CommonResponse<List<CourseDto>>.FailureResponse($"An error occurred while fetching courses: {ex.Message}");
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
            try
            {
                _logger.LogInformation("Fetching courses for SubjectId: {SubjectId}", subjectId);
                var courses = (await _repo.GetBySubjectAsync(subjectId)).Select(Map).ToList();
                if (courses == null || courses.Count() == 0)
                {
                    _logger.LogWarning("No courses found for SubjectId: {SubjectId}", subjectId);
                    return CommonResponse<List<CourseDto>>.FailureResponse("No courses found for the given subject");
                }

                _logger.LogInformation(
                      "Successfully retrieved {CourseCount} courses for SubjectId: {SubjectId}",
                      courses.Count,
                      subjectId);
                return CommonResponse<List<CourseDto>>.SuccessResponse("Courses retrieved successfully", courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                  "Error occurred while fetching courses for SubjectId: {SubjectId}",
                  subjectId);
                return CommonResponse<List<CourseDto>>.FailureResponse($"An error occurred while fetching courses: {ex.Message}");
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
            try
            {
                _logger.LogInformation(
                  "Creating course with Name: {CourseName}, SubjectId: {SubjectId}, GradeId: {GradeId}",
                  request.coursename,
                  request.subjectid,
                  request.gradeid);
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
                var createdEntity = await _repo.AddAsync(course);
                var created = Map(createdEntity);

                _logger.LogInformation(
                   "Course created successfully with Id: {CourseId}",
                   created.Id);
                return CommonResponse<CourseDto>.SuccessResponse("Course created successfully", created);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex,
                   "Error occurred while creating course with Name: {CourseName}",
                   request.coursename);
                return CommonResponse<CourseDto>.FailureResponse($"An error occurred while creating the course: {ex.Message}");
            }
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
            try
            {
                _logger.LogInformation(
            "Updating course. CourseId: {CourseId}",
            id);

                var course = await _repo.GetByIdAsync(id);
                if (course == null)
                {
                    _logger.LogWarning(
                        "Update failed. Course not found. CourseId: {CourseId}",
                        id);

                    return CommonResponse<CourseDto>
                        .FailureResponse("Course not found");
                }

                course.Name = request.coursename;
                course.SubjectId = request.subjectid;
                course.GradeId = request.gradeid;
                course.CourseDescription = request.cousedescription;
                course.CourseUrl = request.courseurl;
                course.MaxStudent = request.maxstudent;
                course.AcademicYear = request.academicyear;
                course.UpdatedAt = DateTime.UtcNow;

                var updatedEntity = await _repo.UpdateAsync(course);
                var updated = Map(updatedEntity);

                _logger.LogInformation(
                    "Course updated successfully. CourseId: {CourseId}",
                    updated.Id);

                return CommonResponse<CourseDto>.SuccessResponse("Course updated successfully", updated);
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error occurred while updating course. CourseId: {CourseId}",
                   id);
                return CommonResponse<CourseDto>.FailureResponse($"An error occurred while updating the course: {ex.Message}");
            }
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
            try
            {
                _logger.LogInformation(
                   "Deleting course. CourseId: {CourseId}",
                   id);
                bool res = await _repo.SoftDeleteAsync(id);
                if (res)
                {
                    _logger.LogInformation(
              "Course deleted successfully. CourseId: {CourseId}",
              id);
                    return CommonResponse<bool>.SuccessResponse("Course deleted successfully", res);
                }
                else
                {
                    _logger.LogWarning(
                "Delete failed. Course not found or already deleted. CourseId: {CourseId}",
                id);

                    return CommonResponse<bool>.FailureResponse("Course not found or could not be deleted");
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(
                  ex,
                  "Error occurred while deleting course. CourseId: {CourseId}",
                  id);
                return CommonResponse<bool>.FailureResponse($"An error occurred while deleting the course: {ex.Message}");
            }
        }

        /// <summary>
        /// Retrieves courses based on multiple filter criteria with pagination support.
        /// </summary>
        /// <param name="centreId">Centre identifier used to filter courses.</param>
        /// <param name="syllabusId">Syllabus identifier used to filter courses.</param>
        /// <param name="gradeId">Grade identifier used to filter courses.</param>
        /// <param name="streamId">Stream identifier used to filter courses.</param>
        /// <param name="subjectsId">Subject identifier used to filter courses.</param>
        /// <param name="startDate">Filters courses created on or after this date.</param>
        /// <param name="endDate">Filters courses created on or before this date.</param>
        /// <param name="active">Indicates whether to filter active or inactive courses.</param>
        /// <param name="searchText">Search keyword applied to course name.</param>
        /// <param name="limit">Number of records to return (page size).</param>
        /// <param name="offset">Number of records to skip.</param>
        /// <param name="sortOrder">Sorting order ("asc" or "desc").</param>
        /// <returns>
        /// A <see cref="CommonResponse{T}"/> containing a paginated list of
        /// <see cref="CourseDto"/> objects.
        /// </returns>
        public async Task<CommonResponse<PagedResult<CourseDto>>> GetFilteredAsync(
       Guid? centreId,
       Guid? syllabusId,
       Guid? gradeId,
       Guid? streamId,
       Guid? subjectsId,
       DateTime? startDate,
       DateTime? endDate,
       bool? active,
       string? searchText,
       int limit,
       int offset,
       string sortOrder)
        {
            try
            {
                _logger.LogInformation(
                             "Fetching filtered courses. Filters => CentreId:{CentreId}, SyllabusId:{SyllabusId}, GradeId:{GradeId}, StreamId:{StreamId}, Active:{Active}, Search:{SearchText}, Limit:{Limit}, Offset:{Offset}, SortOrder:{SortOrder}",
                             centreId, syllabusId, gradeId, streamId, active, searchText, limit, offset, sortOrder);
                var query = _repo.Query();

                if (syllabusId.HasValue)
                {
                    query = query.Where(c =>
                        c.Stream.Grade.SyllabusId == syllabusId);
                }

                if (gradeId.HasValue)
                {
                    query = query.Where(c =>
                        c.Stream.GradeId == gradeId);
                }

                if (streamId.HasValue)
                {
                    query = query.Where(c =>
                        c.StreamId == streamId);
                }

                if (active.HasValue)
                    query = query.Where(x => x.IsActive == active);

                if (startDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= startDate.Value);

                if (endDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= endDate.Value);

                if (!string.IsNullOrWhiteSpace(searchText))
                {
                    query = query.Where(x =>
                        x.Name.Contains(searchText));
                }

                var totalCount = await query.CountAsync();

                _logger.LogInformation(
                "Filtered courses count: {TotalCount}",
                totalCount);

                query = sortOrder.ToLower() == "desc"
                    ? query.OrderByDescending(x => x.CreatedAt)
                    : query.OrderBy(x => x.CreatedAt);

                var courses = await query
                    .Skip(offset)
                    .Take(limit)
                    .ToListAsync();

                var dtoList = courses.Select(c => new CourseDto
                {
                    Id = c.Id,
                    Name = c.Name,
                    IsActive = c.IsActive,
                }).ToList();

                _logger.LogInformation(
               "Returning {ReturnedCount} courses (Offset:{Offset}, Limit:{Limit})",
               dtoList.Count, offset, limit);
                return CommonResponse<PagedResult<CourseDto>>.SuccessResponse(
                "Courses fetched successfully",
                new PagedResult<CourseDto>(
                    dtoList,
                    totalCount,
                    limit,
                    offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(
                   ex,
                   "Error occurred while fetching filtered courses. Filters => CentreId:{CentreId}, SyllabusId:{SyllabusId}, GradeId:{GradeId}, StreamId:{StreamId}, Active:{Active}, Search:{SearchText}, Limit:{Limit}, Offset:{Offset}, SortOrder:{SortOrder}",
                   centreId, syllabusId, gradeId, streamId, active, searchText, limit, offset, sortOrder);
                return CommonResponse<PagedResult<CourseDto>>.FailureResponse($"An error occurred while fetching courses: {ex.Message}");
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
