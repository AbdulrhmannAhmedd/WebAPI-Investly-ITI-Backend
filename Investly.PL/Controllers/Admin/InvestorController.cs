using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;

// For more information on enabling Web API for empty projects, visit https://go.microsoft.com/fwlink/?LinkID=397860

namespace Investly.PL.Controllers.Admin
{
    //[AuthorizeUserType((int)UserType.Investor)]
    [Route("api/admin/[controller]")]
    [ApiController]

    public class InvestorController : ControllerBase
    {
        private readonly IInvestorService _investorService;
        public InvestorController(IInvestorService investorService)
        {
            _investorService = investorService;
        }

        [HttpPost("paginated")]
        public ResponseDto<InvestorDtoWithPagination> GetPaginted(InvestorSearchDto dataSearch)
         {
            InvestorDtoWithPagination paginatedDto = _investorService.GetPaginatedData(dataSearch);
            return new ResponseDto<InvestorDtoWithPagination>
            {
                IsSuccess = true,
                Message = "Investors retrieved successfully.",
                Data = paginatedDto,
                StatusCode = StatusCodes.Status200OK
            };

        }


        [HttpGet("total-active-inactive")]
        public ResponseDto<InvestorTotalActiveIactiveDto> GetTotalActiveInactive()
        {
            var total = _investorService.GetTotalActiveInactiveInvestors();
            return new ResponseDto<InvestorTotalActiveIactiveDto>
            {
                IsSuccess = true,
                Message = "Total active and inactive investors retrieved successfully.",
                Data = total,
                StatusCode = StatusCodes.Status200OK
            };
        }

        // GET api/<InvestorController>/5
        [HttpGet("{id}")]
        public string Get(int id)
        {
            return "value";
        }

        [HttpPost]
        public ResponseDto<InvestorDto> Post([FromBody] InvestorDto data)
        {
            var result = _investorService.Add(data);
            ResponseDto<InvestorDto> response;
            if (result > 0)
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = true,
                    Message = "Investor added successfully.",
                    Data = _investorService.GetById(result),
                    StatusCode = StatusCodes.Status201Created
                };
             
            }
            else
            {
                response = new ResponseDto<InvestorDto> {
                
                    IsSuccess = false,
                    Message = "Failed to add investor.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
           
            }
            return response;
        }

        // PUT api/<InvestorController>/5
        [HttpPut("{id}")]
        public ResponseDto<InvestorDto> Put(int id, [FromBody] InvestorDto data)
        {
            var result = _investorService.Update(data);
            ResponseDto<InvestorDto> response;
            if (result > 0)
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = true,
                    Message = "Investor updated successfully.",
                    Data = _investorService.GetById(id),
                    StatusCode = StatusCodes.Status200OK
                };

            }
            else
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update investor.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };


            }
            return response;
        }

        // DELETE api/<InvestorController>/5
        [HttpDelete("{id}")]
        public void Delete(int id)
        {
        }
    }
}
