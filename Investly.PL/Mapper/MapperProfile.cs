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
                    src.DesiredInvestmentType.HasValue
                    ? ((DesiredInvestmentType)src.DesiredInvestmentType.Value).ToString()
                    : null
                ))
              .ForMember(dest => dest.AiBusinessEvaluations, opt => opt.MapFrom(src => new AiBusinessEvaluationDto
              {
                  TotalWeightedScore = src.Airate.HasValue ? (int)src.Airate.Value : 0,
                  GeneralFeedback = src.GeneralAiFeedback,

              }))
               //.ForMember(dest => dest.AiBusinessEvaluations, opt => opt.MapFrom(src => src))
               .ReverseMap();
               // .ForMember(dest => dest.Airate, opt => opt.MapFrom(src => src.AiBusinessEvaluations != null ? src.AiBusinessEvaluations.TotalWeightedScore : 0))
               //.ForMember(dest => dest.GeneralAiFeedback, opt => opt.MapFrom(src => src.AiBusinessEvaluations != null ? src.AiBusinessEvaluations.GeneralFeedback : null));

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



            CreateMap<User, UpdateFounderDto>()
                .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
                .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
                .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
                .ForMember(dest => dest.Gender, opt => opt.MapFrom(src => src.Gender))
                .ForMember(dest => dest.GovernmentId, opt => opt.MapFrom(src => src.GovernmentId))
                .ForMember(dest => dest.CityId, opt => opt.MapFrom(src => src.CityId))
                .ForMember(dest => dest.Address, opt => opt.MapFrom(src => src.Address))
                .ForMember(dest => dest.DateOfBirth, opt => opt.MapFrom(src => src.DateOfBirth))
                .ReverseMap() // This creates the UpdateFounderDto → User mapping
                .ForAllMembers(opts => opts.Condition((src, dest, srcMember) =>
                    srcMember != null)); // Only map if source value is not null

            // Explicitly ignore all sensitive fields that should never be updated
            CreateMap<UpdateFounderDto, User>()
                .ForMember(dest => dest.Id, opt => opt.Ignore())
                .ForMember(dest => dest.Email, opt => opt.Ignore())
                .ForMember(dest => dest.HashedPassword, opt => opt.Ignore())
                .ForMember(dest => dest.NationalId, opt => opt.Ignore())
                .ForMember(dest => dest.FrontIdPicPath, opt => opt.Ignore())
                .ForMember(dest => dest.BackIdPicPath, opt => opt.Ignore())
                .ForMember(dest => dest.ProfilePicPath, opt => opt.Ignore())
                .ForMember(dest => dest.UserType, opt => opt.Ignore())
                .ForMember(dest => dest.Status, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.CreatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedBy, opt => opt.Ignore())
                .ForMember(dest => dest.UpdatedAt, opt => opt.Ignore())
                .ForMember(dest => dest.TokenVersion, opt => opt.Ignore());

            CreateMap<Feedback, FeedbackDto>()
                //.ForMember(dest => dest.UserTypeFromName, opt => opt.MapFrom(src => ((UserType)src.UserTypeFrom).ToString()))
                //.ForMember(dest => dest.UserTypeToName, opt => opt.MapFrom(src => ((UserType)src.UserTypeTo).ToString()))
                .ForMember(dest => dest.UserToName, opt => opt.MapFrom(src => src.UserIdToNavigation != null ? $"{src.UserIdToNavigation.FirstName} {src.UserIdToNavigation.LastName}" : null))
                .ForMember(dest => dest.UserFromName, opt => opt.MapFrom(src => src.CreatedByNavigation != null ? $"{src.CreatedByNavigation.FirstName} {src.CreatedByNavigation.LastName}" : null)) // New mapping
                .ReverseMap();


            CreateMap<AiBusinessStandardsEvaluation, AiStandardsEvaluationDto>()
                .ForMember(dest => dest.StandardCategoryId, opt => opt.MapFrom(src => src.CategoryStandardId))
                .ForMember(dest => dest.Feedback, opt => opt.MapFrom(src => src.Feedback))
                .ForMember(des=>des.Name,opt=>opt.MapFrom(src=>src.CategoryStandard.Standard.Name))
                .ForMember(des=>des.FormQuestion,opt=>opt.MapFrom(src=>src.CategoryStandard.Standard.FormQuestion))
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
                .ForMember(dest => dest.AchievementScore, opt => opt.MapFrom(src => src.AchievementScore))
                .ForMember(dest => dest.WeightedContribution, opt => opt.MapFrom(src => src.WeightedContribution))
                .ForMember(dest => dest.Weight, opt => opt.MapFrom(src => src.Weight))
                .ReverseMap();

        }

    }
}