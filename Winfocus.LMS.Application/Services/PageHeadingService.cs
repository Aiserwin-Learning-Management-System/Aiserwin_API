using AutoMapper;
using Microsoft.Extensions.Caching.Memory;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Fees;
using Winfocus.LMS.Application.DTOs.Masters;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Service responsible for retrieving and updating page headings.
    /// Includes in-memory caching for performance optimization.
    /// </summary>
    public class PageHeadingService : IPageHeadingService
    {
        private readonly IPageHeadingRepository _repository;
        private readonly IMapper _mapper;
        private readonly IMemoryCache _cache;
        private readonly ILogger<PageHeadingService> _logger;

        /// <summary>
        /// Cache key used for storing page headings.
        /// </summary>
        private const string CACHE_KEY = "PAGE_HEADINGS";

        /// <summary>
        /// Initializes a new instance of the <see cref="PageHeadingService"/> class.
        /// </summary>
        /// <param name="repository">Repository for page heading data access.</param>
        /// <param name="mapper">AutoMapper instance for DTO mapping.</param>
        /// <param name="cache">Memory cache instance.</param>
        /// <param name="logger">logger.</param>
        public PageHeadingService(
            IPageHeadingRepository repository,
            IMapper mapper,
            IMemoryCache cache,
            ILogger<PageHeadingService> logger)
        {
            _repository = repository;
            _mapper = mapper;
            _cache = cache;
            _logger = logger;
        }

        /// <summary>
        /// Retrieves all page headings.
        /// Results are cached to improve performance.
        /// </summary>
        /// <returns>.</returns>
        public async Task<CommonResponse<List<PageHeadingResponseDto>>> GetAllAsync()
        {
            try
            {
                _logger.LogInformation("Fetching all page headings");

                if (!_cache.TryGetValue(CACHE_KEY, out List<PageHeadingResponseDto>? cached))
                {
                    _logger.LogInformation("Cache miss. Loading page headings from database");

                    var headings = await _repository.GetAllAsync();
                    cached = headings.Select(MapToDto).ToList();

                    _cache.Set(CACHE_KEY, cached, TimeSpan.FromHours(1));

                    _logger.LogInformation("Page headings cached successfully");
                }
                else
                {
                    _logger.LogInformation("Page headings retrieved from cache");
                }

                return CommonResponse<List<PageHeadingResponseDto>>
                    .SuccessResponse("Page headings retrieved successfully", cached);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving page headings");

                return CommonResponse<List<PageHeadingResponseDto>>
                    .FailureResponse("Failed to retrieve page headings");
            }
        }

        /// <summary>
        /// Retrieves a single page heading by its page key.
        /// </summary>
        /// <param name="pageKey">The pageKey.</param>
        /// <returns>.</returns>
        public async Task<CommonResponse<PageHeadingResponseDto>> GetByKeyAsync(string pageKey)
        {
            try
            {
                _logger.LogInformation("Fetching page heading for PageKey: {PageKey}", pageKey);

                var response = await GetAllAsync();
                var heading = response.Data.FirstOrDefault(x => x.PageKey == pageKey);

                if (heading == null)
                {
                    _logger.LogWarning("Page heading not found for PageKey: {PageKey}", pageKey);

                    return CommonResponse<PageHeadingResponseDto>
                        .FailureResponse("Page heading not found");
                }

                return CommonResponse<PageHeadingResponseDto>
                    .SuccessResponse("Page heading retrieved successfully", heading);
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while retrieving page heading for PageKey: {PageKey}", pageKey);

                return CommonResponse<PageHeadingResponseDto>
                    .FailureResponse("Failed to retrieve page heading");
            }
        }

        /// <summary>
        /// Updates the main and sub heading for a specific page.
        /// Cache is invalidated after update.
        /// </summary>
        /// <param name="pageKey">The pageKey.</param>
        /// <param name="dto">The dto.</param>
        /// <returns>.</returns>
        public async Task<CommonResponse<PageHeadingResponseDto>> UpdateAsync(string pageKey, UpdatePageHeadingDto dto)
        {
            try
            {
                _logger.LogInformation("Updating page heading for PageKey: {PageKey}", pageKey);

                var entity = await _repository.GetByPageKeyAsync(pageKey);

                if (entity == null)
                {
                    _logger.LogWarning("Invalid PageKey provided: {PageKey}", pageKey);

                    return CommonResponse<PageHeadingResponseDto>
                        .FailureResponse("Invalid pageKey");
                }

                entity.MainHeading = dto.MainHeading;
                entity.SubHeading = dto.SubHeading;

                await _repository.UpdateAsync(entity);

                // Clear cache after update
                _cache.Remove(CACHE_KEY);

                _logger.LogInformation("Page heading updated successfully for PageKey: {PageKey}", pageKey);

                return CommonResponse<PageHeadingResponseDto>
                    .SuccessResponse("Page heading updated successfully", MapToDto(entity));
            }
            catch (Exception ex)
            {
                _logger.LogError(ex, "Error occurred while updating page heading for PageKey: {PageKey}", pageKey);

                return CommonResponse<PageHeadingResponseDto>
                    .FailureResponse("Failed to update page heading");
            }
        }

        private static PageHeadingResponseDto MapToDto(PageHeading pageHeading)
        {
            return new PageHeadingResponseDto
            {
                Id = pageHeading.Id,
                PageKey = pageHeading.PageKey,
                MainHeading = pageHeading.MainHeading,
                SubHeading = pageHeading.SubHeading,
                ModuleName = pageHeading.ModuleName,
                IsActive = pageHeading.IsActive,
            };
        }
    }
}
