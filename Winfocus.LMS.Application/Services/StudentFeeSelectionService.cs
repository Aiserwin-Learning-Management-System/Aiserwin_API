using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Fees;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.DTOs.Students;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Microsoft.EntityFrameworkCore;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business operations for <see cref="StudentFeeSelection"/> entities.
    /// </summary>
    public class StudentFeeSelectionService : IStudentFeeSelectionService
    {
        private readonly IStudentFeeSelectionRepository _repository;
        private readonly ILogger<StudentFeeSelectionService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentFeeSelection"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access.</param>
        /// <param name="logger">Logger.</param>
        public StudentFeeSelectionService(IStudentFeeSelectionRepository repository, ILogger<StudentFeeSelectionService> logger)
        {
            _repository = repository ?? throw new ArgumentNullException(nameof(repository));
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>DoubtClearingDto.</returns>
        public async Task<CommonResponse<List<StudentFeeSelectionDto>>> GetAllAsync()
        {
            _logger.LogInformation("Fetching all Student fee selection");
            var studentfeeSelection = await _repository.GetAllAsync();
            _logger.LogInformation("Fetched {Count}student fee selection", studentfeeSelection.Count());
            var mappeddata = studentfeeSelection.Select(Map).ToList();
            if (mappeddata.Any())
            {
                return CommonResponse<List<StudentFeeSelectionDto>>.SuccessResponse("Fetching all Student fee selection", mappeddata);
            }
            else
            {
                return CommonResponse<List<StudentFeeSelectionDto>>.FailureResponse("no Student fee selection");
            }
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentFeeSelectionDto.</returns>
        public async Task<CommonResponse<StudentFeeSelectionDto>> GetByIdAsync(Guid id)
        {
            _logger.LogInformation("Fetching student fee selection details by Id: {Id}", id);
            var studentfeeselection = await _repository.GetByIdAsync(id);
            _logger.LogInformation("student fee selection details fetched successfully for Id: {Id}", id);
            var mappeddata = studentfeeselection == null ? null : Map(studentfeeselection);
            if (mappeddata != null)
            {
                return CommonResponse<StudentFeeSelectionDto>.SuccessResponse("Student fee selection fetched successfully", mappeddata);
            }
            else
            {
                return CommonResponse<StudentFeeSelectionDto>.FailureResponse("Student fee selection details not found");
            }
        }

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentFeeSelectionDto.</returns>
        public async Task<StudentFeeSelectionDto> CreateAsync(StudentFeeSelectionRequest request)
        {
            decimal scholarshipPercent = request.scholarshipPercent == 0 ? 0 : request.scholarshipPercent;
            decimal seasonalPercent = request.seasonalPercent == 0 ? 0 : request.scholarshipPercent;
            decimal manualDiscountPercent = request.manualDiscountPercent == 0 ? 0 : request.scholarshipPercent;
            var finalAmountCalc = CalculateFinalAmount(
                                            request.baseFee,
                                            scholarshipPercent,
                                            request.isScholarshipActive,
                                            seasonalPercent,
                                            request.isSeasonalActive,
                                            manualDiscountPercent,
                                            request.isManualDiscountActive);

            var studentfeeSelection = new StudentFeeSelection(
             request.studentId,
             request.courseId,
             request.feePlanId,
             request.scholarshipPercent,
             request.isScholarshipActive,
             request.seasonalPercent,
             request.isSeasonalActive,
             request.manualDiscountPercent,
             request.isManualDiscountActive,
             request.baseFee,
             finalAmountCalc);

            studentfeeSelection.CreatedBy = request.userId;
            studentfeeSelection.CreatedAt = DateTime.UtcNow;

            var created = await _repository.AddAsync(studentfeeSelection);
            _logger.LogInformation(
           "student fee selection created successfully. Id: {userId}",
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
        public async Task<StudentFeeSelectionDto> UpdateAsync(Guid id, StudentFeeSelectionRequest request)
        {
            _logger.LogInformation("Updating Student fee selection session details: {Id}", id);
            var studentSelection = await _repository.GetByIdAsync(id)
                ?? throw new KeyNotFoundException("Student fee selection not found");

            decimal scholarshipPercent = request.scholarshipPercent == 0 ? 0 : request.scholarshipPercent;
            decimal seasonalPercent = request.seasonalPercent == 0 ? 0 : request.scholarshipPercent;
            decimal manualDiscountPercent = request.manualDiscountPercent == 0 ? 0 : request.scholarshipPercent;
            var finalAmountCalc = CalculateFinalAmount(
                                            request.baseFee,
                                            scholarshipPercent,
                                            request.isScholarshipActive,
                                            seasonalPercent,
                                            request.isSeasonalActive,
                                            manualDiscountPercent,
                                            request.isManualDiscountActive);

            var studentfeeSelection = new StudentFeeSelection(
             request.studentId,
             request.courseId,
             request.feePlanId,
             request.scholarshipPercent,
             request.isScholarshipActive,
             request.seasonalPercent,
             request.isSeasonalActive,
             request.manualDiscountPercent,
             request.isManualDiscountActive,
             request.baseFee,
             finalAmountCalc);

            studentSelection.UpdatedBy = request.userId;
            studentSelection.UpdatedAt = DateTime.UtcNow;

            var updated = await _repository.UpdateAsync(studentSelection);
            _logger.LogInformation(
           "Student Fee selection  updated successfully. Id: {Id}",
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
            _logger.LogInformation("Deleting Student fee Selection session Id: {Id}", id);
            bool res = await _repository.DeleteAsync(id);
            if (res)
            {
                _logger.LogInformation("Student fee Selection deleted successfully. Id: {Id}", id);
                return res;
            }
            else
            {
                _logger.LogWarning("Student fee Selection deletion failed. Id: {Id}", id);
                return res;
            }
        }

        /// <summary>
        /// Calculates the final amount after applying only active discounts.
        /// Discount application order: Scholarship → Seasonal → Manual (sequential).
        /// </summary>
        /// <param name="baseFee">The base fee.</param>
        /// <param name="scholarshipPercent">The scholarship percent.</param>
        /// <param name="isScholarshipActive">Whether scholarship is active.</param>
        /// <param name="seasonalPercent">The seasonal percent.</param>
        /// <param name="isSeasonalActive">Whether seasonal is active.</param>
        /// <param name="manualPercent">The manual percent.</param>
        /// <param name="isManualActive">Whether manual is active.</param>
        /// <returns>The final amount.</returns>
        private static decimal CalculateFinalAmount(
            decimal baseFee,
            decimal scholarshipPercent,
            bool isScholarshipActive,
            decimal seasonalPercent,
            bool isSeasonalActive,
            decimal manualPercent,
            bool isManualActive)
        {
            var amount = baseFee;

            if (isScholarshipActive && scholarshipPercent > 0)
            {
                amount -= amount * scholarshipPercent / 100m;
            }

            if (isSeasonalActive && seasonalPercent > 0)
            {
                amount -= amount * seasonalPercent / 100m;
            }

            if (isManualActive && manualPercent > 0)
            {
                amount -= amount * manualPercent / 100m;
            }

            return Math.Max(amount, 0);
        }

        /// <summary>
        /// Gets filtered batch timing for monday to frida with pagination support.
        /// Search works on Subject name, Course Name, Stream Name, Grade Name, and Syllabus Name.
        /// </summary>
        /// <param name="request">The paged request.</param>
        /// <returns>Paginated doubt clear time result.</returns>
        public async Task<CommonResponse<PagedResult<StudentFeeSelectionDto>>> GetFilteredAsync(
            PagedRequest request)
        {
            try
            {
                _logger.LogInformation(
                    "Fetching filtered Student Fee SelectionDto data. Filters => Active:{Active}, " +
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

                if (!string.IsNullOrWhiteSpace(request.SearchText))
                {
                    var searchTerm = request.SearchText.Trim().ToLower();
                    query = query.Where(x =>
                        x.FeePlan.PlanName.ToString().Contains(searchTerm) ||
                        x.BaseFee.ToString().Contains(searchTerm) ||
                        x.FinalAmount.ToString().Contains(searchTerm) ||
                        x.Student.RegistrationNumber.ToLower().Contains(searchTerm));
                }

                // ── Total Count ──
                var totalCount = await query.CountAsync();

                if (totalCount == 0)
                {
                    return CommonResponse<PagedResult<StudentFeeSelectionDto>>.SuccessResponse(
                        "No Student Fee Selection found.",
                        new PagedResult<StudentFeeSelectionDto>(
                            new List<StudentFeeSelectionDto>(), 0, request.Limit, request.Offset));
                }

                // ── Dynamic Sorting ──
                var isDesc = request.SortOrder.Equals("desc", StringComparison.OrdinalIgnoreCase);

                query = request.SortBy.ToLower() switch
                {
                    "planname" => isDesc ? query.OrderByDescending(x => x.FeePlan.PlanName)
                                             : query.OrderBy(x => x.FeePlan.PlanName),
                    "basefee" => isDesc ? query.OrderByDescending(x => x.BaseFee)
                                             : query.OrderBy(x => x.BaseFee),

                    "finalamount" => isDesc ? query.OrderByDescending(x => x.FinalAmount)
                                             : query.OrderBy(x => x.FinalAmount),

                    "registernumber" => isDesc ? query.OrderByDescending(x => x.Student.RegistrationNumber)
                                             : query.OrderBy(x => x.Student.RegistrationNumber),

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

                return CommonResponse<PagedResult<StudentFeeSelectionDto>>.SuccessResponse(
                    "Student Fee Selection data fetched successfully.",
                    new PagedResult<StudentFeeSelectionDto>(
                        dtoList, totalCount, request.Limit, request.Offset));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error fetching filtered Student Fee Selection data.");
                return CommonResponse<PagedResult<StudentFeeSelectionDto>>.FailureResponse(
                    $"An error occurred: {ex.Message}");
            }
        }

        private static List<StudentFeeSelectionDto> Map(IEnumerable<StudentFeeSelection> studentfeeselection)
        {
            return studentfeeselection.Select(Map).ToList();
        }

        private static StudentFeeSelectionDto Map(StudentFeeSelection c) =>
   new StudentFeeSelectionDto
   {
       Id = c.Id,
       StudentId = c.StudentId,
       CourseId = c.CourseId,
       IsActive = c.IsActive,
       FeePlanId = c.FeePlanId,
       ScholarshipPercent = c.ScholarshipPercent,
       IsScholarshipActive = c.IsScholarshipActive,
       SeasonalPercent =c.SeasonalPercent,
       IsSeasonalActive =c.IsSeasonalActive,
       ManualDiscountPercent =c.ManualDiscountPercent,
       IsManualDiscountActive =c.IsManualDiscountActive,
       BaseFee = c.BaseFee,
       FinalAmount = c.FinalAmount,
       PaymentMode = c.PaymentMode,

       Student = c.Student == null ? null : new StudentDto
       {
           Id = c.Student.Id,
           RegistraionNumber = c.Student.RegistrationNumber,
       },
       FeePlan =c.FeePlan == null ?null : new FeePlanDto
       {
           CourseId = c.CourseId,
           PlanName = c.FeePlan.PlanName,
           SubjectId =c.FeePlan.SubjectId
       }
   };
    }
}
