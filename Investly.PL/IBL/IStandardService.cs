using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IStandardService
    {
        public List<StandardDto> GetStandardsByCategory(int Category_ID);
    }
}
