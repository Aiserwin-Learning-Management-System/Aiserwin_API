using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Repository implementation for <see cref="TeacherRegistration"/> persistence operations.
    /// </summary>
    public sealed class TeacherRegistrationRepository : ITeacherRegistrationRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="TeacherRegistrationRepository"/> class.
        /// </summary>
        /// <param name="db">Application database context.</param>
        public TeacherRegistrationRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <summary>
        /// Adds the specified <see cref="TeacherRegistration"/> to the database.
        /// </summary>
        /// <param name="entity">The entity to add.</param>
        /// <returns>The created entity.</returns>
        public async Task<TeacherRegistration> AddAsync(TeacherRegistration entity)
        {
            _db.Set<TeacherRegistration>().Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Retrieves all teacher registrations.
        /// </summary>
        /// <returns>Read-only list of <see cref="TeacherRegistration"/> entities.</returns>
        public async Task<IReadOnlyList<TeacherRegistration>> GetAllAsync()
        {
            return await _db.Set<TeacherRegistration>()
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Returns a paged list of teacher registrations matching the provided filter.
        /// </summary>
        /// <param name="request">Filter request.</param>
        /// <returns>Paged result of <see cref="TeacherRegistration"/> entities.</returns>
        public async Task<Winfocus.LMS.Application.DTOs.Common.PagedResult<TeacherRegistration>> GetFilteredAsync(Winfocus.LMS.Application.DTOs.Teacher.TeacherFilterRequest request)
        {
            var query = _db.Set<TeacherRegistration>()
                .AsNoTracking()
                .AsQueryable();

            if (request.CountryId.HasValue && request.CountryId != Guid.Empty)
                query = query.Where(t => t.CountryId == request.CountryId.Value);

            if (request.StateId.HasValue && request.StateId != Guid.Empty)
                query = query.Where(t => t.StateId == request.StateId.Value);

            if (request.StartDate.HasValue)
                query = query.Where(t => t.CreatedAt >= request.StartDate.Value);

            if (request.EndDate.HasValue)
                query = query.Where(t => t.CreatedAt <= request.EndDate.Value);

            if (request.Status.HasValue)
                query = query.Where(t => t.Status == request.Status.Value);

            if (!string.IsNullOrWhiteSpace(request.SearchText))
            {
                var s = request.SearchText.Trim().ToLower();
                query = query.Where(t => t.FullName.ToLower().Contains(s)
                    || t.EmailAddress.ToLower().Contains(s)
                    || t.EmployeeId.ToLower().Contains(s));
            }

            var total = await query.CountAsync();

            bool isDesc = request.SortOrder?.ToLower() == "desc";
            query = request.SortBy?.ToLower() switch
            {
                "fullname" => isDesc ? query.OrderByDescending(t => t.FullName) : query.OrderBy(t => t.FullName),
                "createdat" => isDesc ? query.OrderByDescending(t => t.CreatedAt) : query.OrderBy(t => t.CreatedAt),
                "employeeid" => isDesc ? query.OrderByDescending(t => t.EmployeeId) : query.OrderBy(t => t.EmployeeId),
                _ => query.OrderBy(t => t.CreatedAt)
            };

            var items = await query.Skip(request.Offset).Take(request.Limit).ToListAsync();

            return new Winfocus.LMS.Application.DTOs.Common.PagedResult<TeacherRegistration>(items, total, request.Limit, request.Offset);
        }

        /// <summary>
        /// Marks the teacher registration as confirmed (keeps status as Pending if already set).
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>Operation result.</returns>
        public async Task<Winfocus.LMS.Application.DTOs.CommonResponse<bool>> TeacherConfirm(Guid id)
        {
            var existing = await _db.Set<TeacherRegistration>().FindAsync(id);
            if (existing == null)
                return Winfocus.LMS.Application.DTOs.CommonResponse<bool>.FailureResponse("Teacher not found");

            if (!existing.IsActive)
                return Winfocus.LMS.Application.DTOs.CommonResponse<bool>.FailureResponse("Teacher is inactive");

            existing.UpdatedAt = DateTime.UtcNow;
            // Set to Pending (no explicit Submitted state in TeacherStatus enum)
            existing.Status = Winfocus.LMS.Domain.Enums.TeacherStatus.Pending;

            _db.Set<TeacherRegistration>().Update(existing);
            await _db.SaveChangesAsync();

            return Winfocus.LMS.Application.DTOs.CommonResponse<bool>.SuccessResponse("Teacher confirmed successfully", true);
        }

        /// <summary>
        /// Marks the teacher registration as approved.
        /// </summary>
        /// <param name="id">Teacher identifier.</param>
        /// <returns>Operation result.</returns>
        public async Task<Winfocus.LMS.Application.DTOs.CommonResponse<bool>> TeacherApprove(Guid id)
        {
            var existing = await _db.Set<TeacherRegistration>().FindAsync(id);
            if (existing == null)
                return Winfocus.LMS.Application.DTOs.CommonResponse<bool>.FailureResponse("Teacher not found");

            if (!existing.IsActive)
                return Winfocus.LMS.Application.DTOs.CommonResponse<bool>.FailureResponse("Teacher is inactive");

            existing.UpdatedAt = DateTime.UtcNow;
            existing.Status = Winfocus.LMS.Domain.Enums.TeacherStatus.Approved;

            _db.Set<TeacherRegistration>().Update(existing);
            await _db.SaveChangesAsync();

            return Winfocus.LMS.Application.DTOs.CommonResponse<bool>.SuccessResponse("Teacher approved successfully", true);
        }

        /// <summary>
        /// Gets a teacher registration by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The matching <see cref="TeacherRegistration"/>, or <c>null</c> if not found.</returns>
        public async Task<TeacherRegistration?> GetByIdAsync(Guid id)
        {
            return await _db.Set<TeacherRegistration>()
                .AsNoTracking()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <summary>
        /// Updates fields of an existing teacher registration and persists the changes.
        /// </summary>
        /// <param name="entity">Entity containing updated values (must include Id).</param>
        /// <returns>The updated entity, or <c>null</c> when the entity does not exist.</returns>
        public async Task<TeacherRegistration?> UpdateAsync(TeacherRegistration entity)
        {
            var existing = await _db.Set<TeacherRegistration>()
                .Include(x => x.ProfessionalDetail)
                .Include(x => x.Schedule)
                .Include(x => x.Documents)
                .Include(x => x.WorkHistory)
                .FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existing == null) return null;

            // update allowed fields
            existing.FullName = entity.FullName;
            existing.EmailAddress = entity.EmailAddress;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.MobileNumber = entity.MobileNumber;
            existing.ResidentialAddress = entity.ResidentialAddress;
            existing.Gender = entity.Gender;
            existing.IsTermsAccepted = entity.IsTermsAccepted;
            existing.WorkMode = entity.WorkMode;
            existing.EmploymentTypeId = entity.EmploymentTypeId;
            existing.UpdatedAt = DateTime.UtcNow;

            // Professional detail
            if (entity.ProfessionalDetail != null)
            {
                if (existing.ProfessionalDetail == null)
                {
                    existing.ProfessionalDetail = entity.ProfessionalDetail;
                    existing.ProfessionalDetail.TeacherId = existing.Id;
                }
                else
                {
                    existing.ProfessionalDetail.HighestQualification = entity.ProfessionalDetail.HighestQualification;
                    existing.ProfessionalDetail.TotalTeachingExperience = entity.ProfessionalDetail.TotalTeachingExperience;
                    existing.ProfessionalDetail.HasOnlineTeachingExperience = entity.ProfessionalDetail.HasOnlineTeachingExperience;
                    existing.ProfessionalDetail.HasOfflineTeachingExperience = entity.ProfessionalDetail.HasOfflineTeachingExperience;
                    existing.ProfessionalDetail.IsAvailableForDemoClass = entity.ProfessionalDetail.IsAvailableForDemoClass;
                    existing.ProfessionalDetail.ComputerLiteracy = entity.ProfessionalDetail.ComputerLiteracy;
                }
            }

            // Schedule
            if (entity.Schedule != null)
            {
                if (existing.Schedule == null)
                {
                    existing.Schedule = entity.Schedule;
                    // ensure schedule has id set when added by EF; TeacherId FK should be set
                    existing.Schedule.TeacherId = existing.Id;
                }
                else
                {
                    // replace availabilities - remove old and add new
                    if (existing.Schedule.Availabilities != null && existing.Schedule.Availabilities.Count > 0)
                    {
                        _db.Set<TeacherAvailability>().RemoveRange(existing.Schedule.Availabilities);
                    }

                    if (entity.Schedule.Availabilities != null)
                    {
                    foreach (var a in entity.Schedule.Availabilities)
                    {
                        // when adding new availabilities, set ScheduleId to the existing schedule's id
                        a.ScheduleId = existing.Schedule.Id;
                        _db.Set<TeacherAvailability>().Add(a);
                    }
                    }
                }
            }

            // Documents
            if (entity.Documents != null)
            {
                if (existing.Documents == null)
                {
                    existing.Documents = entity.Documents;
                    existing.Documents.TeacherId = existing.Id;
                }
                else
                {
                    existing.Documents.PhotoPath = entity.Documents.PhotoPath;
                    existing.Documents.ProofType = entity.Documents.ProofType;
                    existing.Documents.ProofNumber = entity.Documents.ProofNumber;
                }
            }

            // Work history - replace existing entries with provided ones
            if (entity.WorkHistory != null)
            {
                // remove old
                if (existing.WorkHistory != null && existing.WorkHistory.Count > 0)
                {
                    _db.Set<TeacherWorkHistory>().RemoveRange(existing.WorkHistory);
                }

                // attach new
                foreach (var wh in entity.WorkHistory)
                {
                    wh.TeacherRegistrationId = existing.Id;
                    _db.Set<TeacherWorkHistory>().Add(wh);
                }
            }

            _db.TeacherRegistrations.Update(existing);
            await _db.SaveChangesAsync();

            return existing;
        }

        /// <summary>
        /// Marks the teacher registration as deleted (soft delete).
        /// </summary>
        /// <param name="id">Identifier of the teacher registration to soft delete.</param>
        /// <returns><c>true</c> when the entity was marked deleted; otherwise <c>false</c>.</returns>
        public async Task<bool> SoftDeleteAsync(Guid id)
        {
            var existing = await _db.TeacherRegistrations.FirstOrDefaultAsync(x => x.Id == id);
            if (existing == null) return false;

            existing.IsActive = false;
            existing.IsDeleted = true;
            existing.UpdatedAt = DateTime.UtcNow;

            _db.TeacherRegistrations.Update(existing);
            await _db.SaveChangesAsync();

            return true;
        }
    }
}
