using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IInvestorService
    {
        public int Add(InvestorDto investor,int? loggedInUser);
        public InvestorDto? GetById(int id);

        public InvestorDtoWithPagination GetPaginatedData(InvestorSearchDto investorSearch);
        public int Update(InvestorDto investor, int? loggedInUser);
        public InvestorTotalActiveIactiveDto GetTotalActiveInactiveInvestors();
        public int ChangeStatus(int id, int status,int? loggedUser);
        public Task<List<DropdownDto>> GetInvestorsForDropdownAsync();

    }
}
