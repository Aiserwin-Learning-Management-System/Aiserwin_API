using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Repository providing data access operations for <see cref="ExamAccount"/> entities.
    /// </summary>
    public class ExamAccountRepository : IExamAccountRepository
    {
        private readonly AppDbContext _dbContext;
        private readonly Microsoft.Extensions.Logging.ILogger<ExamAccountRepository> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamAccountRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The application database context.</param>
        /// <param name="logger">Logger instance.</param>
        public ExamAccountRepository(AppDbContext dbContext, Microsoft.Extensions.Logging.ILogger<ExamAccountRepository> logger)
        {
            _dbContext = dbContext;
            _logger = logger;
        }

        /// <summary>
        /// Gets all active exam accounts.
        /// </summary>
        /// <returns>A read-only list of <see cref="ExamAccount"/> entities.</returns>
        public async Task<IReadOnlyList<ExamAccount>> GetAllAsync()
        {
            try
            {
                var q = _dbContext.ExamAccounts
                    .Include(x => x.Batch)
                    .Include(x => x.Student)
                    .Include(x => x.Subject)
                    .Include(x => x.Unit)
                    .Include(x => x.Chapter)
                    .Include(x => x.QuestionTypeConfig)
                    .Include(x => x.Exam)
                    .Where(x => x.IsActive && !x.IsDeleted)
                    .AsNoTracking();

                return await q.ToListAsync();
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Gets an exam account by its identifier.
        /// </summary>
        /// <param name="id">The exam account identifier.</param>
        /// <returns>The <see cref="ExamAccount"/> if found; otherwise <c>null</c>.</returns>
        public async Task<ExamAccount?> GetByIdAsync(Guid id)
        {
            try
            {
                return await _dbContext.ExamAccounts
                    .Include(x => x.Batch)
                    .Include(x => x.Student)
                    .Include(x => x.Subject)
                    .Include(x => x.Unit)
                    .Include(x => x.Chapter)
                    .Include(x => x.QuestionTypeConfig)
                    .Include(x => x.Exam)
                    .FirstOrDefaultAsync(x => x.Id == id && !x.IsDeleted);
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Adds a new exam account to the database.
        /// </summary>
        /// <param name="entity">The exam account to add.</param>
        /// <returns>The added <see cref="ExamAccount"/> with generated identifiers.</returns>
        public async Task<ExamAccount> AddAsync(ExamAccount entity)
        {
            try
            {
                await _dbContext.ExamAccounts.AddAsync(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Updates an existing exam account.
        /// </summary>
        /// <param name="entity">The exam account with updated values.</param>
        /// <returns>The updated <see cref="ExamAccount"/>.</returns>
        public async Task<ExamAccount> UpdateAsync(ExamAccount entity)
        {
            try
            {
                entity.UpdatedAt = DateTime.UtcNow;
                _dbContext.ExamAccounts.Update(entity);
                await _dbContext.SaveChangesAsync();
                return entity;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Soft-deletes an exam account by setting <see cref="Domain.Common.BaseEntity.IsDeleted"/> and <see cref="Domain.Common.BaseEntity.IsActive"/> flags.
        /// </summary>
        /// <param name="id">The exam account identifier.</param>
        /// <returns><c>true</c> if deletion succeeded; otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            try
            {
                var entity = await _dbContext.ExamAccounts
                    .Include(x => x.Subject)
                        .ThenInclude(x => x.Course)
                        .ThenInclude(x => x.Stream)
                        .ThenInclude(x => x.Grade)
                        .ThenInclude(x => x.Syllabus)
                    .FirstOrDefaultAsync(x => x.Id == id);

                if (entity == null) return false;

                entity.IsActive = false;
                entity.IsDeleted = true;
                _dbContext.ExamAccounts.Update(entity);
                await _dbContext.SaveChangesAsync();
                return true;
            }
            catch (Exception ex)
            {
                throw;
            }
        }

        /// <summary>
        /// Returns an <see cref="IQueryable{ExamAccount}"/> for building filtered queries.
        /// </summary>
        /// <returns>An <see cref="IQueryable{ExamAccount}"/> instance.</returns>
        public IQueryable<ExamAccount> Query()
        {
            try
            {
                var q = _dbContext.ExamAccounts
                    .Include(x => x.Batch)
                    .Include(x => x.Student)
                    .Include(x => x.Subject)
                    .Include(x => x.Unit)
                    .Include(x => x.Chapter)
                    .Include(x => x.QuestionTypeConfig)
                    .Include(x => x.Exam)
                    .Where(x => !x.IsDeleted)
                    .AsNoTracking();

                return q;
            }
            catch (Exception ex)
            {
                throw;
            }
        }
    }
}
