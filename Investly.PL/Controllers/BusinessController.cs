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

    }
}
