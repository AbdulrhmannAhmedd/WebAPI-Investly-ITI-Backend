using AutoMapper;
using Investly.DAL.Entities;
using Investly.PL.Dtos;
using Investly.PL.General;

namespace Investly.PL.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<UserDto, User>().ReverseMap();

            CreateMap<UserDto, User>().ReverseMap();
            CreateMap<InvestorDto, Investor>().ReverseMap();
            CreateMap<Business, BusinessDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.FounderName, opt => opt.MapFrom(src =>
                    src.Founder != null && src.Founder.User != null ? $"{src.Founder.User.FirstName} {src.Founder.User.LastName}" : null
                ))
                 .ForMember(dest => dest.DesiredInvestmentTypeName, opt => opt.MapFrom(src =>
                    // Map the integer value to the enum and then get its string name
                    src.DesiredInvestmentType.HasValue
                    ? ((DesiredInvestmentType)src.DesiredInvestmentType.Value).ToString()
                    : null
                ))
                .ReverseMap();

            CreateMap<Dtos.UserDto,User>().ReverseMap();   
            CreateMap<Dtos.InvestorDto, Investor>().ReverseMap();
            CreateMap<GovernmentDto, Government>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
            CreateMap<NotificationDto, Notification>().ReverseMap();
         
            CreateMap <Dtos.FounderDto,Founder>().ReverseMap();
            CreateMap<BusinessStandardAnswer, BusinessStandardAnswerDto>()
                           .ForMember(dest => dest.StandardQuestion, opt => opt.MapFrom(src => src.Standard != null ? src.Standard.Name : null)) // <--- NEW: Add this line
                           .ReverseMap(); CreateMap<StandardDto, Standard>().ReverseMap();

            CreateMap<InvestorContactRequest, InvestorContactRequestDto>()
            .ForMember(dest => dest.InvestorName,
                opt => opt.MapFrom(src => $"{src.Investor.User.FirstName} {src.Investor.User.LastName}"))
            .ForMember(dest => dest.BusinessTitle,
                opt => opt.MapFrom(src => src.Business.Title))
            .ForMember(dest => dest.FounderName,
                opt => opt.MapFrom(src => $"{src.Business.Founder.User.FirstName} {src.Business.Founder.User.LastName}"))
            .ForMember(dest => dest.BusinessId,
                opt => opt.MapFrom(src => src.Business.Id))
            .ForMember(dest => dest.InvestorId,
                opt => opt.MapFrom(src => src.Investor.Id)) // Corrected from Business.Id
            .ForMember(dest => dest.FounderId,
                opt => opt.MapFrom(src => src.Business.Founder.Id))
            .ForMember(dest => dest.Status,
                opt => opt.MapFrom(src => (ContactRequestStatus)src.Status));
            CreateMap<CategoryForListDto,Category>().ReverseMap();
            CreateMap<Feedback, FeedbackDto>()
                .ForMember(dest => dest.UserTypeFromName, opt => opt.MapFrom(src => ((UserType)src.UserTypeFrom).ToString()))
                .ForMember(dest => dest.UserTypeToName, opt => opt.MapFrom(src => ((UserType)src.UserTypeTo).ToString()))
                .ForMember(dest => dest.UserToName, opt => opt.MapFrom(src => src.UserIdToNavigation != null ? $"{src.UserIdToNavigation.FirstName} {src.UserIdToNavigation.LastName}" : null))
                .ForMember(dest => dest.UserFromName, opt => opt.MapFrom(src => src.CreatedByNavigation != null ? $"{src.CreatedByNavigation.FirstName} {src.CreatedByNavigation.LastName}" : null)) // New mapping
                .ReverseMap();
        }

    }
}