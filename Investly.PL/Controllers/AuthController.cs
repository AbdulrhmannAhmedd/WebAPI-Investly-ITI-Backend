using Investly.PL.Dtos;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IInvestorService _investorService;
        private readonly IJWTService _jWTService;
        public AuthController(IInvestorService investorService,IJWTService jWTService)
        {
            _investorService = investorService;
            _jWTService = jWTService;

        }
        [HttpPost("register-investor")]
        public IActionResult RegisterInvestor([FromBody] Dtos.InvestorDto investorDto)
        {
            
            var result = _investorService.Add(investorDto);
            if (result > 0)
            {
                // Generate JWT token for the new investor
                var investor = _investorService.GetById(result);
                var token = _jWTService.GenerateToken(investor.User);

                return Ok(new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "Investor registered successfully.",
                    Data = token,
                    StatusCode = StatusCodes.Status201Created

                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "An error occurred while registering the investor.",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }



    }
}
