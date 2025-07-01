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
        public InvestorDto GetInvestorByUserId(int? loggedInUser);
        public int UpdateProfilePicture(string ProfilePicPath, int? loggedInUser);
        public int UpdateNationalId(string FrontIdPicPath, string BackIdPicPath, int? loggedInUser);
        public int ChangePassword(ChangePasswordDto passwordDto,int? loggedInUser);



    }
}
