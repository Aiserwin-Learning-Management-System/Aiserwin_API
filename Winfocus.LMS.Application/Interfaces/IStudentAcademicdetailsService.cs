namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Students;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Defines business operations for <see cref="StudentAcademicdetails"/> entities.
    /// </summary>
    public interface IStudentAcademicdetailsService
    {
        /// <summary>
        /// Gets all asynchronous.
        /// </summary>
        /// <returns>StudentAcademicdetailsDto.</returns>
        Task<IReadOnlyList<StudentAcademicdetailsDto>> GetAllAsync();

        /// <summary>
        /// Gets the by identifier asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>StudentAcademicdetailsDto.</returns>
        Task<StudentAcademicdetailsDto?> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentAcademicdetailsDto.</returns>
        Task<CommonResponse<StudentAcademicdetailsDto>> CreateAsync(StudentAcademicdetailsRequest request);

        /// <summary>
        /// Updates the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <param name="request">The request.</param>
        /// <returns>id.</returns>
        Task UpdateAsync(Guid id, StudentAcademicdetailsRequest request);

        /// <summary>
        /// Deletes the asynchronous.
        /// </summary>
        /// <param name="id">The identifier.</param>
        /// <returns>id.</returns>
        Task DeleteAsync(Guid id);

        /// <summary>
        /// Creates the asynchronous.
        /// </summary>
        /// <param name="request">The request.</param>
        /// <returns>StudentDocumentsDto.</returns>
        Task<StudentDocumentsDto> AddUploadedDocuments(StudentUploaddocumentsRequest request);

        /// <summary>
        /// Adds courses for a specific student academic record.
        /// </summary>
        /// <param name="studentId">Student details identifier.</param>
        /// <param name="courseIds">List of course identifiers.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddCoursesAsync(Guid studentId, List<Guid> courseIds);

        /// <summary>
        /// Adds courses for a specific student academic record.
        /// </summary>
        /// <param name="studentId">Student  details identifier.</param>
        /// <param name="batchtimingmtfid">List of Batchtimingmtf identifiers.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBatchTimingMTFsAsync(Guid studentId, List<Guid> batchtimingmtfid);

        /// <summary>
        /// Adds courses for a specific student academic record.
        /// </summary>
        /// <param name="studentId">Student  details identifier.</param>
        /// <param name="batchtimingsaturdayid">List of Batchtiming saturday identifiers.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBatchTimingSaturdaysAsync(Guid studentId, List<Guid> batchtimingsaturdayid);

        /// <summary>
        /// Adds courses for a specific student academic record.
        /// </summary>
        /// <param name="studentId">Student  details identifier.</param>
        /// <param name="batchtimingsundaysid">List of Batchtiming sunday identifiers.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        Task AddBatchTimingSundaysAsync(Guid studentId, List<Guid> batchtimingsundaysid);
    }
}
