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
        public async Task<(List<Question> Items, int TotalCount)> GetByOperatorAsync(
            Guid operatorId,
            string? subject,
            string? chapter,
            int? status,
            int? questionType,
            string? search,
            string? sortBy,
            int pageNumber,
            int pageSize)
        {
            var query = _context.Questions
                .Where(q => q.OperatorId == operatorId)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Subject)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Chapter)
                .AsQueryable();

            if (!string.IsNullOrWhiteSpace(subject))
            {
                query = query.Where(q => q.TaskAssignment.Subject.Name == subject);
            }

            if (!string.IsNullOrWhiteSpace(chapter))
            {
                query = query.Where(q => q.TaskAssignment.Chapter != null && q.TaskAssignment.Chapter.Name == chapter);
            }

            if (status.HasValue)
            {
                query = query.Where(q => q.Status == status.Value);
            }

            if (questionType.HasValue)
            {
                query = query.Where(q => q.QuestionType == questionType.Value);
            }

            if (!string.IsNullOrWhiteSpace(search))
            {
                query = query.Where(q => q.QuestionText.Contains(search));
            }

            // Determine ordering
            switch (sortBy?.ToLowerInvariant())
            {
                case "status":
                    query = query.OrderBy(q => q.Status).ThenByDescending(q => q.CreatedAt);
                    break;
                case "subject":
                    query = query.OrderBy(q => q.TaskAssignment.Subject.Name).ThenByDescending(q => q.CreatedAt);
                    break;
                case "date":
                default:
                    query = query.OrderByDescending(q => q.CreatedAt);
                    break;
            }

            var total = await query.CountAsync();

            var items = await query
                .Skip((pageNumber - 1) * pageSize)
                .Take(pageSize)
                .ToListAsync();

            return (items, total);
        }

        /// <inheritdoc />
        public async Task<Winfocus.LMS.Application.DTOs.Stats.QuestionStatsDto> GetStatsForOperatorAsync(Guid operatorId)
        {
            var q = _context.Questions.Where(x => x.OperatorId == operatorId);

            var groups = await q.GroupBy(x => x.Status)
                .Select(g => new { Status = g.Key, Count = g.Count() })
                .ToListAsync();

            int total = await q.CountAsync();
            int draft = groups.FirstOrDefault(g => g.Status == (int)Winfocus.LMS.Domain.Enums.QuestionStatus.Draft)?.Count ?? 0;
            int submitted = groups.FirstOrDefault(g => g.Status == (int)Winfocus.LMS.Domain.Enums.QuestionStatus.Submitted)?.Count ?? 0;
            int underReview = groups.FirstOrDefault(g => g.Status == (int)Winfocus.LMS.Domain.Enums.QuestionStatus.UnderReview)?.Count ?? 0;
            int approved = groups.FirstOrDefault(g => g.Status == (int)Winfocus.LMS.Domain.Enums.QuestionStatus.Approved)?.Count ?? 0;
            int rejected = groups.FirstOrDefault(g => g.Status == (int)Winfocus.LMS.Domain.Enums.QuestionStatus.Rejected)?.Count ?? 0;

            var stats = new Winfocus.LMS.Application.DTOs.Stats.QuestionStatsDto
            {
                Total = total,
                Draft = draft,
                Pending = submitted + underReview,
                Approved = approved,
                Rejected = rejected,
                Completed = approved + rejected + submitted + underReview,
            };

            if ((approved + rejected) > 0)
            {
                stats.ApprovalRate = Math.Round((decimal)approved / (approved + rejected) * 100, 2);
                stats.RejectionRate = Math.Round((decimal)rejected / (approved + rejected) * 100, 2);
            }

            return stats;
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

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable questions.</returns>
        public IQueryable<Question> Query()
        {
            return _context.Questions.Where(x => !x.IsDeleted)
                                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Subject)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Chapter)
                .AsNoTracking();
        }
    }
}
