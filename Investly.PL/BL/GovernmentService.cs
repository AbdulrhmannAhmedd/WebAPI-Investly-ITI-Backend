using AutoMapper;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;

namespace Investly.PL.BL
{
    public class GovernmentService:IGovernementService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        public GovernmentService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public List<GovernmentDto> GetAll()
        {
            try
            {
                var governments = _unitOfWork.GovernmentRepo.GetAll();
                return _mapper.Map<List<GovernmentDto>>(governments);
            }
            catch (Exception ex)
            {
                
                return new List<GovernmentDto>(); 
            }
        }
        public List<CityDto> GetCitiesByGovernmentId(int governmentId)
        {
            try
            {
                if (governmentId <= 0)
                {
                    return new List<CityDto>(); 
                }
                var cities = _unitOfWork.CityRepo.GetAll(c => c.GovId == governmentId);
                return _mapper.Map<List<CityDto>>(cities);
            }
            catch (Exception ex)
            {
                return new List<CityDto>(); // Exception occurred
            }
        }
    }
}
