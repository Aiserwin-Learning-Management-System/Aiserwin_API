using AutoMapper;
using Winfocus.LMS.Application.DTOs;
using Winfocus.LMS.Domain.Entities;

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
            // Maps Create DTO → Entity
            CreateMap<CreateFormFieldDto, FormField>();

            /// Maps Update DTO → Existing Entity
            /// Ignores Id to prevent overwriting the primary key.
            CreateMap<UpdateFormFieldDto, FormField>()
                .ForMember(dest => dest.Id, opt => opt.Ignore());

            /// Maps Entity → Detailed response DTO
            CreateMap<FormField, FormFieldResponseDto>()
                .ForMember(
                    dest => dest.FieldId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FieldType,
                    opt => opt.MapFrom(src => src.FieldType.ToString()));

            /// Maps Entity → List DTO
            CreateMap<FormField, FormFieldListDto>()
                .ForMember(
                    dest => dest.FieldId,
                    opt => opt.MapFrom(src => src.Id))
                .ForMember(
                    dest => dest.FieldType,
                    opt => opt.MapFrom(src => src.FieldType.ToString()));

            /// Maps FieldGroup entity → DTO
            CreateMap<FieldGroup, FieldGroupDto>()
                .ForMember(
                    dest => dest.GroupId,
                    opt => opt.MapFrom(src => src.Id));
        }
    }
}
