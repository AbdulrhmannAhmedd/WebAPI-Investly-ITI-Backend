using AutoMapper;
using Investly.DAL.Entities;
using Investly.PL.Dtos;

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


            CreateMap <Dtos.FounderDto,Founder>().ReverseMap();
            CreateMap<InvestorContactRequest, InvestorContactRequestDto>()
                .AfterMap((src, dest) =>
                {
                    dest.InvestorName = $"{src.Investor.User.FirstName} {src.Investor.User.LastName}";
                    dest.BusinessTitle = $"{src.Business.Title}";
                    dest.FounderName = $"{src.Business.Founder.User.FirstName} {src.Business.Founder.User.LastName}";
                    dest.BusinessId = src.Business.Id;
                    dest.InvestorId = src.Investor.Id; // Fixed: was src.Business.Id
                    dest.FounderId = src.Business.Founder.Id;
                });
        }

    }
}