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

            CreateMap<InvestorDto, Investor>().ReverseMap();
         
            CreateMap<Business, BusinessDto>()
                .ForMember(dest => dest.CategoryName, opt => opt.MapFrom(src => src.Category.Name))
                .ForMember(dest => dest.FounderName, opt => opt.MapFrom(src =>
                    src.Founder != null && src.Founder.User != null ? $"{src.Founder.User.FirstName} {src.Founder.User.LastName}" : null
                ))
                .ReverseMap();


            //CreateMap<Founder, FounderForListDto>()
            //    .ForMember(dest => dest.FullName, opt => opt.MapFrom(src =>
            //        src.User != null ? $"{src.User.FirstName} {src.User.LastName}" : null
            //    ))
            //    .ReverseMap();
        }

    }
}