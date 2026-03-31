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
            _db.TeacherRegistrations.Add(entity);
            await _db.SaveChangesAsync();
            return entity;
        }

        /// <summary>
        /// Retrieves all teacher registrations.
        /// </summary>
        /// <returns>Read-only list of <see cref="TeacherRegistration"/> entities.</returns>
        public async Task<IReadOnlyList<TeacherRegistration>> GetAllAsync()
        {
            return await _db.TeacherRegistrations
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets a teacher registration by its identifier.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>The matching <see cref="TeacherRegistration"/>, or <c>null</c> if not found.</returns>
        public async Task<TeacherRegistration?> GetByIdAsync(Guid id)
        {
            return await _db.TeacherRegistrations
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
            var existing = await _db.TeacherRegistrations
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
            existing.Address = entity.Address;
            existing.Gender = entity.Gender;
            existing.IsTermsAccepted = entity.IsTermsAccepted;
           // existing.IsDeclarationAccepted = entity.IsDeclarationAccepted;
            existing.WorkMode = entity.WorkMode;
            existing.EmploymentTypeId = entity.EmploymentTypeId;
            existing.UpdatedAt = DateTime.UtcNow;

            // Professional detail
            if (entity.ProfessionalDetail != null)
            {
                if (existing.ProfessionalDetail == null)
                {
                    existing.ProfessionalDetail = entity.ProfessionalDetail;
                    // domain uses TeacherId as FK
                    existing.ProfessionalDetail.TeacherId = existing.Id;
                }
                else
                {
                    existing.ProfessionalDetail.HighestQualification = entity.ProfessionalDetail.HighestQualification;
                    existing.ProfessionalDetail.University = entity.ProfessionalDetail.University;
                    existing.ProfessionalDetail.YearOfPassing = entity.ProfessionalDetail.YearOfPassing;
                    existing.ProfessionalDetail.HasTeachingCertification = entity.ProfessionalDetail.HasTeachingCertification;
                    existing.ProfessionalDetail.AdditionalCourses = entity.ProfessionalDetail.AdditionalCourses;
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
                    existing.Documents.IdCardPath = entity.Documents.IdCardPath;
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
