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
    /// Concrete implementation of <see cref="IQuestionRepository"/>.
    /// Handles database operations for Question entities using Entity Framework.
    /// </summary>
    public class QuestionRepository : IQuestionRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public QuestionRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <inheritdoc />
        public async Task<Question?> GetByIdAsync(Guid id)
        {
            return await _context.Questions
                .Include(q => q.Options)
                .Include(q => q.Reviews)
                .FirstOrDefaultAsync(q => q.Id == id);
        }

        /// <inheritdoc />
        public async Task<List<Question>> GetByTaskIdAsync(Guid taskId, int page, int pageSize)
        {
            return await _context.Questions
                .Where(q => q.TaskId == taskId)
                .Include(q => q.Options)
                .Include(q => q.Reviews)
                .OrderByDescending(q => q.CreatedAt)
                .Skip((page - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();
        }

        /// <inheritdoc />
        public async Task<int> GetCountByTaskIdAsync(Guid taskId)
        {
            return await _context.Questions
                .Where(q => q.TaskId == taskId)
                .CountAsync();
        }

        /// <inheritdoc />
        public async Task AddAsync(Question question)
        {
            await _context.Questions.AddAsync(question);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task UpdateAsync(Question question)
        {
            _context.Questions.Update(question);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task DeleteAsync(Question question)
        {
            _context.Questions.Remove(question);
            await _context.SaveChangesAsync();
        }

        /// <inheritdoc />
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Questions
                .AnyAsync(q => q.Id == id);
        }
    }
}
