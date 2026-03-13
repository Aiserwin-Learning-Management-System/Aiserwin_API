using Microsoft.EntityFrameworkCore;
using Org.BouncyCastle.Pqc.Crypto.Lms;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Infrastructure.Data;

namespace Winfocus.LMS.Infrastructure.Repositories
{
    /// <summary>
    /// Repository responsible for managing <see cref="PageHeading"/> data access operations.
    /// </summary>
    public class PageHeadingRepository : IPageHeadingRepository
    {
        private readonly AppDbContext _context;

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHeadingRepository"/> class.
        /// </summary>
        /// <param name="context">The database context used for accessing page heading data.</param>
        public PageHeadingRepository(AppDbContext context)
        {
            _context = context;
        }

        /// <summary>
        /// Retrieves all active page headings from the database.
        /// </summary>
        /// <returns>
        /// A list of <see cref="PageHeading"/> entities that are not marked as deleted.
        /// </returns>
        public async Task<List<PageHeading>> GetAllAsync()
        {
            return await _context.PageHeadings
                .Where(x => !x.IsDeleted)
                .ToListAsync();
        }

        /// <summary>
        /// Retrieves a page heading by its unique page key.
        /// </summary>
        /// <param name="pageKey">The unique identifier of the page.</param>
        /// <returns>
        /// The matching <see cref="PageHeading"/> entity if found; otherwise, <c>null</c>.
        /// </returns>
        public async Task<PageHeading?> GetByPageKeyAsync(string pageKey)
        {
            return await _context.PageHeadings
                .FirstOrDefaultAsync(x => x.PageKey == pageKey && !x.IsDeleted);
        }

        /// <summary>
        /// Updates an existing page heading in the database.
        /// </summary>
        /// <param name="pageHeading">The page heading entity containing updated values.</param>
        /// <returns>A task representing the asynchronous operation.</returns>
        public async Task<PageHeading> UpdateAsync(PageHeading pageHeading)
        {
            _context.PageHeadings.Update(pageHeading);
            await _context.SaveChangesAsync();
            return pageHeading;
        }
    }
}
