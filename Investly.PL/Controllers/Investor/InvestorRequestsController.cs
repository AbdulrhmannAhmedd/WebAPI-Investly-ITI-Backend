using Azure;
using Investly.PL.Attributes;
using Investly.PL.BL;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Investor
{
    [Route("api/investor/[controller]")]
    [TypeFilter(typeof(AuthorizeUserTypeAttribute), Arguments = new object[] { (int)UserType.Investor })]
    public class InvestorRequestsController : Controller
    {
        private readonly IInvestorContactRequestService _contactRequestService;
    
        public InvestorRequestsController(IInvestorContactRequestService contactRequestService) 
        {
            _contactRequestService = contactRequestService;
        }
        [HttpGet]
        public IActionResult GetAllRequests( )
        {
            var res=_contactRequestService.GetContactRequestsByInvestor(User.GetUserId());
            if (res == null)
            {
                var response = new ResponseDto<InvestorContactRequestDto>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode=StatusCodes.Status400BadRequest

                };

                return BadRequest(response);
            }
            else  
            {
                var response = new ResponseDto<List<InvestorContactRequestDto>>
                {
                    Data = res,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK

                };

                return Ok(response);
            }

           
        }
       
        [HttpGet("contact-requests-count")]
        public IActionResult GetRequestsCount()
        {
            var res = _contactRequestService.GetContactRequestsCountByInvestor(User.GetUserId());
            if (res == null)
            {
                var response = new ResponseDto<CountContactRequestDto>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest
                    

                };

                return BadRequest(response);
            }
            else
            {
                var response = new ResponseDto<CountContactRequestDto>
                {
                    Data = res,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                  

                };

                return Ok(response);
            }

         
        }

        [HttpPut]
        public ResponseDto<InvestorContactRequestDto> UpdateContactRequestStatus([FromBody] UpdateContactRequestStatusDto model)
        {
            var response = new ResponseDto<InvestorContactRequestDto>();
           

            try
            {
                _contactRequestService.UpdateContactRequestStatus(model);

                response.IsSuccess = true;
                response.Message = "Contact request status updated successfully.";
                response.StatusCode = StatusCodes.Status200OK;
                response.Data = _contactRequestService.GetContactRequestById(model.ContactRequestId);
         
                return response;
            }
            catch (KeyNotFoundException ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status404NotFound;
                response.Data = null;
                return response;
            }
            catch (InvalidOperationException ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status403Forbidden; 
                response.Data = null;
                return response;
            }
            catch (ArgumentException ex)
            {
                response.IsSuccess = false;
                response.Message = ex.Message;
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Data = null;
                return response;
            }
            catch (Exception)
            {
                response.IsSuccess = false;
                response.Message = "An unexpected error occurred.";
                response.StatusCode = StatusCodes.Status500InternalServerError;
                response.Data = null;
                return response;
            }
        }
    }
}
