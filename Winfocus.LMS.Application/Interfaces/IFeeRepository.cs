namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines the contract for fee-related data access operations,
    /// including fee plans, discounts, student selections, and installments.
    /// </summary>
    public interface IFeeRepository
    {
        // ── FeePlan ──

        /// <summary>
        /// Adds a new fee plan asynchronously.
        /// </summary>
        /// <param name="feePlan">The fee plan entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddAsync(FeePlan feePlan);

        /// <summary>
        /// Retrieves all fee plans asynchronously.
        /// </summary>
        /// <returns>A task whose result contains a list of <see cref="FeePlan"/> entities.</returns>
        Task<List<FeePlan>> GetAllAsync();

        /// <summary>
        /// Retrieves a fee plan by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <returns>A task whose result contains the <see cref="FeePlan"/> if found; otherwise, null.</returns>
        Task<FeePlan?> GetByIdAsync(Guid id);

        /// <summary>
        /// Retrieves a fee plan by its unique identifier asynchronously.
        /// </summary>
        /// <param name="feePlanId">The unique identifier of the fee plan.</param>
        /// <returns>A task whose result contains the <see cref="FeePlan"/> if found; otherwise, null.</returns>
        Task<FeePlan?> GetFeePlanByIdAsync(Guid feePlanId);

        /// <summary>
        /// Retrieves a fee plan along with its associated discounts asynchronously.
        /// </summary>
        /// <param name="feePlanId">The unique identifier of the fee plan.</param>
        /// <returns>A task whose result contains the <see cref="FeePlan"/> with discounts if found; otherwise, null.</returns>
        Task<FeePlan?> GetFeePlanWithDiscountsAsync(Guid feePlanId);

        /// <summary>
        /// Deletes a fee plan by its unique identifier asynchronously.
        /// </summary>
        /// <param name="id">The unique identifier of the fee plan.</param>
        /// <returns>A task whose result indicates whether the deletion was successful.</returns>
        Task<bool> DeleteAsync(Guid id);

        /// <summary>
        /// Provides a queryable collection of fee plans.
        /// </summary>
        /// <returns>An <see cref="IQueryable{FeePlan}"/> for querying fee plans.</returns>
        IQueryable<FeePlan> Query();

        /// <summary>
        /// Adds a discount to a fee plan.
        /// </summary>
        /// <param name="discount">The discount entity to add.</param>
        void AddDiscount(FeePlanDiscount discount);

        /// <summary>
        /// Removes a discount from a fee plan.
        /// </summary>
        /// <param name="discount">The discount entity to remove.</param>
        void RemoveDiscount(FeePlanDiscount discount);

        // ── Student / Course ──

        /// <summary>
        /// Retrieves a student along with their associated courses asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <returns>A task whose result contains the <see cref="Student"/> with courses if found; otherwise, null.</returns>
        Task<Student?> GetStudentWithCoursesAsync(Guid studentId);

        /// <summary>
        /// Retrieves all courses associated with a specific grade asynchronously.
        /// </summary>
        /// <param name="gradeId">The unique identifier of the grade.</param>
        /// <returns>A task whose result contains a list of <see cref="Course"/> entities.</returns>
        Task<List<Course>> GetCoursesByGradeAsync(Guid gradeId);

        // ── StudentCourseDiscount ──

        /// <summary>
        /// Retrieves all course discounts assigned to a student asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <returns>A task whose result contains a list of <see cref="StudentCourseDiscount"/> entities.</returns>
        Task<List<StudentCourseDiscount>> GetStudentCourseDiscountsAsync(Guid studentId);

        /// <summary>
        /// Retrieves course discounts for a student in a specific course asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task whose result contains a list of <see cref="StudentCourseDiscount"/> entities.</returns>
        Task<List<StudentCourseDiscount>> GetStudentCourseDiscountsAsync(Guid studentId, Guid courseId);

        /// <summary>
        /// Adds multiple course discounts for a student asynchronously.
        /// </summary>
        /// <param name="discounts">The collection of discounts to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddStudentCourseDiscountsAsync(IEnumerable<StudentCourseDiscount> discounts);

        /// <summary>
        /// Removes all course discounts for a student in a specific course asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task RemoveStudentCourseDiscountsAsync(Guid studentId, Guid courseId);

        // ── FeePlanDiscount lookup ──

        /// <summary>
        /// Retrieves fee plan discounts by their unique identifiers asynchronously.
        /// </summary>
        /// <param name="ids">The collection of discount identifiers.</param>
        /// <returns>A task whose result contains a list of <see cref="FeePlanDiscount"/> entities.</returns>
        Task<List<FeePlanDiscount>> GetFeePlanDiscountsByIdsAsync(IEnumerable<Guid> ids);

        /// <summary>
        /// Retrieves all discounts associated with a specific course asynchronously.
        /// </summary>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task whose result contains a list of <see cref="FeePlanDiscount"/> entities.</returns>
        Task<List<FeePlanDiscount>> GetDiscountsByCourseAsync(Guid courseId);

        // ── StudentFeeSelection ──

        /// <summary>
        /// Adds a new student fee selection asynchronously.
        /// </summary>
        /// <param name="selection">The student fee selection entity to add.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddStudentFeeSelectionAsync(StudentFeeSelection selection);

        /// <summary>
        /// Retrieves a student fee selection by its unique identifier asynchronously.
        /// </summary>
        /// <param name="selectionId">The unique identifier of the fee selection.</param>
        /// <returns>A task whose result contains the <see cref="StudentFeeSelection"/> if found; otherwise, null.</returns>
        Task<StudentFeeSelection?> GetSelectionByIdAsync(Guid selectionId);

        /// <summary>
        /// Retrieves a student fee selection along with its details asynchronously.
        /// </summary>
        /// <param name="selectionId">The unique identifier of the fee selection.</param>
        /// <returns>A task whose result contains the <see cref="StudentFeeSelection"/> with details if found; otherwise, null.</returns>
        Task<StudentFeeSelection?> GetSelectionWithDetailsAsync(Guid selectionId);

        /// <summary>
        /// Retrieves all fee selections for a specific student asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <returns>A task whose result contains a list of <see cref="StudentFeeSelection"/> entities.</returns>
        Task<List<StudentFeeSelection>> GetSelectionsByStudentAsync(Guid studentId);

        /// <summary>
        /// Checks if a student has a confirmed fee selection for a specific course asynchronously.
        /// </summary>
        /// <param name="studentId">The unique identifier of the student.</param>
        /// <param name="courseId">The unique identifier of the course.</param>
        /// <returns>A task whose result indicates whether a confirmed selection exists.</returns>
        Task<bool> HasConfirmedSelectionForCourseAsync(Guid studentId, Guid courseId);

        /// <summary>
        /// Provides a queryable collection of student fee selections.
        /// </summary>
        /// <returns>An <see cref="IQueryable{StudentFeeSelection}"/> for querying fee selections.</returns>
        IQueryable<StudentFeeSelection> QuerySelections();

        // ── StudentInstallment ──

        /// <summary>
        /// Retrieves a student installment by its unique identifier asynchronously.
        /// </summary>
        /// <param name="installmentId">The unique identifier of the installment.</param>
        /// <returns>A task whose result contains the <see cref="StudentInstallment"/> if found; otherwise, null.</returns>
        Task<StudentInstallment?> GetInstallmentByIdAsync(Guid installmentId);

        /// <summary>
        /// Retrieves all installments associated with a specific fee selection asynchronously.
        /// </summary>
        /// <param name="selectionId">The unique identifier of the fee selection.</param>
        /// <returns>A task whose result contains a list of <see cref="StudentInstallment"/> entities.</returns>
        Task<List<StudentInstallment>> GetInstallmentsBySelectionAsync(Guid selectionId);

        // ── Discount Request ──

        /// <summary>
        /// Retrieves all students who have requested a manual discount asynchronously.
        /// </summary>
        /// <returns>A task whose result contains a list of <see cref="Student"/> entities.</returns>
        Task<List<Student>> GetStudentsWithDiscountRequestsAsync();

        // ── Common ──

        /// <summary>
        /// Saves the changes asynchronous.
        /// </summary>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task SaveChangesAsync();
    }
}
