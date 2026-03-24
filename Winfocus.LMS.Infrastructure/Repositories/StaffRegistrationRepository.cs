namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// EF Core implementation of <see cref="IStaffRegistrationRepository"/>.
    /// </summary>
    public sealed class StaffRegistrationRepository : IStaffRegistrationRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="StaffRegistrationRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public StaffRegistrationRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<RegistrationForm?> GetFormWithFieldsAsync(Guid formId)
        {
            return await _db.RegistrationForms
                .Include(rf => rf.StaffCategory)
                .Include(rf => rf.FormGroups)
                    .ThenInclude(rfg => rfg.FieldGroup)
                .Include(rf => rf.FormFields)
                    .ThenInclude(rff => rff.FormField)
                        .ThenInclude(ff => ff.FieldOptions)
                .AsNoTracking()
                .FirstOrDefaultAsync(rf => rf.Id == formId);
        }

        /// <inheritdoc/>
        public async Task<StaffRegistration> AddAsync(StaffRegistration registration)
        {
            using var transaction = await _db.Database.BeginTransactionAsync();
            try
            {
                _db.StaffRegistrations.Add(registration);
                await _db.SaveChangesAsync();
                await transaction.CommitAsync();
                return registration;
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }

        /// <inheritdoc/>
        public async Task<StaffRegistration?> GetByIdWithDetailsAsync(Guid id)
        {
            return await _db.StaffRegistrations
                .Include(sr => sr.RegistrationForm)
                .Include(sr => sr.StaffCategory)
                .Include(sr => sr.Values)
                    .ThenInclude(v => v.FormField)
                .AsNoTracking()
                .FirstOrDefaultAsync(sr => sr.Id == id);
        }

        /// <summary>
        /// UpdateStatusAsync.
        /// </summary>
        /// <param name="registration"></param>
        /// <returns></returns>
        public async Task UpdateStatusAsync(StaffRegistration registration)
        {
            _db.StaffRegistrations.Update(registration);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public IQueryable<StaffRegistration> Query()
        {
            return _db.StaffRegistrations
                .Include(sr => sr.RegistrationForm)
                .Include(sr => sr.StaffCategory)
                .AsNoTracking();
        }

        /// <inheritdoc />
        public async Task<StaffRegistration?> GetByUserIdAsync(Guid userId)
        {
            return await _db.StaffRegistrations
                .Include(x => x.StaffCategory)
                .Include(x => x.Values)
                .FirstOrDefaultAsync(x => x.UserId == userId && x.IsActive && !x.IsDeleted);
        }
    }
}
