namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Repository implementation for <see cref="QuestionConfiguration"/> data access.
    /// </summary>
    public class QuestionConfigurationRepository : IQuestionConfigurationRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionConfigurationRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public QuestionConfigurationRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<int> GetNextSequenceAsync(
            Guid syllabusId,
            Guid academicYearId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid questionTypeId)
        {
            int maxSequence = await _context.QuestionConfigurations
                .Where(x =>
                    x.SyllabusId == syllabusId &&
                    x.AcademicYearId == academicYearId &&
                    x.GradeId == gradeId &&
                    x.SubjectId == subjectId &&
                    x.UnitId == unitId &&
                    x.ChapterId == chapterId &&
                    x.QuestionTypeId == questionTypeId &&
                    !x.IsDeleted)
                .MaxAsync(x => (int?)x.SequenceNumber) ?? 0;

            return maxSequence + 1;
        }

        /// <inheritdoc />
        public async Task<bool> CodeExistsAsync(string questionCode)
        {
            return await _context.QuestionConfigurations
                .AnyAsync(x => x.QuestionCode == questionCode && !x.IsDeleted);
        }

        /// <inheritdoc />
        public async Task<QuestionConfiguration> AddAsync(QuestionConfiguration entity)
        {
            await _context.QuestionConfigurations.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<QuestionConfiguration?> GetByIdAsync(Guid id)
        {
            return await Query()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public IQueryable<QuestionConfiguration> Query()
        {
            return _context.QuestionConfigurations
                .Include(x => x.Syllabus)
                .Include(x => x.AcademicYear)
                .Include(x => x.Grade)
                .Include(x => x.Subject)
                .Include(x => x.Unit)
                .Include(x => x.Chapter)
                .Include(x => x.ResourceType)
                .Include(x => x.QuestionType)
                .AsQueryable();
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            QuestionConfiguration? entity = await _context.QuestionConfigurations
                .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);

            if (entity == null)
            {
                return false;
            }

            entity.IsDeleted = true;
            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;
            entity.UpdatedBy = userId;
            await _context.SaveChangesAsync();
            return true;
        }
    }
}
