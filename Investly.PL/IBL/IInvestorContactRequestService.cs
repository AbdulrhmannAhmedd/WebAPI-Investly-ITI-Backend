using Investly.DAL.Entities;
using Investly.DAL.Helper;
using Investly.PL.Dtos.InvestorContactRequest;
using System.Linq.Expressions;

namespace Investly.PL.IBL
{
    public interface IInvestorContactRequestService
    {

        public Task<PaginatedResult<ContactRequestViewDto>> GetContactRequestsAsync(
            int? pageNumber = 1,
            int? pageSize = 10,
            int? investorIdFilter = null,
            int? founderIdFilter = null,
            int? statusFilter = null,
            string columnOrderBy = null,
            string orderByDirection = OrderBy.Ascending,
            string searchTerm = null);

    }
}
