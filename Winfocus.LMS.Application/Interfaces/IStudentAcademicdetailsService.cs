namespace Winfocus.LMS.Application.Interfaces
{
    using Winfocus.LMS.Application.DTOs;
    using Winfocus.LMS.Application.DTOs.Students;

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
        Task<CommonResponse<StudentDocumentsDto>> AddUploadedDocuments(StudentUploaddocumentsRequest request);
    }
}
