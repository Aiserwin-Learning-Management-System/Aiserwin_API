namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Org.BouncyCastle.Utilities.IO;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
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
        private readonly ILogger<SubjectService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="SubjectService"/> class.
        /// </summary>
        /// <param name="repo">The repo.</param>
        /// <param name="logger">The logger.</param>
        public SubjectService(ISubjectRepository repo, ILogger<SubjectService> logger)
        {
            _repo = repo;
            _logger = logger;
        }

        /// <summary>
        /// Gets all active subjects.
        /// </summary>
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// A list of subject DTOs.
        /// </returns>
        public async Task<CommonResponse<List<SubjectDto>>> GetAllAsync(Guid centerId)
        {
            try
            {
                _logger.LogInformation("Fetching all active subjects.");
                var subjects = (await _repo.GetAllAsync(centerId))
                    .Select(Map)
                    .ToList();
                _logger.LogInformation("Retrieved {Count} subjects.", subjects.Count);
                return CommonResponse<List<SubjectDto>>.SuccessResponse(
                    "subjects retrieved successfully", subjects);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subjects.");
                return CommonResponse<List<SubjectDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a subject by identifier.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <returns>
        /// The subject DTO if found; otherwise, null.
        /// </returns>
        public async Task<CommonResponse<SubjectDto>> GetByIdAsync(Guid id)
        {
            try
            {
                var subject = (await _repo.GetByIdAsync(id)) is { } c ? Map(c) : null;
                if (subject == null)
                {
                    return CommonResponse<SubjectDto>.FailureResponse("subject not found");
                }
                else
                {
                    return CommonResponse<SubjectDto>.SuccessResponse("Subject retrieved successfully", subject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subject with ID: {Id}", id);
                return CommonResponse<SubjectDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a subject by identifier.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// The subject DTO if found; otherwise, null.
        /// </returns>
        public async Task<CommonResponse<SubjectDto>> GetByIdCenterIdAsync(Guid id, Guid centerId)
        {
            try
            {
                var subject = (await _repo.GetByCenterIdIdAsync(id, centerId)) is { } c ? Map(c) : null;
                if (subject == null)
                {
                    return CommonResponse<SubjectDto>.FailureResponse("subject not found");
                }
                else
                {
                    return CommonResponse<SubjectDto>.SuccessResponse("Subject retrieved successfully", subject);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subject with ID: {Id}", id);
                return CommonResponse<SubjectDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets subjects by stream identifier.
        /// </summary>
        /// <param name="streamId">The stream identifier.</param>
        /// <returns>
        /// A list of subject DTOs associated with the specified stream.
        /// </returns>
        public async Task<CommonResponse<List<SubjectDto>>> GetByStreamAsync(Guid streamId)
        {
            try
            {
                var subjects = (await _repo.GetByStreamAsync(streamId)).Select(Map).ToList();
                if (subjects == null)
                {
                    return CommonResponse<List<SubjectDto>>.FailureResponse("subjects not found");
                }
                else
                {
                    return CommonResponse<List<SubjectDto>>.SuccessResponse("subject retrieved successfully", subjects);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subjects for stream ID: {StreamId}", streamId);
                return CommonResponse<List<SubjectDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets the by course ids asynchronous.
        /// </summary>
        /// <param name="courseIds">The course ids.</param>
        /// <returns>Subject list.</returns>
        public async Task<CommonResponse<List<SubjectDto>>> GetByCourseIdsAsync(List<Guid> courseIds)
        {
            try
            {
                var subjects = (await _repo.GetByCourseIdsAsync(courseIds)).Select(Map).ToList();
                if (subjects == null)
                {
                    return CommonResponse<List<SubjectDto>>.FailureResponse("subjects not found");
                }
                else
                {
                    return CommonResponse<List<SubjectDto>>.SuccessResponse("subject retrieved successfully", subjects);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching subjects for course IDs: {CourseIds}", string.Join(", ", courseIds));
                return CommonResponse<List<SubjectDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Creates a new subject.
        /// </summary>
        /// <param name="request">The subject creation request.</param>
        /// <returns>
        /// The created subject DTO.
        /// </returns>
        public async Task<CommonResponse<SubjectDto>> CreateAsync(SubjectRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Creating subject: {Name}, CourseId: {CourseId}",
                    request.name, request.courseid);

                if (request.subjectCode == null)
                {
                    return CommonResponse<SubjectDto>.FailureResponse("subject code not found");
                }

                var existingCourseCode = await _repo.ExistsByCodeAsync(request.subjectCode);
                if (existingCourseCode)
                    return CommonResponse<SubjectDto>.FailureResponse("subject code already exist");

                var course = new Subject
                {
                    Name = request.name,
                    CourseId = request.courseid,
                    CreatedAt = DateTime.UtcNow,
                    CreatedBy = request.userid,
                    SubjectCode= request.subjectCode
                };

                var created = await _repo.AddAsync(course);
                var response = new SubjectDto
                {
                    Name = created.Name,
                    CourseId = created.CourseId,
                    CreatedAt = created.CreatedAt,
                    CreatedBy = created.CreatedBy,
                    SubjectCode = created.SubjectCode,
                };

                _logger.LogInformation("subject created with Id: {Id}", created.Id);
                return CommonResponse<SubjectDto>.SuccessResponse(
                    "subject created successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating subject: {Name}", request.name);
                return CommonResponse<SubjectDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
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
        public async Task<CommonResponse<SubjectDto>> UpdateAsync(Guid id, SubjectRequest request)
        {
            try
            {
                _logger.LogInformation("Updating subject Id: {Id}", id);

                var course = await _repo.GetByIdAsync(id);
                if (course == null)
                {
                    return CommonResponse<SubjectDto>.FailureResponse("subject not found");
                }

                course.Name = request.name;
                course.CourseId = request.courseid;
                course.UpdatedAt = DateTime.UtcNow;
                course.UpdatedBy = request.userid;
                course.SubjectCode = request.subjectCode;

                var updated = await _repo.UpdateAsync(course);

                var response = new SubjectDto
                {
                    Name = updated.Name,
                    CourseId = updated.CourseId,
                    CreatedAt = updated.CreatedAt,
                    CreatedBy = updated.CreatedBy,
                    SubjectCode = updated.SubjectCode,
                };

                _logger.LogInformation("subject updated Id: {Id}", id);
                return CommonResponse<SubjectDto>.SuccessResponse(
                    "subject updated successfully", response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating subject Id: {Id}", id);
                return CommonResponse<SubjectDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Soft deletes a subject.
        /// </summary>
        /// <param name="id">The subject identifier.</param>
        /// <param name="centerId">The centerId.</param>
        /// <returns>
        /// A task that represents the asynchronous delete operation.
        /// </returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id, Guid centerId)
        {
            try
            {
                _logger.LogInformation("Deleting subject Id: {Id}", id);
                var result = await _repo.SoftDeleteAsync(id, centerId);

                if (result)
                {
                    return CommonResponse<bool>.SuccessResponse(
                        "Subject deleted successfully", true);
                }

                return CommonResponse<bool>.FailureResponse(
                    "Subject not found or already deleted");
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error deleting Subject Id: {Id}", id);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets filtered subjects with pagination support.
        /// Search works on Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated subject result.</returns>
        public async Task<CommonResponse<PagedResult<SubjectDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered subjects. Filters => Active:{Active}, " +
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
                        x.Course.Name.ToLower().Contains(searchTerm) ||
                        x.Course.Stream.Name.ToLower().Contains(searchTerm) ||
                        x.Course.Stream.Grade.Name.ToLower().Contains(searchTerm) ||
                        x.Course.Stream.Grade.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<SubjectDto>>.SuccessResponse(
                        "No subjects found.",
                        new PagedResult<SubjectDto>(
                            new List<SubjectDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "name" => isDesc ? query.OrderByDescending(x => x.Name)
                                             : query.OrderBy(x => x.Name),

                    "coursename" => isDesc ? query.OrderByDescending(x => x.Course.Name)
                                             : query.OrderBy(x => x.Course.Name),
                    "subjectcode" => isDesc ? query.OrderByDescending(x => x.SubjectCode)
                                                                 : query.OrderBy(x => x.SubjectCode),
                    "streamname" => isDesc ? query.OrderByDescending(x => x.Course.Stream.Name)
                                             : query.OrderBy(x => x.Course.Stream.Name),

                    "gradename" => isDesc ? query.OrderByDescending(x => x.Course.Stream.Grade.Name)
                                             : query.OrderBy(x => x.Course.Stream.Grade.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Course.Stream.Grade.Syllabus.Name)
                                             : query.OrderBy(x => x.Course.Stream.Grade.Syllabus.Name),

                    "isactive" => isDesc ? query.OrderByDescending(x => x.IsActive)
                                             : query.OrderBy(x => x.IsActive),

                    "createdat" => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),

                    _ => isDesc ? query.OrderByDescending(x => x.CreatedAt)
                                             : query.OrderBy(x => x.CreatedAt),
                };

                // ── Pagination ──
                var subjects = await query
                    .Skip(request.Offset)
                    .Take(request.Limit)
                    .ToListAsync();

                var dtoList = subjects.Select(Map).ToList();

                _logger.LogInformation(
                    "Returning {Count} of {Total} subjects",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<SubjectDto>>.SuccessResponse(
                    "subjects fetched successfully.",
                    new PagedResult<SubjectDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered subjects.");
                return CommonResponse<PagedResult<SubjectDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static SubjectDto Map(Subject s) => new()
        {
            Id = s.Id,
            Name = s.Name,
            SubjectCode = s.SubjectCode,
            CourseId = s.CourseId,
            StreamId = s.Course?.StreamId ?? Guid.Empty,
            GradeId = s.Course?.Stream?.GradeId ?? Guid.Empty,
            SyllabusId = s.Course?.Stream?.Grade?.SyllabusId ?? Guid.Empty,
            CenterId = s.Course?.Stream?.Grade?.Syllabus?.CenterId ?? Guid.Empty,
            StateId = s.Course?.Stream?.Grade?.Syllabus?.Center.StateId ?? Guid.Empty,
            ModeOfStudyId = s.Course?.Stream?.Grade?.Syllabus?.Center.ModeOfStudyId ?? Guid.Empty,
            CountryId = s.Course?.Stream?.Grade?.Syllabus?.Center.CountryId ?? Guid.Empty,
            IsActive = s.IsActive,
            Course = s.Course == null ? null : new CourseDto
            {
                Id = s.Course.Id,
                Name = s.Course.Name,
                IsActive = s.Course.IsActive,

                Stream = s.Course.Stream == null ? null : new StreamDto
                {
                    Id = s.Course.Stream.Id,
                    Name = s.Course.Stream.Name,

                    Grade = s.Course.Stream.Grade == null ? null : new GradeDto
                    {
                        Id = s.Course.Stream.Grade.Id,
                        Name = s.Course.Stream.Grade.Name,

                        Syllabus = s.Course.Stream.Grade.Syllabus == null ? null : new SyllabusDto
                        {
                            Id = s.Course.Stream.Grade.Syllabus.Id,
                            Name = s.Course.Stream.Grade.Syllabus.Name
                        }
                    }
                }
            }
        };
    }
}
