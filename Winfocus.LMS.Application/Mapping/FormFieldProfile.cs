using AutoMapper;
using System;
using System.Collections.Generic;
using System.Text;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Domain.Entities;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Winfocus.LMS.Application.Mapping
{
    /// <summary>
    /// AutoMapper profile responsible for mapping between
    /// FormField domain entities and FormField-related DTOs.
    /// 
    /// This profile centralizes all mapping rules required for
    /// creating, listing, and returning form field data through the API.
    /// </summary>
    public class FormFieldProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="FormFieldProfile"/> class
        /// and defines mapping configurations between entities and DTOs.
        /// </summary>
        public FormFieldProfile()
        {
            /// <summary>
            /// Maps the <see cref="CreateFormFieldDto"/> request DTO to the
            /// <see cref="FormField"/> entity when creating a new form field.
            /// </summary>
            CreateMap<CreateFormFieldDto, FormField>();

            /// <summary>
            /// Maps the <see cref="FormField"/> entity to the
            /// <see cref="FormFieldResponseDto"/> used for detailed API responses.
            /// 
            /// Custom mappings:
            /// - Maps <c>FormField.Id</c> to <c>FieldId</c>.
            /// - Converts the <see cref="FieldType"/> enum to its string representation.
            /// </summary>
            CreateMap<FormField, FormFieldResponseDto>()
                .ForMember(
                    dest => dest.FieldId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FieldType,
                    opt => opt.MapFrom(src => src.FieldType.ToString()));

            /// <summary>
            /// Maps the <see cref="FormField"/> entity to the
            /// <see cref="FormFieldListDto"/> used in list endpoints.
            /// 
            /// Custom mappings:
            /// - Maps <c>FormField.Id</c> to <c>FieldId</c>.
            /// - Converts the <see cref="FieldType"/> enum to string for readability.
            /// </summary>
            CreateMap<FormField, FormFieldListDto>()
                .ForMember(
                    dest => dest.FieldId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FieldType,
                    opt => opt.MapFrom(src => src.FieldType.ToString()));

            /// <summary>
            /// Maps the <see cref="FieldGroup"/> entity to a lightweight
            /// <see cref="FieldGroupDto"/> used inside form field responses.
            /// Custom mappings:
            /// - Maps <c>FieldGroup.Id</c> to <c>GroupId</c>.
            /// </summary>
            CreateMap<FieldGroup, FieldGroupDto>()
                .ForMember(
                    dest => dest.GroupId,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
