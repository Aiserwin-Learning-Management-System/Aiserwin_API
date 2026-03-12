using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Provides business logic implementation for managing registration forms.
    /// This service interacts with the repository layer to perform
    /// database operations while enforcing application business rules.
    /// </summary>
    public class RegistrationFormService : IRegistrationFormService
    {
        /// <summary>
        /// Repository used for accessing registration form data.
        /// </summary>
        private readonly IRegistrationFormRepository _repository;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFormService"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository responsible for registration form persistence.
        /// </param>
        public RegistrationFormService(IRegistrationFormRepository repository)
        {
            _repository = repository;
        }

        /// <inheritdoc/>
        public async Task<Guid> CreateAsync(CreateRegistrationFormDto dto)
        {
            var form = new RegistrationForm
            {
                Id = Guid.NewGuid(),
                StaffCategoryId = dto.StaffCategoryId,
                FormName = dto.FormName,
                Description = dto.Description,
                IsActive = true,
                CreatedAt = DateTime.UtcNow,
            };

            await _repository.AddAsync(form);

            return form.Id;
        }

        /// <inheritdoc/>
        public async Task<RegistrationFormResponseDto> GetByIdAsync(Guid id)
        {
            var form = await _repository.GetByIdAsync(id);

            if (form == null)
                return null;

            var response = new RegistrationFormResponseDto
            {
                Id = form.Id,
                StaffCategoryId = form.StaffCategoryId,
                FormName = form.FormName,
                IsActive = form.IsActive,
                Sections = new List<FormSectionDto>(),
            };

            return response;
        }

        /// <inheritdoc/>
        public async Task<List<RegistrationFormListDto>> GetAllAsync()
        {
            var forms = await _repository.GetAllAsync();

            var result = forms.Select(form => new RegistrationFormListDto
            {
                Id = form.Id,
                FormName = form.FormName,
                StaffCategoryId = form.StaffCategoryId,
                IsActive = form.IsActive,
                GroupCount = form.FormGroups?.Count ?? 0,
                FieldCount = form.FormFields?.Count ?? 0,
            }).ToList();

            return result;
        }

        /// <inheritdoc/>
        public async Task UpdateAsync(Guid id, CreateRegistrationFormDto dto)
        {
            var form = await _repository.GetByIdAsync(id);

            if (form == null)
                throw new Exception("Registration form not found.");

            form.FormName = dto.FormName;
            form.Description = dto.Description;

            await _repository.UpdateAsync(form);
        }

        /// <inheritdoc/>
        public async Task DeleteAsync(Guid id)
        {
            var form = await _repository.GetByIdAsync(id);

            if (form == null)
                throw new Exception("Registration form not found.");

            await _repository.DeleteAsync(id);
        }
    }
}
