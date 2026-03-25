using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.DtpAdmin;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Service for Daily Activity Report business logic.
    /// </summary>
    public class DarService : IDarService
    {
        private readonly IDarRepository _repository;
        private readonly ILogger<DarService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="DarService"/> class.
        /// </summary>
        /// <param name="repository">The DAR repository.</param>
        /// <param name="logger">The logger.</param>
        public DarService(IDarRepository repository, ILogger<DarService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger;
        }

        /// <summary>
        /// Creates a new Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="request">The DAR request DTO.</param>
        /// <returns>CommonResponse containing the created DAR response DTO.</returns>
        public async Task<CommonResponse<DarResponseDto>> CreateAsync(Guid operatorId, DarRequestDto request)
        {
            try
            {
                _logger.LogInformation("Creating DAR for operator: {OperatorId} on date: {ReportDate}", operatorId, request.ReportDate);

                // Check for existing DAR on the same date for this operator
                var existingDar = await _repository.GetTodayByOperatorAsync(operatorId, request.ReportDate);
                if (existingDar != null)
                {
                    _logger.LogWarning("DAR already exists for operator {OperatorId} on {ReportDate}", operatorId, request.ReportDate);
                    return CommonResponse<DarResponseDto>.FailureResponse(
                        "A Daily Activity Report already exists for this date");
                }

                // Create new DAR entity
                var dar = new DailyActivityReport
                {
                    Id = Guid.NewGuid(),
                    OperatorId = operatorId,
                    TaskId = request.TaskId,
                    ReportDate = request.ReportDate,
                    QuestionsTyped = request.QuestionsTyped,
                    TimeSpentHours = request.TimeSpentHours,
                    IssuesFaced = request.IssuesFaced,
                    Remarks = request.Remarks,
                    Status = (int)DarStatus.Draft,
                    CreatedBy = operatorId,
                    CreatedAt = DateTime.UtcNow,
                    IsActive = true,
                    IsDeleted = false
                };

                // Persist
                var created = await _repository.CreateAsync(dar);
                _logger.LogInformation("DAR created successfully with ID: {DarId}", created.Id);

                // Map to response
                var response = MapToResponse(created);
                return CommonResponse<DarResponseDto>.SuccessResponse(
                    "Daily Activity Report created successfully",
                    response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error creating DAR for operator {OperatorId}", operatorId);
                return CommonResponse<DarResponseDto>.FailureResponse(
                    $"An error occurred while creating the report: {ex.Message}");
            }
        }

        /// <summary>
        /// Updates a Draft Daily Activity Report asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="darId">The DAR identifier.</param>
        /// <param name="request">The DAR request DTO.</param>
        /// <returns>CommonResponse containing the updated DAR response DTO.</returns>
        public async Task<CommonResponse<DarResponseDto>> UpdateAsync(Guid operatorId, Guid darId, DarRequestDto request)
        {
            try
            {
                _logger.LogInformation("Updating DAR {DarId} for operator {OperatorId}", darId, operatorId);

                // Fetch existing DAR
                var dar = await _repository.GetByIdAsync(darId);
                if (dar == null)
                {
                    _logger.LogWarning("DAR {DarId} not found", darId);
                    return CommonResponse<DarResponseDto>.FailureResponse("Daily Activity Report not found");
                }

                // Verify operator ownership
                if (dar.OperatorId != operatorId)
                {
                    _logger.LogWarning("Operator {OperatorId} attempted to update DAR of another operator", operatorId);
                    return CommonResponse<DarResponseDto>.FailureResponse("You don't have permission to update this report");
                }

                // Validate status (only Draft can be updated)
                if (dar.Status != (int)DarStatus.Draft)
                {
                    _logger.LogWarning("Attempted to update submitted DAR {DarId}", darId);
                    return CommonResponse<DarResponseDto>.FailureResponse(
                        "Cannot update a submitted Daily Activity Report");
                }

                // Update properties
                dar.TaskId = request.TaskId;
                dar.ReportDate = request.ReportDate;
                dar.QuestionsTyped = request.QuestionsTyped;
                dar.TimeSpentHours = request.TimeSpentHours;
                dar.IssuesFaced = request.IssuesFaced;
                dar.Remarks = request.Remarks;
                dar.UpdatedBy = operatorId;

                // Persist
                var updated = await _repository.UpdateAsync(dar);
                _logger.LogInformation("DAR {DarId} updated successfully", darId);

                // Map to response
                var response = MapToResponse(updated);
                return CommonResponse<DarResponseDto>.SuccessResponse(
                    "Daily Activity Report updated successfully",
                    response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error updating DAR {DarId}", darId);
                return CommonResponse<DarResponseDto>.FailureResponse(
                    $"An error occurred while updating the report: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets a Daily Activity Report by ID asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="darId">The DAR identifier.</param>
        /// <returns>CommonResponse containing the DAR response DTO.</returns>
        public async Task<CommonResponse<DarResponseDto>> GetByIdAsync(Guid operatorId, Guid darId)
        {
            try
            {
                _logger.LogInformation("Fetching DAR {DarId} for operator {OperatorId}", darId, operatorId);

                var dar = await _repository.GetByIdAsync(darId);
                if (dar == null)
                {
                    _logger.LogWarning("DAR {DarId} not found", darId);
                    return CommonResponse<DarResponseDto>.FailureResponse("Daily Activity Report not found");
                }

                // Verify operator ownership
                if (dar.OperatorId != operatorId)
                {
                    _logger.LogWarning("Operator {OperatorId} attempted to access DAR of another operator", operatorId);
                    return CommonResponse<DarResponseDto>.FailureResponse("You don't have permission to view this report");
                }

                var response = MapToResponse(dar);
                return CommonResponse<DarResponseDto>.SuccessResponse(
                    "Daily Activity Report fetched successfully",
                    response);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching DAR {DarId}", darId);
                return CommonResponse<DarResponseDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets paginated Daily Activity Reports for an operator asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="request">The pagination request.</param>
        /// <returns>CommonResponse containing paginated DAR list DTOs.</returns>
        public async Task<CommonResponse<PagedResult<DarListDto>>> GetListAsync(Guid operatorId, PagedRequest request)
        {
            try
            {
                _logger.LogInformation("Fetching DARs for operator {OperatorId} with offset {Offset}, limit {Limit}",
                    operatorId, request.Offset, request.Limit);

                var (items, totalCount) = await _repository.GetOperatorDarsAsync(operatorId, request);

                var mappedItems = items.Select(MapToListDto).ToList();
                var result = new PagedResult<DarListDto>(mappedItems, totalCount, request.Limit, request.Offset);

                return CommonResponse<PagedResult<DarListDto>>.SuccessResponse(
                    "Daily Activity Reports fetched successfully",
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching DARs for operator {OperatorId}", operatorId);
                return CommonResponse<PagedResult<DarListDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets today's Daily Activity Report or empty template asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <returns>CommonResponse containing today's DAR DTO (exists flag + template).</returns>
        public async Task<CommonResponse<DarTodayDto>> GetTodayAsync(Guid operatorId)
        {
            try
            {
                _logger.LogInformation("Fetching today's DAR for operator {OperatorId}", operatorId);

                var today = DateOnly.FromDateTime(DateTime.UtcNow);
                var dar = await _repository.GetTodayByOperatorAsync(operatorId, today);

                if (dar != null)
                {
                    var response = new DarTodayDto
                    {
                        Exists = true,
                        Template = new DarTodayTemplateDto
                        {
                            ReportDate = dar.ReportDate,
                            TaskId = dar.TaskId,
                            QuestionsTyped = dar.QuestionsTyped,
                            TimeSpentHours = dar.TimeSpentHours,
                            IssuesFaced = dar.IssuesFaced,
                            Remarks = dar.Remarks
                        }
                    };

                    return CommonResponse<DarTodayDto>.SuccessResponse(
                        "Today's Daily Activity Report fetched successfully",
                        response);
                }
                else
                {
                    // Return empty template
                    var response = new DarTodayDto
                    {
                        Exists = false,
                        Template = new DarTodayTemplateDto
                        {
                            ReportDate = today,
                            TaskId = null,
                            QuestionsTyped = 0,
                            TimeSpentHours = 0,
                            IssuesFaced = null,
                            Remarks = null
                        }
                    };

                    return CommonResponse<DarTodayDto>.SuccessResponse(
                        "Empty template returned for today",
                        response);
                }
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching today's DAR for operator {OperatorId}", operatorId);
                return CommonResponse<DarTodayDto>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Submits a Draft Daily Activity Report (changes status to Submitted) asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="darId">The DAR identifier.</param>
        /// <returns>CommonResponse indicating success or failure.</returns>
        public async Task<CommonResponse<bool>> SubmitAsync(Guid operatorId, Guid darId)
        {
            try
            {
                _logger.LogInformation("Submitting DAR {DarId} for operator {OperatorId}", darId, operatorId);

                // Fetch existing DAR
                var dar = await _repository.GetByIdAsync(darId);
                if (dar == null)
                {
                    _logger.LogWarning("DAR {DarId} not found", darId);
                    return CommonResponse<bool>.FailureResponse("Daily Activity Report not found");
                }

                // Verify operator ownership
                if (dar.OperatorId != operatorId)
                {
                    _logger.LogWarning("Operator {OperatorId} attempted to submit DAR of another operator", operatorId);
                    return CommonResponse<bool>.FailureResponse("You don't have permission to submit this report");
                }

                // Validate status (only Draft can be submitted)
                if (dar.Status != (int)DarStatus.Draft)
                {
                    _logger.LogWarning("Attempted to submit non-draft DAR {DarId}", darId);
                    return CommonResponse<bool>.FailureResponse(
                        "Only draft Daily Activity Reports can be submitted");
                }

                // Change status to Submitted
                dar.Status = (int)DarStatus.Submitted;
                dar.UpdatedBy = operatorId;

                // Persist
                await _repository.UpdateAsync(dar);
                _logger.LogInformation("DAR {DarId} submitted successfully", darId);

                return CommonResponse<bool>.SuccessResponse(
                    "Daily Activity Report submitted successfully",
                    true);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error submitting DAR {DarId}", darId);
                return CommonResponse<bool>.FailureResponse(
                    $"An error occurred while submitting the report: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets all Daily Activity Reports for admin view asynchronously.
        /// </summary>
        /// <param name="filter">The filter request with optional operator ID and date range.</param>
        /// <returns>CommonResponse containing paginated DAR response DTOs.</returns>
        public async Task<CommonResponse<PagedResult<DarResponseDto>>> GetAllDarsAdminAsync(DarFilterRequest filter)
        {
            try
            {
                _logger.LogInformation("Fetching all DARs for admin with offset {Offset}, limit {Limit}",
                    filter.Offset, filter.Limit);

                var (items, totalCount) = await _repository.GetAllDarsAsync(filter);

                var mappedItems = items.Select(MapToResponse).ToList();
                var result = new PagedResult<DarResponseDto>(mappedItems, totalCount, filter.Limit, filter.Offset);

                return CommonResponse<PagedResult<DarResponseDto>>.SuccessResponse(
                    "Daily Activity Reports fetched successfully",
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching all DARs for admin");
                return CommonResponse<PagedResult<DarResponseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Gets Daily Activity Reports for a specific operator (admin view) asynchronously.
        /// </summary>
        /// <param name="operatorId">The operator identifier.</param>
        /// <param name="filter">The filter request with date range.</param>
        /// <returns>CommonResponse containing paginated DAR response DTOs.</returns>
        public async Task<CommonResponse<PagedResult<DarResponseDto>>> GetOperatorDarsAdminAsync(
            Guid operatorId, DarFilterRequest filter)
        {
            try
            {
                _logger.LogInformation("Fetching DARs for operator {OperatorId} (admin view) with offset {Offset}, limit {Limit}",
                    operatorId, filter.Offset, filter.Limit);

                // Override the filter to ensure we only get this operator's DARs
                filter.OperatorId = operatorId;

                var (items, totalCount) = await _repository.GetAllDarsAsync(filter);

                var mappedItems = items.Select(MapToResponse).ToList();
                var result = new PagedResult<DarResponseDto>(mappedItems, totalCount, filter.Limit, filter.Offset);

                return CommonResponse<PagedResult<DarResponseDto>>.SuccessResponse(
                    "Daily Activity Reports fetched successfully",
                    result);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching DARs for operator {OperatorId}", operatorId);
                return CommonResponse<PagedResult<DarResponseDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        /// <summary>
        /// Maps a DailyActivityReport entity to DarResponseDto.
        /// </summary>
        /// <param name="dar">The Daily Activity Report entity.</param>
        /// <returns>The mapped response DTO.</returns>
        private DarResponseDto MapToResponse(DailyActivityReport dar)
        {
            var response = new DarResponseDto
            {
                Id = dar.Id,
                OperatorName = dar.Operator?.RegistrationForm != null
                    ? GetOperatorName(dar.Operator)
                    : "Unknown Operator",
                ReportDate = dar.ReportDate,
                QuestionsTyped = dar.QuestionsTyped,
                TimeSpentHours = dar.TimeSpentHours,
                IssuesFaced = dar.IssuesFaced,
                Remarks = dar.Remarks,
                Status = ((DarStatus)dar.Status).ToString(),
                CreatedAt = dar.CreatedAt
            };

            // Map task information if available
            if (dar.TaskAssignment != null)
            {
                response.Task = new DarTaskDto
                {
                    TaskId = dar.TaskAssignment.Id,
                    TaskCode = dar.TaskAssignment.TaskCode,
                    Subject = dar.TaskAssignment.Subject?.Name ?? "Unknown Subject"
                };
            }

            return response;
        }

        /// <summary>
        /// Maps a DailyActivityReport entity to DarListDto.
        /// </summary>
        /// <param name="dar">The Daily Activity Report entity.</param>
        /// <returns>The mapped list DTO.</returns>
        private DarListDto MapToListDto(DailyActivityReport dar)
        {
            return new DarListDto
            {
                Id = dar.Id,
                ReportDate = dar.ReportDate,
                QuestionsTyped = dar.QuestionsTyped,
                TimeSpentHours = dar.TimeSpentHours,
                Status = ((DarStatus)dar.Status).ToString(),
                CreatedAt = dar.CreatedAt
            };
        }

        /// <summary>
        /// Extracts operator name from registration form field values.
        /// </summary>
        /// <param name="operator">The StaffRegistration entity.</param>
        /// <returns>The operator's name or "Unknown Operator".</returns>
        private string GetOperatorName(StaffRegistration @operator)
        {
            try
            {
                // Names are stored in StaffRegistrationValue collection
                // Try to extract from common name fields (FirstName or Name field)
                var firstNameValue = @operator?.Values
                    ?.FirstOrDefault(v => v.FieldName?.ToLower().Contains("name") == true)
                    ?.Value;

                return !string.IsNullOrWhiteSpace(firstNameValue)
                    ? firstNameValue
                    : "Unknown Operator";
            }
            catch
            {
                return "Unknown Operator";
            }
        }
    }
}
