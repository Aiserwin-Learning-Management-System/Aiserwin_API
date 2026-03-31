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
            var existing = await _db.TeacherRegistrations.FirstOrDefaultAsync(x => x.Id == entity.Id);
            if (existing == null) return null;

            // update allowed fields
            existing.FullName = entity.FullName;
            existing.EmailAddress = entity.EmailAddress;
            existing.DateOfBirth = entity.DateOfBirth;
            existing.MobileNumber = entity.MobileNumber;
            existing.Address = entity.Address;
            existing.Gender = entity.Gender;
            existing.MaritalStatus = entity.MaritalStatus;
            existing.IdProofType = entity.IdProofType;
            existing.IdProofNumber = entity.IdProofNumber;
            existing.ComputerLiteracy = entity.ComputerLiteracy;
            existing.HighestQualification = entity.HighestQualification;
            existing.SalaryStructure = entity.SalaryStructure;
            existing.PaymentCycle = entity.PaymentCycle;
            existing.ContractDuration = entity.ContractDuration;
            existing.ReportingManager = entity.ReportingManager;
            existing.PhotoPath = entity.PhotoPath;
            existing.IdCardPath = entity.IdCardPath;
            existing.IsTermsAccepted = entity.IsTermsAccepted;
            existing.IsDeclarationAccepted = entity.IsDeclarationAccepted;
            existing.WorkMode = entity.WorkMode;
            existing.EmploymentTypeId = entity.EmploymentTypeId;
            existing.UpdatedAt = DateTime.UtcNow;

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
