using Microsoft.AspNetCore.Http.HttpResults;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Application.DTOs.Masters;
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
        private readonly ILogger<RegistrationFormService> _logger;

        /// <summary>
        /// Initializes a new instance of the <see cref="RegistrationFormService"/> class.
        /// </summary>
        /// <param name="repository">
        /// The repository responsible for registration form persistence.
        /// </param>
        /// <param name="logger">Logger.</param>
        public RegistrationFormService(IRegistrationFormRepository repository, ILogger<RegistrationFormService> logger)
        {
            _repository = repository;
            _logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        /// <inheritdoc/>
        //public async Task<CommonResponse<RegistrationFormResponseDto>> CreateAsync(CreateRegistrationFormDto dto)
        //{
        //    var form = new RegistrationForm
        //    {
        //        Id = Guid.NewGuid(),
        //        StaffCategoryId = dto.StaffCategoryId,
        //        FormName = dto.FormName,
        //        Description = dto.Description,
        //        IsActive = true,
        //        CreatedAt = DateTime.UtcNow,
        //    };

        //    var created = await _repository.AddAsync(form);
        //    _logger.LogInformation(
        //  "Batch created successfully. Id: {Id}",
        //  created.Id);
        //    return CommonResponse<RegistrationFormResponseDto>.SuccessResponse(
        //      "batch created successfully", Map(created));
        //}


        public async Task<CommonResponse<Guid>> CreateAsync(CreateRegistrationFormDto dto)
        {
            // 1️⃣ Create the main form
            var form = new RegistrationForm
            {
                Id = Guid.NewGuid(),
                StaffCategoryId = dto.StaffCategoryId,
                FormName = dto.FormName,
                Description = dto.Description,
                IsActive = true
            };

            await _repository.AddAsync(form);

            var formGroups = new List<RegistrationFormGroup>();
            var formFields = new List<RegistrationFormField>();

            if (dto.Groups != null)
            {
                foreach (var group in dto.Groups)
                {
                    var formGroup = new RegistrationFormGroup
                    {
                        Id = Guid.NewGuid(),
                        FormId = form.Id,
                        FieldGroupId = group.FieldGroupId,
                        DisplayOrder = group.DisplayOrder
                    };

                    formGroups.Add(formGroup);
                }
            }

            // 4️⃣ Save standalone fields
            if (dto.StandaloneFields != null)
            {
                foreach (var field in dto.StandaloneFields)
                {
                    formFields.Add(new RegistrationFormField
                    {
                        Id = Guid.NewGuid(),
                        FormId = form.Id,
                        FieldId = field.FieldId,
                        DisplayOrder = field.DisplayOrder,
                        IsRequired = field.IsRequired
                    });
                }
            }

            // 5️⃣ Save groups and fields
            await _repository.AddGroupsAsync(formGroups);
            await _repository.AddFieldsAsync(formFields);

            _logger.LogInformation(
"registration form created successfully. Id: {Id}",
form.Id);
            return CommonResponse<Guid>.SuccessResponse(
              "batch created successfully", form.Id);
        }

        /// <inheritdoc/>
        public async Task<RegistrationFormResponseDto> GetByIdAsync(Guid id)
        {
            var form = await _repository.GetByIdAsync(id);

            if (form == null)
                return null;

            _logger.LogInformation("registration form data fetched successfully for Id: {Id}", id);
            var mappeddata = form == null ? null : Map(form);

            return mappeddata;
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


        private static RegistrationFormResponseDto Map(RegistrationForm c)
        {
            var sections = new List<FormSectionDto>();

            // GROUP SECTIONS
            if (c.FormGroups != null && c.FormGroups.Any())
            {
                var groupSections = c.FormGroups.OrderBy(g => g.DisplayOrder).
                    Select(g => new FormSectionDto
                    {
                        Type = "group",
                        GroupId = g.FieldGroupId,
                        GroupName = g.FieldGroup?.GroupName,
                        DisplayOrder = g.DisplayOrder
                    })
                    .ToList();

                sections.AddRange(groupSections);
            }

            // STANDALONE FIELDS
            var standaloneFields = c.FormFields
                .OrderBy(f => f.DisplayOrder)
                .Select(f => new FormFieldResponseDto
                {
                    FieldId = f.FieldId,
                    FieldName = f.FormField?.FieldName,
                    DisplayLabel = f.FormField?.DisplayLabel,
                    FieldType = f.FormField?.FieldType.ToString(),
                    IsRequired = f.IsRequired,
                    DisplayOrder = f.DisplayOrder,

                    Options = f.FormField?.FieldOptions?
                        .OrderBy(o => o.DisplayOrder)
                        .Select(o => new FieldOptionDto
                        {
                            Id = o.Id,
                            OptionValue = o.OptionValue,
                            DisplayOrder = o.DisplayOrder
                        }).ToList()
                }).ToList();

            if (standaloneFields.Any())
            {
                sections.Add(new FormSectionDto
                {
                    Type = "standalone",
                    GroupId = null,
                    GroupName = "Additional Fields",
                    DisplayOrder = 99,
                    Formfields = standaloneFields
                });
            }

            return new RegistrationFormResponseDto
            {
                Id = c.Id,
                StaffCategoryId = c.StaffCategoryId,
                FormName = c.FormName,
                IsActive = c.IsActive,

                StaffCategory = c.StaffCategory == null ? null : new StaffCategoryDto
                {
                    Id = c.StaffCategory.Id,
                    Name = c.StaffCategory.Name,
                    Description = c.StaffCategory.Description
                },

                Sections = sections.OrderBy(s => s.DisplayOrder).ToList()
            };
        }
    }
}
