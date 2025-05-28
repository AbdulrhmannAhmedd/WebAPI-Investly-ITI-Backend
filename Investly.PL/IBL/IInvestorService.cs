using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IInvestorService
    {
        public int Add(InvestorDto investor);
        public InvestorDto? GetById(int id);
    }
}
