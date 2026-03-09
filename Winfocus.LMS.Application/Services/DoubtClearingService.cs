namespace Winfocus.LMS.Application.Services
{
    using Microsoft.AspNetCore.Http.HttpResults;
    using Microsoft.EntityFrameworkCore;
    using Microsoft.Extensions.Logging;
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Provides business operations for <see cref="DoubtClearing"/> entities.
    /// </summary>
    public class DoubtClearingService : IDoubtClearingService
    {
        private readonly IDoubtClearingRepository _repository;
        private readonly ILogger<DoubtClearingService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DoubtClearingService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public DoubtClearingService(IDoubtClearingRepository repository, ILogger<DoubtClearingService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>DoubtClearingDto.</returns>
        public async Task<CommonResponse<List<DoubtClearingDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Doubt clearing");
            var doutclearing = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count} doubt clearing", doutclearing.Count());
            var mappeddata = doutclearing.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<DoubtClearingDto>>.SuccessResponse("Fetching all doubt clearing", mappeddata);
            }
            else
            {
                return CommonResponse<List<DoubtClearingDto>>.FailureResponse("no doubt clearing section");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>DoubtClearingDto.</returns>
        public async Task<CommonResponse<DoubtClearingDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching doubt clear details by Id: {Id}", id);
            var doubtcleartiming = await _repository.GetByIdAsync(id);
            _logger.LogInformation("Doubt Clear details fetched successfully for Id: {Id}", id);
            var mappeddata = doubtcleartiming == null ? null : Map(doubtcleartiming);
            if (mappeddata != null)
            {
                return CommonResponse<DoubtClearingDto>.SuccessResponse("Doubt Clear details fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<DoubtClearingDto>.FailureResponse("Doubt Clear details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>DoubtClearingDto.</returns>
        public async Task<DoubtClearingDto> CreateAsync(DoubtClearingRequest request)
        {
            var doubtClear = new DoubtClearing
            {
                ScheduleTime = request.startTime,
                ScheduleEndTime = request.startTime, // Used for future
                SubjectId = request.subjectId,
                CreatedBy = request.userId,
                CreatedAt = DateTime.UtcNow,
            };

            var created = await _repository.AddAsync(doubtClear);
            _logger.LogInformation(
           "Doubt clearing scession created successfully. Id: {userId}",
           created.Id);
            return Map(created);
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <exception cref="KeyNotFoundException">Doubt clear session created not found.</exception>
        /// <returns>task.</returns>
        public async Task<DoubtClearingDto> UpdateAsync(Guid id, DoubtClearingRequest request)
        {
            _logger.LogInformation("Updating doubt cleare session details: {Id}", id);
            var doubtCleartiming = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Doubt clear scession not found");

            doubtCleartiming.ScheduleTime = request.startTime;
            doubtCleartiming.SubjectId = request.subjectId;
            doubtCleartiming.UpdatedBy = request.userId;
            doubtCleartiming.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(doubtCleartiming);
            _logger.LogInformation(
           "Doubt clear session updated successfully. Id: {Id}",
           id);
            return Map(updated);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            _logger.LogInformation("Deleting Doubt clear session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Doubt clear session deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Doubt clear session deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="subjectid">The identifier.</param>
        /// <returns>DoubtClearingDto.</returns>
        public async Task<List<DoubtClearingDto>> GetBySubjectIdAsync(Guid subjectid)
        {
            var doubtClearTimings = await _repository.GetBySubjectIdAsync(subjectid);
            return Map(doubtClearTimings);
        }

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        public async Task<CommonResponse<PagedResult<DoubtClearingDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered doubt clear data. Filters => Active:{Active}, " +
                    "Search:{SearchText}, SortBy:{SortBy}, SortOrder:{SortOrder}, " +
                    "Limit:{Limit}, Offset:{Offset}",
                    request.Active, request.SearchText, request.SortBy,
                    request.SortOrder, request.Limit, request.Offset);

                var query = _repository.Query();

                // ── Filters ──
                if (request.Active.HasValue)
                    query = query.Where(x => x.IsActive == request.Active.Value);

                if (request.StartDate.HasValue)
                    query = query.Where(x => x.CreatedAt >= request.StartDate.Value);

                if (request.EndDate.HasValue)
                    query = query.Where(x => x.CreatedAt <= request.EndDate.Value);

                // ── Search on Subject, Course, Stream, Grade, AND Syllabus Name ──
                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.ScheduleTime.ToString().Contains(searchTerm) ||
                        x.Subject.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Grade.Name.ToLower().Contains(searchTerm) ||
                        x.Subject.Course.Stream.Grade.Syllabus.Name.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<DoubtClearingDto>>.SuccessResponse(
                        "No doubt clear session found.",
                        new PagedResult<DoubtClearingDto>(
                            new List<DoubtClearingDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "scheduletime" => isDesc ? query.OrderByDescending(x => x.ScheduleTime)
                                             : query.OrderBy(x => x.ScheduleTime),
                    "subjectname" => isDesc ? query.OrderByDescending(x => x.Subject.Name)
                                             : query.OrderBy(x => x.Subject.Name),

                    "coursename" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Name)
                                             : query.OrderBy(x => x.Subject.Course.Name),

                    "streamname" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Name),

                    "gradename" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Grade.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Grade.Name),

                    "syllabusname" => isDesc ? query.OrderByDescending(x => x.Subject.Course.Stream.Grade.Syllabus.Name)
                                             : query.OrderBy(x => x.Subject.Course.Stream.Grade.Syllabus.Name),

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
                    "Returning {Count} of {Total} doubt clear schedule session",
                    dtoList.Count, totalCount);

                return CommonResponse<PagedResult<DoubtClearingDto>>.SuccessResponse(
                    "doubt clear session data fetched successfully.",
                    new PagedResult<DoubtClearingDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered doubt clear data.");
                return CommonResponse<PagedResult<DoubtClearingDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<DoubtClearingDto> Map(IEnumerable<DoubtClearing> doubtclearing)
        {
            return doubtclearing.Select(Map).ToList();
        }

        private static DoubtClearingDto Map(DoubtClearing c) =>
   new DoubtClearingDto
   {
       Id = c.Id,
       ScheduleStartDate = c.ScheduleTime.ToString("dd/MM/yyyy hh:mm tt"),
       //ScheduleEndDate = c.ScheduleTime.ToString("dd/MM/yyyy hh:mm tt"),
       SubjectId = c.SubjectId,
       IsActive = c.IsActive,
       Subject = c.Subject == null ? null : new SubjectDto
       {
           Id = c.Subject.Id,
           Name = c.Subject.Name,
           CourseId = c.Subject.CourseId,
           Course = c.Subject.Course == null ? null : new CourseDto
           {
               Id = c.Subject.Course.Id,
               Name = c.Subject.Course.Name,
               IsActive = c.Subject.Course.IsActive,
               StreamId = c.Subject.Course.StreamId,
               Stream = c.Subject.Course.Stream == null ? null : new StreamDto
               {
                   Id = c.Subject.Course.Stream.Id,
                   Name = c.Subject.Course.Stream.Name,
                   IsActive = c.Subject.Course.Stream.IsActive,
                   GradeId = c.Subject.Course.Stream.GradeId,
                   Grade = c.Subject.Course.Stream.Grade == null ? null : new GradeDto
                   {
                       Id = c.Subject.Course.Stream.Grade.Id,
                       Name = c.Subject.Course.Stream.Grade.Name,
                       IsActive = c.Subject.Course.Stream.Grade.IsActive,
                       SyllabusId = c.Subject.Course.Stream.Grade.SyllabusId,
                       Syllabus = c.Subject.Course.Stream.Grade.Syllabus == null ? null : new SyllabusDto
                       {
                           Id = c.Subject.Course.Stream.Grade.Syllabus.Id,
                           Name = c.Subject.Course.Stream.Grade.Syllabus.Name,
                           IsActive = c.Subject.Course.Stream.Grade.Syllabus.IsActive
                       }
                   }
               }
           }
       }
   };


    }
}
