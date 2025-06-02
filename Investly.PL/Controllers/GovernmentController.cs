using Investly.PL.Dtos;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class GovernmentController : ControllerBase
    {
        private readonly IGovernementService _governmentService;
        public GovernmentController(IGovernementService governementService)
        {
            _governmentService = governementService;

        }
        [HttpGet()]
        public IActionResult Get()
        {
            var data = _governmentService.GetAll();
            if (data == null || !data.Any())
            {
                return NotFound(new ResponseDto<List<GovernmentDto>>
                {
                    IsSuccess = false,
                    Message = "No governments found.",
                    Data = data,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            else
            {
                return Ok(new ResponseDto<List<GovernmentDto>>
                {
                    IsSuccess = true,
                    Message = "Governments retrieved successfully.",
                    Data = data,
                    StatusCode = StatusCodes.Status200OK
                });

            }

        }

        [HttpGet("{id:int}/cities")]
        public IActionResult Get(int id)
        {

            if (id <= 0)
            {
                return BadRequest(new ResponseDto<GovernmentDto>
                {
                    IsSuccess = false,
                    Message = "Invalid ID provided.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var cities = _governmentService.GetCitiesByGovernmentId(id);
            if (cities == null ||!cities.Any())
            {
                return NotFound(new ResponseDto<List<CityDto>>
                {
                    IsSuccess = false,
                    Message = "cities not rertived ",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            return Ok(new ResponseDto<List<CityDto>>
            {
                IsSuccess = true,
                Message = "Government retrieved successfully.",
                Data = cities,
                StatusCode = StatusCodes.Status200OK
            });


        }
    }
}
