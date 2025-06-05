using Investly.DAL.Entities;
using Investly.DAL.Helper;
using Investly.PL.Dtos;
using Investly.PL.General;
using System.Linq.Expressions;

namespace Investly.PL.IBL
{
    public interface IInvestorContactRequestService
    {

        public Task<PaginatedResult<InvestorContactRequestDto>> GetContactRequestsAsync(
            int? pageNumber = 1,
            int? pageSize = 10,
            int? investorIdFilter = null,
            int? founderIdFilter = null,
            ContactRequestStatus? statusFilter = null, // Change parameter type to enum
            string columnOrderBy = null,
            string orderByDirection = OrderBy.Ascending,
            string searchTerm = null);

        public void UpdateContactRequestStatus(UpdateContactRequestStatusDto model);
        public InvestorContactRequestDto GetContactRequestById(int contactId);


    }
}
