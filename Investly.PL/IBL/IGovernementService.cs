using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IGovernementService
    {
        public List<GovernmentDto> GetAll();
        public List<CityDto> GetCitiesByGovernmentId(int governmentId);
    }
}
