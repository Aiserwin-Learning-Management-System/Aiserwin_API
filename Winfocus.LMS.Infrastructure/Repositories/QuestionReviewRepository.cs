namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// QuestionReviewRepository.
    /// </summary>
    /// <seealso cref="Winfocus.LMS.Application.Interfaces.IQuestionReviewRepository" />
    public sealed class QuestionReviewRepository : IQuestionReviewRepository
    {
        private readonly AppDbContext _db;

        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionReviewRepository"/> class.
        /// </summary>
        /// <param name="db">The database.</param>
        public QuestionReviewRepository(AppDbContext db)
        {
            _db = db;
        }

        /// <inheritdoc/>
        public async Task<Question?> GetQuestionWithDetailsAsync(Guid questionId)
        {
            return await _db.Questions
                .Include(q => q.Options)
                .Include(q => q.Reviews)

                // ── Full hierarchy through TaskAssignment ────────
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Syllabus)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Grade)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Subject)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Unit)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Chapter)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.ResourceType)

                // ── Operator info ────────────────────────────────
                .Include(q => q.Operator)
                    .ThenInclude(o => o.Values)
                        .ThenInclude(v => v.FormField)
                .FirstOrDefaultAsync(q => q.Id == questionId);
        }

        /// <inheritdoc/>
        public IQueryable<Question> QuerySubmittedQuestions()
        {
            return _db.Questions
                .Include(q => q.Options)
                .Include(q => q.Reviews)

                // ── Full hierarchy ───────────────────────────────
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Syllabus)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Grade)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Subject)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Unit)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Chapter)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.ResourceType)
                // ── Operator info ────────────────────────────────
                .Include(q => q.Operator)
                    .ThenInclude(o => o.Values)
                        .ThenInclude(v => v.FormField)
                .AsNoTracking()
                .Where(q => q.Status == 1); // Submitted
        }

        /// <inheritdoc/>
        public IQueryable<Question> QueryRejectedByOperator(Guid operatorId)
        {
            return _db.Questions
                .Include(q => q.Options)
                .Include(q => q.Reviews)

                // ── Full hierarchy ───────────────────────────────
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Syllabus)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Grade)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Subject)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Unit)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.Chapter)
                .Include(q => q.TaskAssignment)
                    .ThenInclude(t => t.ResourceType)
                .AsNoTracking()
                .Where(q => q.OperatorId == operatorId && q.Status == 4); // Rejected
        }

        /// <inheritdoc/>
        public async Task<int> CountByStatusAsync(int status)
            => await _db.Questions.CountAsync(q => q.Status == status);

        /// <inheritdoc/>
        public async Task<int> CountReviewedTodayAsync(int action)
        {
            var today = DateTime.UtcNow.Date;
            return await _db.QuestionReviews
                .CountAsync(r => r.Action == action && r.ReviewedAt >= today);
        }

        /// <inheritdoc/>
        public async Task<int> CountAllByActionAsync(int action)
            => await _db.QuestionReviews.CountAsync(r => r.Action == action);

        /// <inheritdoc/>
        public async Task AddReviewAsync(QuestionReview review)
        {
            _db.QuestionReviews.Add(review);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateQuestionAsync(Question question)
        {
            _db.Questions.Update(question);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task UpdateTaskAssignmentAsync(TaskAssignment task)
        {
            _db.TaskAssignments.Update(task);
            await _db.SaveChangesAsync();
        }

        /// <inheritdoc/>
        public async Task SaveChangesAsync()
            => await _db.SaveChangesAsync();
    }
}
