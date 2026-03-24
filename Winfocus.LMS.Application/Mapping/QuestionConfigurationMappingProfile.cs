namespace Winfocus.LMS.Application.Mappings
{
    using AutoMapper;
    using Winfocus.LMS.Application.DTOs.QuestionConfig;
    using Winfocus.LMS.Domain.Entities;

    /// <summary>
    /// AutoMapper profile for <see cref="QuestionConfiguration"/> entity mappings.
    /// </summary>
    public class QuestionConfigurationMappingProfile : Profile
    {
        /// <summary>
        /// Initializes a new instance of the <see cref="QuestionConfigurationMappingProfile"/> class.
        /// </summary>
        public QuestionConfigurationMappingProfile()
        {
            CreateMap<CreateQuestionConfigurationDto, QuestionConfiguration>();
        }
    }
}
