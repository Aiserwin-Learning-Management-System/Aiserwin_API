namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// FeeRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IFeeRepository" />
    public sealed class FeeRepository : IFeeRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeRepository"/> class.
        /// </summary>
        /// <param name="context">The context.</param>
        public FeeRepository(AppDbContext context)
        {
            _context = context;
        }

        // ── FeePlan ──

        /// <inheritdoc/>
        public async Task AddAsync(FeePlan feePlan)
            => await _context.FeePlans.AddAsync(feePlan);

        /// <inheritdoc/>
        public async Task<List<FeePlan>> GetAllAsync()
            => await _context.FeePlans.Where(x => !x.IsDeleted)
                .Include(x => x.Discounts.Where(d => !d.IsDeleted && d.IsActive))
                .Include(x => x.Course).ThenInclude(c => c.Stream)
                .Include(x => x.Course).ThenInclude(c => c.Grade)
                    .ThenInclude(g => g.Syllabus).ThenInclude(s => s.Center)
                    .ThenInclude(c => c.State)
                .AsNoTracking().ToListAsync();

        /// <inheritdoc/>
        public async Task<FeePlan?> GetByIdAsync(Guid id)
            => await _context.FeePlans
                .Include(x => x.Discounts.Where(d => !d.IsDeleted && d.IsActive))
                .Include(x => x.Course).ThenInclude(c => c.Stream)
                    .ThenInclude(s => s.Grade).ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center).ThenInclude(c => c.State)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<FeePlan?> GetFeePlanByIdAsync(Guid feePlanId)
            => await _context.FeePlans
                .FirstOrDefaultAsync(x => x.Id == feePlanId && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<FeePlan?> GetFeePlanWithDiscountsAsync(Guid feePlanId)
            => await _context.FeePlans
                .Include(x => x.Discounts.Where(d => !d.IsDeleted && d.IsActive))
                .Include(x => x.Course)
                .FirstOrDefaultAsync(x => x.Id == feePlanId && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.FeePlans.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;
            await _context.SaveChangesAsync();
            return true;
        }

        /// <inheritdoc/>
        public IQueryable<FeePlan> Query()
            => _context.FeePlans.Where(x => !x.IsDeleted)
                .Include(x => x.Discounts.Where(d => !d.IsDeleted && d.IsActive))
                .Include(x => x.Course).ThenInclude(c => c.Stream)
                    .ThenInclude(s => s.Grade).ThenInclude(g => g.Syllabus)
                    .ThenInclude(s => s.Center).ThenInclude(c => c.State)
                .AsNoTracking();

        /// <inheritdoc/>
        public void AddDiscount(FeePlanDiscount discount)
            => _context.FeePlanDiscount.Add(discount);

        /// <inheritdoc/>
        public void RemoveDiscount(FeePlanDiscount discount)
        {
            discount.IsActive = false;
            discount.IsDeleted = true;
            discount.UpdatedAt = DateTime.UtcNow;
            _context.FeePlanDiscount.Update(discount);
        }

        // ── Student / Course ──

        /// <inheritdoc/>
        public async Task<Student?> GetStudentWithCoursesAsync(Guid studentId)
            => await _context.Students
                .Include(x => x.StudentAcademicCouses)
                .Include(x => x.AcademicDetails)
                    .ThenInclude(a => a.Grade).ThenInclude(g => g.Syllabus)
                .Include(x => x.StudentPersonalDetails)
                .FirstOrDefaultAsync(x => x.Id == studentId && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<List<Course>> GetCoursesByGradeAsync(Guid gradeId)
            => await _context.Courses
                .Include(c => c.Stream)
                .Include(c => c.FeePlans.Where(fp => !fp.IsDeleted))
                    .ThenInclude(fp => fp.Discounts.Where(d => !d.IsDeleted && d.IsActive))
                .Where(c => c.Stream.GradeId == gradeId && c.IsActive && !c.IsDeleted)
                .ToListAsync();

        // ── StudentCourseDiscount ──

        /// <inheritdoc/>
        public async Task<List<StudentCourseDiscount>> GetStudentCourseDiscountsAsync(
            Guid studentId)
            => await _context.StudentCourseDiscounts
                .Where(x => x.StudentId == studentId && x.IsActive && !x.IsDeleted)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<StudentCourseDiscount>> GetStudentCourseDiscountsAsync(
            Guid studentId, Guid courseId)
            => await _context.StudentCourseDiscounts
                .Where(x => x.StudentId == studentId
                          && x.CourseId == courseId
                          && x.IsActive && !x.IsDeleted)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task AddStudentCourseDiscountsAsync(
            IEnumerable<StudentCourseDiscount> discounts)
            => await _context.StudentCourseDiscounts.AddRangeAsync(discounts);

        /// <inheritdoc/>
        public async Task RemoveStudentCourseDiscountsAsync(Guid studentId, Guid courseId)
        {
            var existing = await _context.StudentCourseDiscounts
                .Where(x => x.StudentId == studentId
                          && x.CourseId == courseId
                          && !x.IsDeleted)
                .ToListAsync();

            foreach (var e in existing)
            {
                e.IsActive = false;
                e.IsDeleted = true;
            }
        }

        // ── FeePlanDiscount lookup ──

        /// <inheritdoc/>
        public async Task<List<FeePlanDiscount>> GetFeePlanDiscountsByIdsAsync(
            IEnumerable<Guid> ids)
            => await _context.FeePlanDiscount
                .Include(d => d.FeePlan)
                .Where(d => ids.Contains(d.Id) && !d.IsDeleted && d.IsActive)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<List<FeePlanDiscount>> GetDiscountsByCourseAsync(Guid courseId)
            => await _context.FeePlanDiscount
                .Include(d => d.FeePlan)
                .Where(d => d.FeePlan.CourseId == courseId
                          && d.IsActive && !d.IsDeleted
                          && d.FeePlan.IsActive && !d.FeePlan.IsDeleted)
                .ToListAsync();

        // ── StudentFeeSelection ──

        /// <inheritdoc/>
        public async Task AddStudentFeeSelectionAsync(StudentFeeSelection selection)
            => await _context.StudentFeeSelections.AddAsync(selection);

        /// <inheritdoc/>
        public async Task<StudentFeeSelection?> GetSelectionByIdAsync(Guid selectionId)
            => await _context.StudentFeeSelections
                .Include(x => x.Course)
                .Include(x => x.FeePlan)
                .FirstOrDefaultAsync(x => x.Id == selectionId && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<StudentFeeSelection?> GetSelectionWithDetailsAsync(
            Guid selectionId)
            => await _context.StudentFeeSelections
                .Include(x => x.AppliedDiscounts)
                .Include(x => x.Installments)
                .Include(x => x.Course)
                .Include(x => x.FeePlan)
                .FirstOrDefaultAsync(x => x.Id == selectionId && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<List<StudentFeeSelection>> GetSelectionsByStudentAsync(
            Guid studentId)
            => await _context.StudentFeeSelections
                .Include(x => x.Course)
                .Include(x => x.FeePlan)
                .Include(x => x.AppliedDiscounts)
                .Include(x => x.Installments)
                .Where(x => x.StudentId == studentId
                          && x.IsActive && !x.IsDeleted)
                .ToListAsync();

        /// <inheritdoc/>
        public async Task<bool> HasConfirmedSelectionForCourseAsync(
            Guid studentId, Guid courseId)
            => await _context.StudentFeeSelections
                .AnyAsync(x => x.StudentId == studentId
                             && x.CourseId == courseId
                             && x.IsActive && !x.IsDeleted);

        /// <inheritdoc/>
        public IQueryable<StudentFeeSelection> QuerySelections()
            => _context.StudentFeeSelections.Where(x => !x.IsDeleted)
                .Include(x => x.Student).ThenInclude(s => s.StudentPersonalDetails)
                .Include(x => x.Course)
                .Include(x => x.FeePlan)
                .AsNoTracking();

        // ── StudentInstallment ──

        /// <inheritdoc/>
        public async Task<StudentInstallment?> GetInstallmentByIdAsync(Guid installmentId)
            => await _context.StudentInstallments
                .FirstOrDefaultAsync(x => x.Id == installmentId && !x.IsDeleted);

        /// <inheritdoc/>
        public async Task<List<StudentInstallment>> GetInstallmentsBySelectionAsync(
            Guid selectionId)
            => await _context.StudentInstallments
                .Where(x => x.StudentFeeSelectionId == selectionId && !x.IsDeleted)
                .OrderBy(x => x.InstallmentNo)
                .ToListAsync();

        // ── Common ──

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
            => await _context.SaveChangesAsync();
    }
}
