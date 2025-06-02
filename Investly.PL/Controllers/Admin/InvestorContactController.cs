using Investly.DAL.Helper;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Admin
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorContactController : ControllerBase
    {
        private readonly IInvestorContactRequestService _investorContactRequestService;

        public InvestorContactController(IInvestorContactRequestService investorContactRequestService)
        {
            _investorContactRequestService = investorContactRequestService;
        }

        [HttpGet]
        public async Task<IActionResult> GetContactRequests(
            int? pageNumber,
            int? pageSize,
            int? investorIdFilter,
            int? founderIdFilter,
            int? statusFilter,
            string columnOrderBy = null,
            string orderByDirection = OrderBy.Ascending,
            string searchTerm = null)
        {
            var result = await _investorContactRequestService.GetContactRequestsAsync(
                pageNumber,
                pageSize,
                investorIdFilter,
                founderIdFilter,
                statusFilter,
                columnOrderBy,
                orderByDirection,
                searchTerm
            );

            return Ok(result);
        }



    }
}
