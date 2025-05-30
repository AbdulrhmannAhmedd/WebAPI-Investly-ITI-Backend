using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IInvestorService
    {
        public int Add(InvestorDto investor);
        public InvestorDto? GetById(int id);

        public InvestorDtoWithPagination GetPaginatedData(InvestorSearchDto investorSearch);
        public int Update(InvestorDto investor);
        public InvestorTotalActiveIactiveDto GetTotalActiveInactiveInvestors();
    }
}
