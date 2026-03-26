namespace Winfocus.LMS.Application.Services
{
    using Winfocus.LMS.Application.DTOs.Common;
    using Winfocus.LMS.Application.DTOs.LoginLog;
    using Winfocus.LMS.Application.Interfaces;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// Service implementation for user login log business logic.
    /// Orchestrates between controller and repository — all business rules live here.
    /// </summary>
    public class UserLoginLogService : IUserLoginLogService
    {
        private readonly IUserLoginLogRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="UserLoginLogService"/> class.
        /// </summary>
        /// <param name="repository">The repository.</param>
        public UserLoginLogService(IUserLoginLogRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc />
        public async Task<UserLoginLogDto> AddLogAsync(CreateLoginLogDto dto)
        {
            if (!Guid.TryParse(dto.UserId, out var userIdGuid))
            {
                throw new ArgumentException("Invalid UserId format.", nameof(dto.UserId));
            }

            var entity = new UserLoginLog
            {
                Id = Guid.NewGuid(),
                UserId = userIdGuid,
                LoginTimestamp = DateTimeOffset.UtcNow,
                IpAddress = dto.IpAddress,
                UserAgent = dto.UserAgent,
                IsSuccessful = dto.IsSuccessful,
                FailureReason = dto.FailureReason,
                CreatedAt = DateTime.UtcNow,
                CreatedBy = userIdGuid,
                IsActive = true,
            };

            var created = await _repository.AddAsync(entity);
            return MapToDto(created);
        }

        /// <inheritdoc />
        public async Task<PagedResult<UserLoginLogDto>> GetLogsByUserIdAsync(
            Guid userId, PagedRequest request)
        {
            var pagedEntities = await _repository.GetByUserIdAsync(userId, request);

            var dtos = pagedEntities.items.Select(MapToDto);

            return new PagedResult<UserLoginLogDto>(
                items: dtos,
                totalCount: pagedEntities.totalCount,
                limit: pagedEntities.limit,
                offset: pagedEntities.offset);
        }

        /// <inheritdoc />
        public async Task<bool> DeleteLogAsync(Guid logId)
        {
            var entity = await _repository.GetByIdAsync(logId);

            if (entity is null)
            {
                return false;
            }

            entity.IsActive = false;
            entity.UpdatedAt = DateTime.UtcNow;

            await _repository.UpdateAsync(entity);
            return true;
        }

        private static UserLoginLogDto MapToDto(UserLoginLog entity)
        {
            return new UserLoginLogDto
            {
                Id = entity.Id,
                UserId = entity.UserId,
                LoginTimestamp = entity.LoginTimestamp,
                IpAddress = entity.IpAddress,
                UserAgent = entity.UserAgent,
                IsSuccessful = entity.IsSuccessful,
                FailureReason = entity.FailureReason,
                CreatedAt = entity.CreatedAt,
            };
        }
    }
}
