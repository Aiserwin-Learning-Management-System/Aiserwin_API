using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// ExamGradeRepository.
    /// </summary>
    public class ContentResourceTypeRepository : IContentResourceTypeRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="ContentResourceTypeRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public ContentResourceTypeRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>ContentResourceType list.</returns>
        public async Task<IReadOnlyList<ContentResourceType>> GetAllAsync()
        {
            return await _dbContext.ContentResourceTypes
                 .Include(x => x.Chapter)
                 .ThenInclude(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .Where(x => x.IsActive && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>ContentResourceType.</returns>
        public async Task<ContentResourceType?> GetByIdAsync(Guid id)
        {
            var res = _dbContext.ContentResourceTypes
                  .Include(x => x.Chapter)
                 .ThenInclude(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .Where(x => x.Id == id && !x.IsDeleted);
            return await res.FirstOrDefaultAsync();
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="contentResourceType">The ContentResourceType.</param>
        /// <returns>ContentResourceType.</returns>
        public async Task<ContentResourceType> AddAsync(ContentResourceType contentResourceType)
        {
            _dbContext.ContentResourceTypes.Add(contentResourceType);
            await _dbContext.SaveChangesAsync();
            return contentResourceType;
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="contentResourceType">The ContentResourceType.</param>
        /// <returns>task.</returns>
        public async Task<ContentResourceType> UpdateAsync(ContentResourceType contentResourceType)
        {
            _dbContext.ContentResourceTypes.Update(contentResourceType);
            await _dbContext.SaveChangesAsync();
            return contentResourceType;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _dbContext.ContentResourceTypes.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _dbContext.ContentResourceTypes.Update(entity);
            await _dbContext.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="name">The name.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsByNameAsync(string name)
        {
            name = name.Trim();

            return await _dbContext.ContentResourceTypes
                .AnyAsync(x =>
                    x.Name.Trim().ToLower() == name.ToLower());
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable ContentResourceType.</returns>
        public IQueryable<ContentResourceType> Query()
        {
            return _dbContext.ContentResourceTypes.Where(x => !x.IsDeleted)
                  .Include(x => x.Chapter)
                 .ThenInclude(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .AsNoTracking();
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="chapterid">The Syllabus.</param>
        /// <returns>ContentResourceType.</returns>
        public async Task<List<ContentResourceType>> GetByChapterIdAsync(Guid chapterid)
        {
            var res = _dbContext.ContentResourceTypes
                 .Include(x => x.Chapter)
                 .ThenInclude(x => x.Unit)
                .ThenInclude(x => x.Subject)
                .ThenInclude(x => x.Course)
                .ThenInclude(x => x.Grade)
                .ThenInclude(x => x.Syllabus)
                 .ThenInclude(x => x.AcademicYear)
                .Where(x => x.ChapterId == chapterid && !x.IsDeleted);
            return await res.ToListAsync();
        }
    }
}
