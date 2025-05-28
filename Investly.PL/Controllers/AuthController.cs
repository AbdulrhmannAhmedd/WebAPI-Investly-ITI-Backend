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
        private readonly IUserService _userService;
        public AuthController(IInvestorService investorService,IJWTService jWTService, IUserService userService)
        {
            _investorService = investorService;
            _jWTService = jWTService;
            _userService = userService;
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

        [HttpPost("login")]
        public IActionResult Login([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors=ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Invalid input data.",
                    Data = errors,
                    StatusCode = StatusCodes.Status400BadRequest
                });

            }
            var user = _userService.GetByEmail(loginDto.Email);
            if (user == null || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
            {
                return Unauthorized(new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Invalid email or password.",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            var token = _jWTService.GenerateToken(user);
            return Ok(new ResponseDto<string>
            {
                Data = token,
                IsSuccess = true,
                Message = "Login successful.",
                StatusCode = StatusCodes.Status200OK
            });
        }



    }
}
