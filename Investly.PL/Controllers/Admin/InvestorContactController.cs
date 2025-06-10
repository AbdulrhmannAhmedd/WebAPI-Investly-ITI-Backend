
using Azure;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Http.HttpResults;
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
        public async Task<ResponseDto<PaginatedResultDto<InvestorContactRequestDto>>> GetContactRequests(
            int? pageNumber,
            int? pageSize,
            int? investorIdFilter,
            int? founderIdFilter,
            string statusFilter = null, // ← accept as string to allow both "1" and "Accepted"
            string columnOrderBy = null,
            string orderByDirection = Constants.Ascending,
            string searchTerm = null)
        {
            ResponseDto<PaginatedResultDto<InvestorContactRequestDto>> respnose;

            try
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

                respnose = new ResponseDto<PaginatedResultDto<InvestorContactRequestDto>>()
                {
                    Data = result,
                    Message = "Data Retrieved Successfully ",
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                };
                return respnose;

            }
            catch (Exception ex)
            {
                respnose = new ResponseDto<PaginatedResultDto<InvestorContactRequestDto>>
                {
                    Data = null,
                    Message = "Error While Retireve Data ",
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                };
                return respnose;
            }

        }




        [HttpPut("update-status")]
        public ResponseDto<InvestorContactRequestDto> UpdateContactRequestStatus([FromBody] UpdateContactRequestStatusDto model)
        {
            var response = new ResponseDto<InvestorContactRequestDto>();
            if (!ModelState.IsValid)
            {
                response.IsSuccess = false;
                response.Message = "Validation failed.";
                response.StatusCode = StatusCodes.Status400BadRequest;
                response.Data = null;
                return response;
            }

            try
            {
                _investorContactRequestService.UpdateContactRequestStatus(model);

                response.IsSuccess = true;
                response.Message = "Contact request status updated successfully.";
                response.StatusCode = StatusCodes.Status200OK;
                response.Data = _investorContactRequestService.GetContactRequestById(model.ContactRequestId);
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

        [HttpGet("{contactId}")]
        public async Task<ResponseDto<InvestorContactRequestDto>> GetContactRequest(int contactId)
        {
            var response = new ResponseDto<InvestorContactRequestDto>();

            try
            {
                var result = _investorContactRequestService.GetContactRequestById(contactId);

                response.IsSuccess = true;
                response.Message = "Contact request retrieved successfully.";
                response.StatusCode = StatusCodes.Status200OK;
                response.Data = result;

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
