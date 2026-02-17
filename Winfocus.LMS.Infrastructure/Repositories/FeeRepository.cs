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

        /// <summary>
        /// Gets the student with courses asynchronous.
        /// </summary>
        /// <param name="studentId">The student identifier.</param>
        /// <returns>
        /// Task&lt;Student?&gt;.
        /// </returns>
        public async Task<Student?> GetStudentWithCoursesAsync(Guid studentId)
        {
            return await _context.Students
                .Include(x => x.StudentAcademicCouses)
                .FirstOrDefaultAsync(x => x.Id == studentId);
        }

        /// <summary>
        /// Gets the courses by grade asynchronous.
        /// </summary>
        /// <param name="gradeId">The grade identifier.</param>
        /// <returns>
        /// Task&lt;List&lt;Course&gt;&gt;.
        /// </returns>
        public async Task<List<Course>> GetCoursesByGradeAsync(Guid gradeId)
        {
            return await _context.Courses
                .Include(c => c.Stream)
                .Include(c => c.Subject)
                .Include(c => c.FeePlans)
                .Where(c => c.Stream.GradeId == gradeId && c.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// Adds the student fee selection asynchronous.
        /// </summary>
        /// <param name="entity">The entity.</param>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task AddStudentFeeSelectionAsync(StudentFeeSelection entity)
        {
            await _context.StudentFeeSelections.AddAsync(entity);
        }

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>
        /// Task.
        /// </returns>
        public async Task SaveChangesAsync()
        {
            await _context.SaveChangesAsync();
        }
    }
}
