using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using System.Text.RegularExpressions;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Provides data access operations for <see cref="Student"/> entities.
    /// </summary>
    public sealed class StudentRepository : IStudentRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;
        private readonly ILogger<StudentRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="StudentRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        /// <param name="logger">The logger.</param>
        public StudentRepository(AppDbContext dbContext, ILogger<StudentRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>Student list.</returns>
        public async Task<IReadOnlyList<Student>> GetAllAsync()
        {
            return await _dbContext.Students
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Student.</returns>
        public async Task<Student?> GetByIdAsync(Guid id)
        {
            return await _dbContext.Students
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Country)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.State)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.ModeOfStudy)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Center)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Syllabus)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Grade)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Stream)
        .Include(x => x.AcademicDetails)
           .ThenInclude(ad => ad.Subject)
        .Include(x => x.StudentPersonalDetails)
        .Include(x => x.StudentDocuments)

        .Include(x => x.StudentAcademicCouses)
            .ThenInclude(sc => sc.Course)

        .Include(x => x.StudentBatchTimingMTFs)
           .ThenInclude(sc => sc.BatchTimingMTF)
        .Include(x => x.StudentBatchTimingSaturdays)
           .ThenInclude(sc => sc.BatchTimingSaturday)
        .Include(x => x.StudentBatchTimingSundays)
           .ThenInclude(sc => sc.BatchTimingSunday)

        .FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true);
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="student">The student.</param>
        /// <returns>Student.</returns>
        public async Task<Student> AddAsync(Student student)
        {
            var latestRecord = _dbContext.Students
                    .Where(item => !string.IsNullOrEmpty(item.RegistrationNumber))
                    .AsEnumerable()
                    .OrderByDescending(item => Convert.ToInt32(item.RegistrationNumber.Split('-').Last()))
                    .FirstOrDefault();

            var maxValue = latestRecord?.RegistrationNumber;

            if (string.IsNullOrEmpty(maxValue))
            {
                maxValue = "1";
                string formattedNumber = maxValue.ToString().PadLeft(4, '0');
                student.RegistrationNumber = $"AIS-{formattedNumber}";
            }
            else
            {
                    string formattedNumber = maxValue.ToString().PadLeft(4, '0');
                    string digitsOnly = Regex.Replace(formattedNumber, "[^0-9]", "");
                    digitsOnly = digitsOnly.Substring(digitsOnly.Length - 4);
                    int val = int.Parse(digitsOnly);
                    val++;
                    string num = val.ToString().PadLeft(4, '0');
                    student.RegistrationNumber = $"AIS-{num}";
            }

            student.CreatedAt = DateTime.UtcNow;
            _dbContext.Students.Add(student);
            await _dbContext.SaveChangesAsync();
            return student;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="student">The student.</param>
        /// <returns>task.</returns>
        public async Task UpdateAsync(Student student)
        {
            student.UpdatedAt = DateTime.UtcNow;
            _dbContext.Students.Update(student);
            await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.Students.FindAsync(id);
            if (entity == null)
            {
                return CommonResponse<bool>
                   .FailureResponse("Student not found");
            }

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.RegistrationStatus = RegistrationStatus.Disabled;

            _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();
            return CommonResponse<bool>
       .SuccessResponse("Student successfully deleted", true);
        }

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="countryId">The country identifier.</param>
        /// <param name="stateId">The state identifier.</param>
        /// <param name="modeId">The mode identifier.</param>
        /// <param name="centreId">The centre identifier.</param>
        /// <param name="batchId">The batch identifier.</param>
        /// <param name="gradeId">The grade identifier.</param>
        /// <param name="courseId">The course identifier.</param>
        /// <param name="startDate">The start date.</param>
        /// <param name="endDate">The end date.</param>
        /// <param name="registrationStatus">The registration status.</param>
        /// <param name="searchText">The search text.</param>
        /// <param name="limit">The limit.</param>
        /// <param name="offset">The offset.</param>
        /// <param name="sortBy">The sort by.</param>
        /// <param name="sortOrder">The sort order.</param>
        /// <returns>
        /// Student.
        /// </returns>
        public async Task<IReadOnlyList<Student>> GetFilteredAsync(
        Guid? countryId,
        Guid? stateId,
        Guid? modeId,
        Guid? centreId,
        Guid? batchId,
        Guid? gradeId,
        Guid? courseId,
        DateTime? startDate,
        DateTime? endDate,
        RegistrationStatus? registrationStatus,
        string? searchText,
        int limit,
        int offset,
        string sortBy,
        string sortOrder)
        {
            _logger.LogInformation(
                "Executing student filter query with parameters: Country={CountryId}, State={StateId}, Mode={ModeId}, Centre={CentreId}, Batch={BatchId}, Grade={GradeId}, Course={CourseId}, StartDate={StartDate}, EndDate={EndDate}, RegistrationStatus={RegistrationStatus}, SearchText={SearchText}, Limit={Limit}, Offset={Offset}, SortBy={SortBy}, SortOrder={SortOrder}",
                countryId,
                stateId,
                modeId,
                centreId,
                batchId,
                gradeId,
                courseId,
                startDate,
                endDate,
                registrationStatus,
                searchText,
                limit,
                offset,
                sortBy,
                sortOrder);

            var query = _dbContext.Students
                .Include(s => s.AcademicDetails)
                .Include(s => s.StudentPersonalDetails)
                .Include(s => s.StudentDocuments)
                .Include(s => s.StudentAcademicCouses)
                    .ThenInclude(sc => sc.Course)
                .AsQueryable();

            // Filters
            if (countryId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.CountryId == countryId.Value);
            }

            if (stateId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.StateId == stateId.Value);
            }

            if (modeId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.ModeOfStudyId == modeId.Value);
            }

            if (centreId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.CenterId == centreId.Value);
            }

            if (batchId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.BatchId == batchId.Value);
            }

            if (gradeId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.GradeId == gradeId.Value);
            }

            if (courseId.HasValue)
            {
                query = query.Where(s => s.StudentAcademicCouses.Any(c => c.CourseId == courseId.Value));
            }

            if (startDate.HasValue && endDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt >= startDate.Value && s.CreatedAt <= endDate.Value);
            }

            if (registrationStatus.HasValue)
            {
                query = query.Where(s => s.RegistrationStatus == registrationStatus.Value);
            }

            // Search (partial match)
            if (!string.IsNullOrEmpty(searchText))
            {
                var lower = searchText.ToLower();
                query = query.Where(s =>
                    s.StudentPersonalDetails.FullName.ToLower().Contains(lower) ||
                    s.StudentPersonalDetails.EmailAddress.ToLower().Contains(lower) ||
                    s.StudentPersonalDetails.MobileWhatsapp.ToLower().Contains(lower));
            }

            // Sorting
            query = sortOrder.ToLower() == "desc"
                ? query.OrderByDescending(e => EF.Property<object>(e, sortBy))
                : query.OrderBy(e => EF.Property<object>(e, sortBy));

            // Pagination
            query = query.Skip(offset).Take(limit);

            var result = await query.ToListAsync();
            _logger.LogInformation("Student filter query returned {Count} records", result.Count);

            return result;
        }

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> StudentConfirm(Guid id)
        {
            var entity = await _dbContext.Students.FindAsync(id);
            if (entity == null)
            {
                return CommonResponse<bool>
                    .FailureResponse("Student not found");
            }

            if (!entity.IsActive)
            {
                return CommonResponse<bool>
                    .FailureResponse("Student is inactive");
            }

            entity.UpdatedAt = DateTime.UtcNow;
            entity.RegistrationStatus = RegistrationStatus.Submitted;

            _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();

            return CommonResponse<bool>
        .SuccessResponse("Student confirmed successfully", true);
        }

        /// <summary>
        /// update the registration status.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<CommonResponse<bool>> StudentApprove(Guid id)
        {
            var entity = await _dbContext.Students.FindAsync(id);
            if (entity == null)
            {
                return CommonResponse<bool>
                    .FailureResponse("Student not found");
            }

            if (!entity.IsActive)
            {
                return CommonResponse<bool>
                    .FailureResponse("Student is inactive");
            }

            entity.UpdatedAt = DateTime.UtcNow;
            entity.RegistrationStatus = RegistrationStatus.Approved;

            _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();

            return CommonResponse<bool>
        .SuccessResponse("Student approved successfully", true);
        }
    }
}
