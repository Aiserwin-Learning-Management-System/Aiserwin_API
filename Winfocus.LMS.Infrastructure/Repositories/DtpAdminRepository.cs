using Microsoft.EntityFrameworkCore;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// DtpAdminRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IDtpAdminRepository" />
    public sealed class DtpAdminRepository : IDtpAdminRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="DtpAdminRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public DtpAdminRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<RegistrationForm?> GetDtpRegistrationFormAsync()
        {
            var dtpCategory = await GetDtpCategoryAsync();
            if (dtpCategory == null) return null;

            return await _db.RegistrationForms
                .Include(rf => rf.FormFields)
                    .ThenInclude(rff => rff.FormField)
                .AsNoTracking()
                .Where(rf => rf.StaffCategoryId == dtpCategory.Id
                    && rf.IsActive)
                .FirstOrDefaultAsync();
        }

        /// <inheritdoc/>
        public async Task<List<StaffRegistration>> GetDtpRegistrationsAsync()
        {
            var dtpCategory = await GetDtpCategoryAsync();
            if (dtpCategory == null) return new List<StaffRegistration>();

            return await _db.StaffRegistrations
                .Include(sr => sr.StaffCategory)
                .Include(sr => sr.Values)
                    .ThenInclude(v => v.FormField)
                .AsNoTracking()
                .Where(sr => sr.StaffCategoryId == dtpCategory.Id)
                .OrderByDescending(sr => sr.CreatedAt)
                .ToListAsync();
        }

        /// <inheritdoc/>
        public async Task<StaffRegistration?> GetRegistrationByIdAsync(Guid registrationId)
        {
            return await _db.StaffRegistrations
                .Include(sr => sr.StaffCategory)
                .Include(sr => sr.Values)
                    .ThenInclude(v => v.FormField)
                .FirstOrDefaultAsync(sr => sr.Id == registrationId);
        }

        /// <inheritdoc/>
        public async Task<StaffCategory?> GetDtpCategoryAsync()
        {
            return await _db.StaffCategories
                .AsNoTracking()
                .FirstOrDefaultAsync(sc =>
                    sc.Name.Contains("DTP") && sc.IsActive);
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
        {
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task<StaffRegistration?> GetRegistrationWithDetailsAsync(Guid id)
        {
            return await _db.StaffRegistrations

                // VALUES
                .Include(r => r.Values)
                    .ThenInclude(v => v.FormField)
                        .ThenInclude(f => f.FieldOptions)

                // FORM
                .Include(r => r.RegistrationForm)
                    .ThenInclude(f => f.FormGroups)
                        .ThenInclude(g => g.FieldGroup)

                .Include(r => r.RegistrationForm)
                    .ThenInclude(f => f.FormFields)
                        .ThenInclude(ff => ff.FormField)
                            .ThenInclude(f => f.FieldOptions)

                // CATEGORY
                .Include(r => r.StaffCategory)

                .FirstOrDefaultAsync(r => r.Id == id);
        }

        /// <inheritdoc/>
        public async Task<List<TaskAssignment>> GetTasksByOperatorIdAsync(Guid registrationId)
        {
            return await _db.TaskAssignments
                .Where(t => t.OperatorId == registrationId)
                .ToListAsync();
        }
    }
}
