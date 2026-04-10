using System;
using System.Collections.Generic;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Common;

namespace Winfocus.LMS.Application.Interfaces
{
    /// <summary>
    /// Defines business operations for <see cref="Winfocus.LMS.Domain.Entities.ExamAccount"/> entities.
    /// </summary>
    public interface IExamAccountService
    {
        /// <summary>
        /// Gets all exam accounts.
        /// </summary>
        /// <returns>CommonResponse containing list of <see cref="ExamAccountDto"/>.</returns>
        Task<CommonResponse<List<ExamAccountDto>>> GetAllAsync();

        /// <summary>
        /// Gets an exam account by id.
        /// </summary>
        /// <param name="id">Exam account identifier.</param>
        /// <returns>CommonResponse containing the <see cref="ExamAccountDto"/>.</returns>
        Task<CommonResponse<ExamAccountDto>> GetByIdAsync(Guid id);

        /// <summary>
        /// Creates a new exam account.
        /// </summary>
        /// <param name="request">Create request.</param>
        /// <returns>CommonResponse containing created <see cref="ExamAccountDto"/>.</returns>
        Task<CommonResponse<ExamAccountDto>> CreateAsync(ExamAccountRequest request);

        /// <summary>
        /// Updates an existing exam account.
        /// </summary>
        /// <param name="id">Exam account identifier.</param>
        /// <param name="request">Update request.</param>
        /// <returns>CommonResponse containing updated <see cref="ExamAccountDto"/>.</returns>
        Task<CommonResponse<ExamAccountDto>> UpdateAsync(Guid id, ExamAccountRequest request);

        /// <summary>
        /// Deletes (soft) an exam account.
        /// </summary>
        /// <param name="id">Exam account identifier.</param>
        /// <returns>CommonResponse indicating success.</returns>
        Task<CommonResponse<bool>> DeleteAsync(Guid id);

        /// <summary>
        /// Gets filtered and paginated exam accounts.
        /// </summary>
        /// <param name="request">Paging and filter request.</param>
        /// <returns>CommonResponse containing paged result of <see cref="ExamAccountDto"/>.</returns>
        Task<CommonResponse<PagedResult<ExamAccountDto>>> GetFilteredAsync(PagedRequest request);
    }
}
