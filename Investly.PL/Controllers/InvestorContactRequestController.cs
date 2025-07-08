using Microsoft.AspNetCore.Http;
using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class InvestorContactRequestController : ControllerBase
    {

        private readonly IInvestorContactRequestService _contactRequestService;

        public InvestorContactRequestController(IInvestorContactRequestService contactRequestService)
        {
            _contactRequestService = contactRequestService;
        }

        [HttpPost("create")]
        public IActionResult CreateContactRequest([FromBody] CreateContactRequestDto request)
        {
            try
            {
                var result = _contactRequestService.CreateContactRequest(request.BusinessId, User.GetUserId());

                return Created("", new ResponseDto<ContactRequestResultDto>
                {
                    IsSuccess = true,
                    Message = "Contact request sent successfully.",
                    Data = new ContactRequestResultDto
                    {
                        ContactRequestId = result
                    },
                    StatusCode = StatusCodes.Status201Created
                });
            }
            catch (UnauthorizedAccessException ex)
            {
                return Unauthorized(new ResponseDto<ContactRequestResultDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseDto<ContactRequestResultDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            catch (InvalidOperationException ex)
            {
                return BadRequest(new ResponseDto<ContactRequestResultDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto<ContactRequestResultDto>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred while creating the contact request.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }

    }
    
}
