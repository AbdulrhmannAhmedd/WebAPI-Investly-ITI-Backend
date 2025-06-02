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

            CreateMap<InvestorContactRequestDto, InvestorContactRequest>().AfterMap((src, dest) =>
            {
                src.InvestorName = $"{dest.Investor.User.FirstName} {dest.Investor.User.LastName}";
                src.BusinessTitle = $"{dest.Business.Title}";
                src.FounderName = $"{dest.Business.Founder.User.FirstName} {dest.Business.Founder.User.LastName}";
                src.BusinessId = dest.Business.Id;
                src.InvestorId = dest.Business.Id;
                src.FounderId = dest.Business.Founder.Id;
            }).ReverseMap();
        }
    }
}
