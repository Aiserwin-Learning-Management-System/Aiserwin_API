namespace Winfocus.LMS.Infrastructure.Repositories
{
    using Microsoft.EntityFrameworkCore;
    using Winfocus.LMS.Application.DTOs.Masters;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;
    using Winfocus.LMS.Infrastructure.Data;

    /// <summary>
    /// Provides data access operations for <see cref="State"/> entities.
    /// </summary>
    public sealed class StateRepository : IStateRepository
    {
        /// <summary>
        /// The application database context used to access persistence.
        /// </summary>
        private readonly AppDbContext _dbContext;

        /// <summary>
        /// Initializes a new instance of the <see cref="StateRepository"/> class.
        /// </summary>
        /// <param name="dbContext">The database context.</param>
        public StateRepository(AppDbContext dbContext)
        {
            _dbContext = dbContext;
        }

        /// <summary>
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="id">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<State?> GetByIdAsync(Guid id)
        {
            return _dbContext.States
                .FirstOrDefaultAsync(r => r.Id == id && r.IsActive);
        }

        /// <summary>
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="name">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<State?> GetByNameAsync(string name)
        {
            return _dbContext.States
                .FirstOrDefaultAsync(r => r.StateName == name && r.IsActive);
        }

        /// <summary>
        /// Retrieves an active <see cref="State"/> by its identifier.
        /// </summary>
        /// <param name="countryid">The unique identifier of the state.</param>
        /// <returns>
        /// A <see cref="Task{TResult}"/> that resolves to the matching <see cref="State"/> if found and active; otherwise <c>null</c>.
        /// </returns>
        public Task<State?> GetByCountryAsync(Guid countryid)
        {
            return _dbContext.States
                .FirstOrDefaultAsync(r => r.CountryId == countryid && r.IsActive);
        }

        /// <summary>
        /// Retrieves all active <see cref="State"/> entities.
        /// </summary>
        /// <returns>A task resolving to a read-only list of active states.</returns>
        public Task<List<State>> GetAllAsync()
        {
            return _dbContext.States
                .Where(s => s.IsActive)
                .ToListAsync();
        }

        /// <summary>
        /// Creates a new <see cref="State"/> in the database.
        /// </summary>
        /// <param name="state">The state entity to create.</param>
        /// <returns>The created <see cref="State"/> with its assigned identifier.</returns>
        public async Task<State> CreateAsync(State state)
        {
            state.Id = state.Id == Guid.Empty ? Guid.NewGuid() : state.Id;
            state.CreatedAt = DateTime.UtcNow;

            await _dbContext.States.AddAsync(state);
            await _dbContext.SaveChangesAsync();

            return state;
        }

        /// <summary>
        /// Updates an existing active <see cref="State"/>.
        /// </summary>
        /// <param name="state">The state entity containing updated values. The <see cref="State.Id"/> must identify an existing active state.</param>
        /// <returns>The updated <see cref="State"/> if the entity existed and was updated; otherwise <c>null</c>.</returns>
        public async Task<State?> UpdateAsync(State state)
        {
            var existing = await _dbContext.States
                .FirstOrDefaultAsync(s => s.Id == state.Id && s.IsActive);

            if (existing is null)
            {
                return null;
            }

            // Preserve immutable/audit fields
            var createdAt = existing.CreatedAt;
            var createdBy = existing.CreatedBy;
            var isActive = existing.IsActive;

            // Apply incoming values
            _dbContext.Entry(existing).CurrentValues.SetValues(state);

            // Restore preserved fields and set updated timestamp
            existing.CreatedAt = createdAt;
            existing.CreatedBy = createdBy;
            existing.IsActive = isActive;
            existing.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return existing;
        }

        /// <summary>
        /// Soft-deletes an existing <see cref="State"/> by marking it inactive.
        /// </summary>
        /// <param name="id">The identifier of the state to delete.</param>
        /// <returns><c>true</c> if the state was found and marked inactive; otherwise <c>false</c>.</returns>
        public async Task<bool> DeleteAsync(Guid id)
        {
            var existing = await _dbContext.States
                .FirstOrDefaultAsync(s => s.Id == id && s.IsActive);

            if (existing is null)
            {
                return false;
            }

            existing.IsActive = false;
            existing.UpdatedAt = DateTime.UtcNow;

            await _dbContext.SaveChangesAsync();

            return true;
        }
    }
}
