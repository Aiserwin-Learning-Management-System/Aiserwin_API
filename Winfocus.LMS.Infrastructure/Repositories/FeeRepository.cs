namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// FeeRepository – EF Core implementation of IFeeRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IFeeRepository" />
    public sealed class FeeRepository : IFeeRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="FeeRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public FeeRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Gets the student with courses asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;Student?&gt;.</returns>
        public async Task<Student?> GetStudentWithCoursesAsync(Guid studentId)
        {
            return await _context.Students
                .Include(x => x.StudentAcademicCouses)
                .Include(x => x.AcademicDetails)
                .FirstOrDefaultAsync(x => x.Id == studentId && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the courses by grade asynchronous.
        /// </summary>
        /// <param name="gradeId">The grade identifier.</param>
        /// <returns>Task&lt;List&lt;Course&gt;&gt;.</returns>
        public async Task<List<Course>> GetCoursesByGradeAsync(Guid gradeId)
        {
            return await _context.Courses
                .Include(c => c.Stream)
                .Include(c => c.FeePlans)
                .Where(c => c.Stream.GradeId == gradeId && c.IsActive && !c.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Adds the student fee selection asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>Task.</returns>
        public async Task AddStudentFeeSelectionAsync(StudentFeeSelection entity)
        {
            await _context.StudentFeeSelections.AddAsync(entity);
        }

        /// <summary>
        /// Gets the student fee selection by identifier asynchronous.
        /// </summary>
        /// <param name="selectionId">The selection identifier.</param>
        /// <returns>Task&lt;StudentFeeSelection?&gt;.</returns>
        public async Task<StudentFeeSelection?> GetStudentFeeSelectionByIdAsync(
            Guid selectionId)
        {
            return await _context.StudentFeeSelections
                .FirstOrDefaultAsync(x => x.Id == selectionId && !x.IsDeleted);
        }

        /// <summary>
        /// Gets all student fee selections for a student.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>Task&lt;List&lt;StudentFeeSelection&gt;&gt;.</returns>
        public async Task<List<StudentFeeSelection>> GetStudentFeeSelectionsByStudentAsync(
            Guid studentId)
        {
            return await _context.StudentFeeSelections
                .Where(x => x.StudentId == studentId && x.IsActive && !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Gets the fee plan by identifier asynchronous.
        /// </summary>
        /// <param name="feePlanId">The fee plan identifier.</param>
        /// <returns>Task&lt;FeePlan?&gt;.</returns>
        public async Task<FeePlan?> GetFeePlanByIdAsync(Guid feePlanId)
        {
            return await _context.FeePlans
                .FirstOrDefaultAsync(x => x.Id == feePlanId && !x.IsDeleted);
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>Task.</returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="feePlan">The feePlan.</param>
        /// <returns>feePlan.</returns>
        public async Task AddAsync(FeePlan feePlan)
        {
            await _context.FeePlans.AddAsync(feePlan);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>FeePlan.</returns>
        public async Task<FeePlan?> GetByIdAsync(Guid id)
        {
            return await _context.FeePlans
                .Include(x => x.Discounts)
                .Include(x => x.Course)
                .ThenInclude(x => x.Stream)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                 .ThenInclude(x => x.State)
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <returns>FeePlan.</returns>
        public async Task<List<FeePlan>> GetAllAsync()
        {
            return await _context.FeePlans.Where(x => !x.IsDeleted)
                .Include(x => x.Discounts)
                .Include(x => x.Installments)
                 .Include(x => x.Course)
                .ThenInclude(x => x.Stream)
                .Include(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                .ThenInclude(x => x.Center)
                 .ThenInclude(x => x.State)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="discount">The discount.</param>
        public void AddDiscount(FeePlanDiscount discount)
        {
            _context.FeePlanDiscount.Add(discount);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="discount">The discount.</param>
        public void RemoveDiscount(FeePlanDiscount discount)
        {
            _context.FeePlanDiscount.Remove(discount);
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.FeePlans.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = false;

            _context.FeePlans.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
