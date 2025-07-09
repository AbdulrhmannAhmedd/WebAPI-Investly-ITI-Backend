using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class BusinessController : ControllerBase
    {

        private readonly IBusinessService _businessService;

        public BusinessController(IBusinessService businessService)
        {
            _businessService = businessService;
        }


        [HttpPost("GetAllBusinesses")]
        public async Task<IActionResult> GetAllBusinessesForExplore([FromBody] BusinessSeachForExploreDto searchDto)
        {
            var res = await _businessService.GetAllBusinessesForExploreAsync(searchDto, User.GetUserId());

            if (res == null)
            {
                return BadRequest(new ResponseDto<BusinessListDtoForExplore>
                {
                    IsSuccess = false,
                    Message = "Businesses Retrieval Failed",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            else if (!res.Businesses.Any())
            {
                return Ok(new ResponseDto<BusinessListDtoForExplore>
                {
                    IsSuccess = true,
                    Message = "No business ideas found matching the criteria.",
                    Data = res,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else
            {
                return Ok(new ResponseDto<BusinessListDtoForExplore>
                {
                    IsSuccess = true,
                    Message = "Business ideas retrieved successfully.",
                    Data = res,
                    StatusCode = StatusCodes.Status200OK
                });
            }
        }


        [HttpGet("{businessId}")]
        public IActionResult GetBusinessDetails(int businessId)
        {
            try
            {
                var result = _businessService.GetBusinessDetails(businessId, User.GetUserId());

                return Ok(new ResponseDto<BusinessDetailsDto>
                {
                    IsSuccess = true,
                    Message = "Business details retrieved successfully.",
                    Data = result,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseDto<BusinessDetailsDto>
                {
                    IsSuccess = false,
                    Message = ex.Message,
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            catch (Exception ex)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto<BusinessDetailsDto>
                {
                    IsSuccess = false,
                    Message = "An unexpected error occurred while retrieving business details.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }


    }
}
