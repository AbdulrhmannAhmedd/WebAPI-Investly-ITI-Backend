using Investly.PL.Dtos;
using Microsoft.AspNetCore.Identity;

namespace Investly.PL.IBL
{
    public interface IFounderService
    {
        public FoundersPaginatedDto GetAllPaginatedFounders(FounderSearchDto search);
        public int ChangeFounderStatus(int Id, int Status, int? LoggedInUser);
        public FoundersTotalActiveIactiveDto GetTotalFoundersActiveIactive();
        public FounderDto GetFounderById(int Id);
        public int Add(FounderDto founder, int? loggedInUser);
        public Task<List<DropdownDto>> GetFoundersForDropdownAsync();

        public FounderDto GetFounderByUserId(int LoggedInUserId);

        public Tuple<bool, FounderDto> UpdateFounderData(string email, UpdateFounderDto founderDto);

        public bool ChangePassword(ChangePasswordDto model);

    }
}
