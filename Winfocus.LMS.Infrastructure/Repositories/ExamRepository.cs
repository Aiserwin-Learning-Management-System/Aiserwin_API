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
    /// ExamRepository.
    /// </summary>
    public class ExamRepository : IExamRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="ExamRepository"/> class.
        /// </summary>
        /// <param name="context">The database context.</param>
        public ExamRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Adds the asynchronous.
        /// </summary>
        /// <param name="exam">The exam.</param>
        /// <returns>exam.</returns>
        public async Task<Exam> AddAsync(Exam exam)
        {
            await _context.Exams.AddAsync(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>task.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var entity = await _context.Exams.FindAsync(id);
            if (entity == null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.IsDeleted = true;

            _context.Exams.Update(entity);
            await _context.SaveChangesAsync();
            return true;
        }

        /// <summary>
        /// Existses the by code asynchronous.
        /// </summary>
        /// <param name="id">The id.</param>
        /// <returns>bool.</returns>
        public async Task<bool> ExistsAsync(Guid id)
        {
            return await _context.Exams.AnyAsync(e => e.Id == id);
        }

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>Exam.</returns>
        public async Task<Exam?> GetByIdAsync(Guid id)
        {
            return await _context.Exams
                .FirstOrDefaultAsync(e => e.Id == id);
        }

        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>exam list.</returns>
        public async Task<IReadOnlyList<Exam>> GetAllAsync()
        {
            return await _context.Exams
                .Where(x => x.IsActive && !x.IsDeleted)
                .AsNoTracking()
                .ToListAsync();
        }

        /// <summary>
        /// GetQuestionsForExamAsync.
        /// </summary>
        /// <param name="examId">The exam id.</param>
        /// <returns>task.</returns>
        public async Task<List<ExamQuestion>> GetQuestionsForExamAsync(Guid examId)
        {
            return await _context.ExamQuestions
                .Where(eq => eq.ExamId == examId)
                .Include(eq => eq.Question)
                    .ThenInclude(q => q.Options)
                .ToListAsync();
        }

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="exam">The exam.</param>
        /// <returns>task.</returns>
        public async Task<Exam> UpdateAsync(Exam exam)
        {
            _context.Exams.Update(exam);
            await _context.SaveChangesAsync();
            return exam;
        }

        /// <summary>
        /// Gets queryable for filtering with full hierarchy.
        /// </summary>
        /// <returns>Queryable exams.</returns>
        public IQueryable<Exam> Query()
        {
            var res = _context.Exams.Where(x => !x.IsDeleted)
                .AsNoTracking();
            return res;
        }

        /// <summary>
        /// Adds a new ExamQuestion mapping.
        /// </summary>
        /// <param name="examQuestion">The mapping entity.</param>
        /// <returns>The created mapping.</returns>
        public async Task<ExamQuestion> AddExamQuestionAsync(ExamQuestion examQuestion)
        {
            await _context.ExamQuestions.AddAsync(examQuestion);
            await _context.SaveChangesAsync();
            return examQuestion;
        }

        /// <summary>
        /// Gets an ExamQuestion mapping by id.
        /// </summary>
        /// <param name="id">The mapping id.</param>
        /// <returns>The mapping or null.</returns>
        public async Task<ExamQuestion?> GetExamQuestionByIdAsync(Guid id)
        {
            return await _context.ExamQuestions
                .Include(eq => eq.Question)
                .FirstOrDefaultAsync(eq => eq.Id == id);
        }

        /// <summary>
        /// Updates an existing ExamQuestion mapping.
        /// </summary>
        /// <param name="examQuestion">The mapping to update.</param>
        /// <returns>The updated mapping.</returns>
        public async Task<ExamQuestion> UpdateExamQuestionAsync(ExamQuestion examQuestion)
        {
            _context.ExamQuestions.Update(examQuestion);
            await _context.SaveChangesAsync();
            return examQuestion;
        }
    }
}
