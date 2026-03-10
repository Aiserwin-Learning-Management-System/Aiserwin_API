using AutoMapper;
using Org.BouncyCastle.Crypto;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.Interfaces;
using Winfocus.LMS.Domain.Entities;
using Winfocus.LMS.Domain.Enums;

namespace Winfocus.LMS.Application.Services
{
    /// <summary>
    /// Service responsible for managing dynamic form fields.
    /// Handles creation, update, deletion, grouping, and retrieval of form fields
    /// along with their selectable options.
    /// </summary>
    public class FormFieldService : IFormFieldService
    {
        private readonly IFormFieldRepository _repository;
        private readonly IMapper _mapper;

        /// <summary>
        /// Initializes a new instance of the <see cref="FormFieldService"/> class.
        /// </summary>
        /// <param name="repository">Repository used for data access operations.</param>
        /// <param name="mapper">AutoMapper instance used for DTO mapping.</param>
        public FormFieldService(
            IFormFieldRepository repository,
            IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        /// <summary>
        /// Creates a new dynamic form field.
        /// </summary>
        /// <param name="dto">The request DTO containing field details.</param>
        /// <returns>The created form field response.</returns>
        /// <exception cref="ArgumentException">
        /// Thrown when the provided <see cref="CreateFormFieldDto.FieldGroupId"/> does not exist.
        /// </exception>
        public async Task<FormFieldResponseDto> CreateAsync(CreateFormFieldDto dto)
        {
            if (dto.FieldGroupId.HasValue)
            {
                var exists = await _repository
                    .FieldGroupExistsAsync(dto.FieldGroupId.Value);

                if (!exists)
                    throw new ArgumentException("Invalid FieldGroupId");
            }

            var field = _mapper.Map<FormField>(dto);

            // Auto assign display order within group or standalone list
            field.DisplayOrder = await _repository
                .GetNextDisplayOrderAsync(dto.FieldGroupId);

            // Map selectable options for dropdown/radio/checkbox fields
            if (dto.Options != null)
            {
                field.FieldOptions = dto.Options.Select(o => new FieldOption
                {
                    Id = Guid.NewGuid(),
                    OptionValue = o.OptionValue,
                    DisplayOrder = o.DisplayOrder
                }).ToList();
            }

            await _repository.AddAsync(field);

            return _mapper.Map<FormFieldResponseDto>(field);
        }

        /// <summary>
        /// Retrieves all form fields including group information.
        /// </summary>
        /// <returns>A list of form fields.</returns>
        public async Task<List<FormFieldListDto>> GetAllAsync()
        {
            var fields = await _repository.GetAllAsync();

            return _mapper.Map<List<FormFieldListDto>>(fields);
        }

        /// <summary>
        /// Retrieves a form field by its identifier.
        /// Includes field options and group information.
        /// </summary>
        /// <param name="id">The unique identifier of the form field.</param>
        /// <returns>
        /// The form field response DTO if found; otherwise <c>null</c>.
        /// </returns>
        public async Task<FormFieldResponseDto?> GetByIdAsync(Guid id)
        {
            var field = await _repository.GetByIdAsync(id);

            if (field == null)
                return null;

            return _mapper.Map<FormFieldResponseDto>(field);
        }

        /// <summary>
        /// Updates an existing form field.
        /// Also synchronizes selectable options if provided.
        /// </summary>
        /// <param name="id">The identifier of the field to update.</param>
        /// <param name="dto">The updated field details.</param>
        /// <returns>The updated form field response.</returns>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the specified field does not exist.
        /// </exception>
        public async Task<FormFieldResponseDto> UpdateAsync(Guid id, UpdateFormFieldDto dto)
        {
            var field = await _repository.GetByIdAsync(id);

            if (field == null)
                throw new KeyNotFoundException("Field not found");

            _mapper.Map(dto, field);

            // Synchronize options if provided
            SyncOptions(field, dto.Options);

            await _repository.UpdateAsync(field);

            return _mapper.Map<FormFieldResponseDto>(field);
        }

        /// <summary>
        /// Soft deletes a form field.
        /// The record remains in the database but is marked as deleted.
        /// </summary>
        /// <param name="id">The identifier of the field to delete.</param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the field does not exist.
        /// </exception>
        /// <returns>.</returns>
        public async Task DeleteAsync(Guid id)
        {
            var field = await _repository.GetByIdAsync(id);

            if (field == null)
                throw new KeyNotFoundException();

            await _repository.SoftDeleteAsync(field);
        }

        /// <summary>
        /// Moves a form field to a different group or removes it from a group.
        /// </summary>
        /// <param name="id">The identifier of the form field.</param>
        /// <param name="dto">The target group information.</param>
        /// <exception cref="KeyNotFoundException">
        /// Thrown when the field does not exist.
        /// </exception>
        /// <exception cref="ArgumentException">
        /// Thrown when the specified group does not exist.
        /// </exception>
        /// <returns>.</returns>
        public async Task MoveFieldAsync(Guid id, MoveFieldToGroupDto dto)
        {
            var field = await _repository.GetByIdAsync(id);

            if (field == null)
                throw new KeyNotFoundException();

            if (dto.FieldGroupId.HasValue)
            {
                var exists = await _repository
                    .FieldGroupExistsAsync(dto.FieldGroupId.Value);

                if (!exists)
                    throw new ArgumentException("Invalid group");
            }

            // Assign new group
            field.FieldGroupId = dto.FieldGroupId;

            // Recalculate display order
            field.DisplayOrder = await _repository
                .GetNextDisplayOrderAsync(dto.FieldGroupId);

            await _repository.UpdateAsync(field);
        }

        /// <summary>
        /// Retrieves all standalone form fields that do not belong to any group.
        /// </summary>
        /// <returns>A list of ungrouped form fields.</returns>
        public async Task<List<FormFieldListDto>> GetUngroupedAsync()
        {
            var fields = await _repository.GetUngroupedAsync();

            return _mapper.Map<List<FormFieldListDto>>(fields);
        }

        /// <summary>
        /// Retrieves all available form field types defined in the <see cref="FieldType"/> enumeration.
        /// </summary>
        /// <returns>A list of field type identifiers and names.</returns>
        public Task<List<FieldTypeDto>> GetFieldTypesAsync()
        {
            var types = Enum.GetValues(typeof(FieldType))
                .Cast<FieldType>()
                .Select(t => new FieldTypeDto
                {
                    Id = (int)t,
                    Name = t.ToString()
                })
                .ToList();

            return Task.FromResult(types);
        }

        /// <summary>
        /// Synchronizes field options during update operations.
        /// Existing options are cleared and replaced with the provided set.
        /// </summary>
        /// <param name="field">The form field being updated.</param>
        /// <param name="options">The updated list of options.</param>
        private void SyncOptions(FormField field, List<UpdateFieldOptionDto>? options)
        {
            if (options == null)
                return;

            field.FieldOptions.Clear();

            foreach (var opt in options)
            {
                field.FieldOptions.Add(new FieldOption
                {
                    Id = opt.Id ?? Guid.NewGuid(),
                    OptionValue = opt.OptionValue,
                    DisplayOrder = opt.DisplayOrder,
                    IsActive = opt.IsActive
                });
            }
        }
    }
}
