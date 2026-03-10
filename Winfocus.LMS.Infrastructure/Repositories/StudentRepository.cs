using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;
using Winfocus.LMS.Application.DTOs.Students;
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
            return await _dbContext.Students.Where(x => !x.IsDeleted)
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

        .FirstOrDefaultAsync(x => x.Id == id && x.IsActive == true && !x.IsDeleted);
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

            var codes = await _dbContext.StudentAcademicDetails
                             .Where(a => a.Id == student.StudentAcademicDetailsId)
                             .Select(a => new
                             {
                                 a.Stream.StreamCode,
                                 a.Center.CenterCode
                             }).FirstOrDefaultAsync();

            var streamCode = codes?.StreamCode;
            var centerCode = codes?.CenterCode;

            var year = DateTime.UtcNow.ToString("yy");
            var register_prefix = $"{centerCode}-{year}-{streamCode}";

            if (string.IsNullOrEmpty(maxValue))
            {
                maxValue = "1";
                string formattedNumber = maxValue.ToString().PadLeft(4, '0');
                student.RegistrationNumber = $"{register_prefix}-{formattedNumber}";
            }
            else
            {
                    string formattedNumber = maxValue.ToString().PadLeft(4, '0');
                    string digitsOnly = Regex.Replace(formattedNumber, "[^0-9]", "");
                    digitsOnly = digitsOnly.Substring(digitsOnly.Length - 4);
                    int val = int.Parse(digitsOnly);
                    val++;
                    string num = val.ToString().PadLeft(4, '0');
                    student.RegistrationNumber = $"{register_prefix}-{num}";
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
            entity.IsDeleted = true;

            _dbContext.Students.Update(entity);
            await _dbContext.SaveChangesAsync();
            return CommonResponse<bool>
       .SuccessResponse("Student successfully deleted", true);
        }

        /// <summary>
        /// Gets the filtered asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>Student.</returns>
        public async Task<PagedResult<Student>> GetFilteredAsync(StudentFilterRequest request)
        {
            // 1. Start with the query and include all necessary relationships
            var query = _dbContext.Students.Where(x => !x.IsDeleted)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.Country)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.State)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.ModeOfStudy)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.Center)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.Syllabus)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.Grade)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.Stream)
         .Include(s => s.AcademicDetails)
             .ThenInclude(ad => ad.Subject)
         .Include(s => s.StudentPersonalDetails)
         .Include(s => s.StudentDocuments)
         .Include(s => s.StudentAcademicCouses)
             .ThenInclude(sc => sc.Course)
         .AsNoTracking()
         .AsQueryable();

            if (request.CountryId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.CountryId == request.CountryId);
            }

            if (request.StateId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.StateId == request.StateId);
            }

            if (request.ModeId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.ModeOfStudyId == request.ModeId);
            }

            if (request.CentreId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.CenterId == request.CentreId);
            }

            if (request.GradeId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.GradeId == request.GradeId);
            }

            if (request.BatchId.HasValue)
            {
                query = query.Where(s => s.AcademicDetails.AcademicYearId == request.BatchId);
            }

            if (request.RegistrationStatus.HasValue)
            {
                query = query.Where(s => s.RegistrationStatus == request.RegistrationStatus);
            }

            if (request.StartDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt >= request.StartDate.Value);
            }

            if (request.EndDate.HasValue)
            {
                query = query.Where(s => s.CreatedAt <= request.EndDate.Value);
            }

            if (!string.IsNullOrEmpty(request.SearchText))
            {
                var search = request.SearchText.ToLower();
                query = query.Where(s =>
                    s.StudentPersonalDetails.FullName.ToLower().Contains(search) ||
                    s.StudentPersonalDetails.EmailAddress.ToLower().Contains(search) ||
                    s.RegistrationNumber.ToLower().Contains(search));
            }

            // 3. Count Total Records BEFORE Pagination
            var totalCount = await query.CountAsync();

            // 4. Apply Sorting
            bool isDesc = request.SortOrder?.ToLower() == "desc";

            query = request.SortBy?.ToLower() switch
            {
                "fullname" => isDesc ? query.OrderByDescending(s => s.StudentPersonalDetails.FullName)
                                     : query.OrderBy(s => s.StudentPersonalDetails.FullName),
                "createdat" => isDesc ? query.OrderByDescending(s => s.CreatedAt)
                                      : query.OrderBy(s => s.CreatedAt),
                "registrationnumber" => isDesc ? query.OrderByDescending(s => s.RegistrationNumber)
                                               : query.OrderBy(s => s.RegistrationNumber),
                _ => query.OrderBy(s => s.CreatedAt)
            };

            // 5. Apply Pagination
            var items = await query
                .Skip(request.Offset)
                .Take(request.Limit)
                .ToListAsync();

            return new PagedResult<Student>(items, totalCount, request.Limit, request.Offset);
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
