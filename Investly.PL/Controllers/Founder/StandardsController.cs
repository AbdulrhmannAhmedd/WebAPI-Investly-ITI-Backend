using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Founder
{
    [Route("api/founder/[controller]")]
    
    public class StandardsController : Controller
    {
        private readonly IStandardService _standardService;
        public StandardsController(IStandardService standardService)
        {
            _standardService = standardService;
        }
        [HttpGet("GetStandardsByCategory/{id}")]
        public IActionResult GetAllStandardsByCategory(int id)
        {
            var standards = _standardService.GetStandardsByCategory(id);
            if (standards != null)
                return Ok(new ResponseDto<List<StandardDto>>
                {
                    IsSuccess = true,
                    Message = "Standards retrieved successfully.",
                    Data = standards,
                    StatusCode = StatusCodes.Status200OK
                });

            else
                return NotFound(new ResponseDto<List<StandardDto>>
                {
                    IsSuccess = false,
                    Message = "Standards retrieved failed.",
                    Data = standards,
                    StatusCode = StatusCodes.Status404NotFound
                });
        }
    }
}
