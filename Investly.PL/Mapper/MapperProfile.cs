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
                .ReverseMap();

            CreateMap<Dtos.UserDto,User>().ReverseMap();   
            CreateMap<Dtos.InvestorDto, Investor>().ReverseMap();
            CreateMap<GovernmentDto, Government>().ReverseMap();
            CreateMap<CityDto, City>().ReverseMap();
            CreateMap<NotificationDto, Notification>().ReverseMap();
         
            CreateMap <Dtos.FounderDto,Founder>().ReverseMap();
            CreateMap<BusinessStandardAnswerDto, BusinessStandardAnswer>().ReverseMap();
            CreateMap<StandardDto, Standard>().ReverseMap();

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

        }

    }
}