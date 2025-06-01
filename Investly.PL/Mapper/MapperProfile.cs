using AutoMapper;
using Investly.DAL.Entities;
using Investly.PL.Dtos;

namespace Investly.PL.Mapper
{
    public class MapperProfile : Profile
    {
        public MapperProfile()
        {
            CreateMap<Dtos.UserDto,User>().ReverseMap();   
            CreateMap<Dtos.InvestorDto, Investor>().ReverseMap();
            CreateMap <Dtos.FounderDto,Founder>().ReverseMap();
        }
    }
}
