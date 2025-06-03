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
            bool? statusFilter = null,
            string columnOrderBy = null,
            string orderByDirection = Constants.Ascending,
            string searchTerm = null);

        public void ToggelActivateContactRequest(ContactRequestToggleActivationDto model);


    }
}
