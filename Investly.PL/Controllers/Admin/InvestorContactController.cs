using Investly.DAL.Helper;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Admin
{
    [Route("api/admin/[controller]")]
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
            string statusFilter = null, // ← accept as string to allow both "1" and "Accepted"
            string columnOrderBy = null,
            string orderByDirection = OrderBy.Ascending,
            string searchTerm = null)
        {
            ContactRequestStatus? statusFilterEnum = null;

            if (!string.IsNullOrWhiteSpace(statusFilter))
            {
                // Try to parse as int first
                if (int.TryParse(statusFilter, out int statusAsInt))
                {
                    if (Enum.IsDefined(typeof(ContactRequestStatus), statusAsInt))
                    {
                        statusFilterEnum = (ContactRequestStatus)statusAsInt;
                    }
                }
                else
                {
                    // Try to parse as enum name (case-insensitive)
                    if (Enum.TryParse(typeof(ContactRequestStatus), statusFilter, ignoreCase: true, out var statusAsEnum))
                    {
                        statusFilterEnum = (ContactRequestStatus)statusAsEnum;
                    }
                }
            }

            var result = await _investorContactRequestService.GetContactRequestsAsync(
                pageNumber,
                pageSize,
                investorIdFilter,
                founderIdFilter,
                statusFilterEnum, // Pass the enum? to service
                columnOrderBy,
                orderByDirection,
                searchTerm
            );

            return Ok(result);
        }


        [HttpPut("update-status")]
        public IActionResult UpdateContactRequestStatus([FromBody] UpdateContactRequestStatusDto model)
        {
            // Automatic model validation (checks data annotations)
            if (!ModelState.IsValid)
            {
                return BadRequest(new { errors = ModelState.Values.SelectMany(v => v.Errors) });
            }

            try
            {
                _investorContactRequestService.UpdateContactRequestStatus(model);
                return Ok(new { message = "Contact request status updated successfully." });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });  
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new { error = ex.Message });  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });  
            }
        }

        [HttpGet("{contactId}")]
        public async Task<IActionResult> GetContactRequest(int contactId)
        {
            try
            {
                var result = _investorContactRequestService.GetContactRequestById(contactId);
                return Ok(result);
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new { error = ex.Message });  
            }
            catch (Exception ex)
            {
                return StatusCode(500, new { error = "An unexpected error occurred" });  
            }
        }



    }
}
