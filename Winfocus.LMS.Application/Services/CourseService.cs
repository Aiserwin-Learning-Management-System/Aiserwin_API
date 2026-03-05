namespace Winfocus.LMS.Application.Services
{
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

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
        /// <returns>CourseDto list.</returns>
        public async Task<CommonResponse<List<CourseDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all courses.");
                var courses = (await _repo.GetAllAsync()).Select(Map).ToList();

                return CommonResponse<List<CourseDto>>.SuccessResponse(
                    courses.Any() ? "Courses retrieved successfully" : "No courses found",
                    courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses.");
                return CommonResponse<List<CourseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets course by identifier.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>CourseDto.</returns>
        public async Task<CommonResponse<CourseDto>> GetByIdAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Fetching course with Id: {CourseId}", id);
                var course = await _repo.GetByIdAsync(id);

                if (course == null)
                {
                    return CommonResponse<CourseDto>.FailureResponse("Course not found");
                }

                return CommonResponse<CourseDto>.SuccessResponse(
                    "Course retrieved successfully", Map(course));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching course Id: {CourseId}", id);
                return CommonResponse<CourseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets courses by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>CourseDto list.</returns>
        public async Task<CommonResponse<List<CourseDto>>> GetByStreamAsync(Guid streamId)
        {
            try
            {
                _logger.LogInformation("Fetching courses for StreamId: {StreamId}", streamId);
                var courses = (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();

                return CommonResponse<List<CourseDto>>.SuccessResponse(
                    courses.Any()
                        ? "Courses retrieved successfully"
                        : "No courses found for this stream",
                    courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses for StreamId: {StreamId}", streamId);
                return CommonResponse<List<CourseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets courses by subject identifier.
        /// </summary>
        /// <param name="subjectId">The subject identifier.</param>
        /// <returns>CourseDto list.</returns>
        public async Task<CommonResponse<List<CourseDto>>> GetBySubjectAsync(Guid subjectId)
        {
            try
            {
                _logger.LogInformation("Fetching courses for SubjectId: {SubjectId}", subjectId);
                var courses = (await _repo.GetBySubjectAsync(subjectId)).Select(Map).ToList();

                return CommonResponse<List<CourseDto>>.SuccessResponse(
                    courses.Any()
                        ? "Courses retrieved successfully"
                        : "No courses found for this subject",
                    courses);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching courses for SubjectId: {SubjectId}", subjectId);
                return CommonResponse<List<CourseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new course.
        /// </summary>
        /// <param name="request">The course request.</param>
        /// <returns>Created CourseDto.</returns>
        public async Task<CommonResponse<CourseDto>> CreateAsync(CourseRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Creating course: {CourseName}, StreamId: {StreamId}",
                    request.coursename, request.streamid);

                if (request.courseCode == null)
                {
                    return CommonResponse<CourseDto>.FailureResponse("Course not found");
                }

                var course = new Course
                {
                    Name = request.coursename,
                    StreamId = request.streamid,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.userId,
                    CourseCode = request.courseCode,
                };

                var created = await _repo.AddAsync(course);

                _logger.LogInformation("Course created with Id: {CourseId}", created.Id);
                return CommonResponse<CourseDto>.SuccessResponse(
                    "Course created successfully", Map(created));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating course: {CourseName}", request.coursename);
                return CommonResponse<CourseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates an existing course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <param name="request">The course request.</param>
        /// <returns>Updated CourseDto.</returns>
        public async Task<CommonResponse<CourseDto>> UpdateAsync(Guid id, CourseRequest request)
        {
            try
            {
                _logger.LogInformation("Updating course Id: {CourseId}", id);

                var course = await _repo.GetByIdAsync(id);
                if (course == null)
                {
                    return CommonResponse<CourseDto>.FailureResponse("Course not found");
                }

                course.Name = request.coursename;
                course.StreamId = request.streamid;
                course.UpdatedAt = DateTime.UtcNow;
                course.UpdatedBy = request.userId;
                course.CourseCode = request.courseCode;

                var updated = await _repo.UpdateAsync(course);

                _logger.LogInformation("Course updated Id: {CourseId}", id);
                return CommonResponse<CourseDto>.SuccessResponse(
                    "Course updated successfully", Map(updated));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating course Id: {CourseId}", id);
                return CommonResponse<CourseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Soft deletes a course.
        /// </summary>
        /// <param name="id">The course identifier.</param>
        /// <returns>bool.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            try
            {
                _logger.LogInformation("Deleting course Id: {CourseId}", id);
                var result = await _repo.SoftDeleteAsync(id);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Course deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Course not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting course Id: {CourseId}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered courses with pagination support.
        /// Search works on Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated course result.</returns>
        public async Task<CommonResponse<PagedResult<CourseDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered courses. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repo.Query();

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                // ── Search on Course, Stream, Grade, AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.Name.ToLower().Contains(searchTerm) ||
                        x.Stream.Name.ToLower().Contains(searchTerm) ||
                        x.Stream.Grade.Name.ToLower().Contains(searchTerm) ||
                        x.Stream.Grade.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<CourseDto>>.SuccessResponse(
                        "No courses found.",
                        new PagedResult<CourseDto>(
                            new List<CourseDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),

                    "streamname" => isDesc ? query.OrderByDescending(x => x.Stream.Name)
                                             : query.OrderBy(x => x.Stream.Name),

                    "gradename" => isDesc ? query.OrderByDescending(x => x.Stream.Grade.Name)
                                             : query.OrderBy(x => x.Stream.Grade.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Stream.Grade.Syllabus.Name)
                                             : query.OrderBy(x => x.Stream.Grade.Syllabus.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var courses = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = courses.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} courses",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<CourseDto>>.SuccessResponse(
                    "Courses fetched successfully.",
                    new PagedResult<CourseDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered courses.");
                return CommonResponse<PagedResult<CourseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps course entity to DTO with full hierarchy.
        /// </summary>
        private static CourseDto Map(Course c) => new ()
        {
            Id = c.Id,
            Name = c.Name,
            IsActive = c.IsActive,
            StreamId = c.StreamId,
            CreatedBy = c.CreatedBy,
            CreatedAt = c.CreatedAt,
            UpdatedBy = c.UpdatedBy,
            UpdatedAt = c.UpdatedAt,
            CourseCode = c.CourseCode,
            Stream = c.Stream == null ? null : new StreamDto
            {
                Id = c.Stream.Id,
                Name = c.Stream.Name,
                IsActive = c.Stream.IsActive,
                GradeId = c.Stream.GradeId,
                Grade = c.Grade == null ? null : new GradeDto
                {
                    Id = c.Grade.Id,
                    Name = c.Grade.Name,
                    IsActive = c.Grade.IsActive,
                    SyllabusId = c.Grade.SyllabusId,
                    Syllabus = c.Grade.Syllabus == null ? null : new SyllabusDto
                    {
                        Id = c.Grade.Syllabus.Id,
                        Name = c.Grade.Syllabus.Name,
                        IsActive = c.Grade.Syllabus.IsActive,
                    },
                },
            },
        };
    }
}
