namespace Winfocus.LMS.Infrastructure.Repositories
{
    using System;
    using System.Collections.Generic;
    using System.Linq;
    using System.Threading.Tasks;
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Repository implementation for <see cref="QuestionTypeConfig"/> data access.
    /// </summary>
    public class QuestionTypeConfigRepository : IQuestionTypeConfigRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionTypeConfigRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public QuestionTypeConfigRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<QuestionTypeConfig> AddAsync(QuestionTypeConfig entity)
        {
            await _context.QuestionTypeConfigs.AddAsync(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<List<QuestionTypeConfig>> AddRangeAsync(List<QuestionTypeConfig> entities)
        {
            await _context.QuestionTypeConfigs.AddRangeAsync(entities);
            await _context.SaveChangesAsync();
            return entities;
        }

        /// <inheritdoc />
        public async Task<QuestionTypeConfig?> GetByIdAsync(Guid id)
        {
            return await Query()
                .FirstOrDefaultAsync(x => x.Id == id);
        }

        /// <inheritdoc />
        public async Task<QuestionTypeConfig> UpdateAsync(QuestionTypeConfig entity)
        {
            _context.QuestionTypeConfigs.Update(entity);
            await _context.SaveChangesAsync();
            return entity;
        }

        /// <inheritdoc />
        public async Task<bool> DeleteAsync(Guid id, Guid userId)
        {
            QuestionTypeConfig? entity = await _context.QuestionTypeConfigs
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

        /// <inheritdoc />
        public async Task<bool> IsDuplicateAsync(
            Guid syllabusId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid resourceTypeId,
            string name,
            Guid? excludeId = null)
        {
            IQueryable<QuestionTypeConfig> query = _context.QuestionTypeConfigs
                .Where(x =>
                    x.SyllabusId == syllabusId &&
                    x.GradeId == gradeId &&
                    x.SubjectId == subjectId &&
                    x.UnitId == unitId &&
                    x.ChapterId == chapterId &&
                    x.ResourceTypeId == resourceTypeId &&
                    x.Name.ToLower() == name.ToLower() &&
                    !x.IsDeleted);

            if (excludeId.HasValue)
            {
                query = query.Where(x => x.Id != excludeId.Value);
            }

            return await query.AnyAsync();
        }

        /// <inheritdoc />
        public async Task<List<QuestionTypeConfig>> GetByHierarchyAsync(
            Guid syllabusId,
            Guid gradeId,
            Guid subjectId,
            Guid unitId,
            Guid chapterId,
            Guid resourceTypeId)
        {
            return await _context.QuestionTypeConfigs
                .Where(x =>
                    x.SyllabusId == syllabusId &&
                    x.GradeId == gradeId &&
                    x.SubjectId == subjectId &&
                    x.UnitId == unitId &&
                    x.ChapterId == chapterId &&
                    x.ResourceTypeId == resourceTypeId &&
                    x.IsActive &&
                    !x.IsDeleted)
                .ToListAsync();
        }

        /// <inheritdoc />
        public IQueryable<QuestionTypeConfig> Query()
        {
            return _context.QuestionTypeConfigs
                .Include(x => x.Syllabus)
                .Include(x => x.Grade)
                .Include(x => x.Subject)
                .Include(x => x.Unit)
                .Include(x => x.Chapter)
                .Include(x => x.ResourceType)
                .AsQueryable();
        }
    }
}
