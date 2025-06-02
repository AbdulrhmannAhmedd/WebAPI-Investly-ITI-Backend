using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IFounderService
    {
        public FoundersPaginatedDto GetAllPaginatedFounders(FounderSearchDto search);
        public int ChangeFounderStatus(int Id, int Status, int? LoggedInUser);
        public FoundersTotalActiveIactiveDto GetTotalFoundersActiveIactive();
        public FounderDto GetFounderById(int Id);
    }
}
