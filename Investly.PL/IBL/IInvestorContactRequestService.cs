using Investly.DAL.Entities;
using Investly.PL.Dtos;
using Investly.PL.General;
using System.Linq.Expressions;

namespace Investly.PL.IBL
{
    public interface IInvestorContactRequestService
    {

        public Task<PaginatedResultDto<InvestorContactRequestDto>> GetContactRequestsAsync(
            int? pageNumber = 1,
            int? pageSize = 10,
            int? investorIdFilter = null,
            int? founderIdFilter = null,
            ContactRequestStatus? statusFilter = null, // Change parameter type to enum
            string columnOrderBy = null,
            string orderByDirection = Constants.Ascending,
            string searchTerm = null);

        public void UpdateContactRequestStatus(UpdateContactRequestStatusDto model, int? LoggedInUser, string? loggedInEmail = null);
        public InvestorContactRequestDto GetContactRequestById(int contactId);
        public List<InvestorContactRequestDto> GetContactRequestsByInvestor( int? LoggedInUser);
        public CountContactRequestDto GetContactRequestsCountByInvestor(int? LoggedInUser);

        public int CreateContactRequest(int businessId, int? loggedInUser);


    }
}
