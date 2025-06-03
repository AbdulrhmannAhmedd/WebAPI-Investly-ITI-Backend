using Investly.PL.Dtos;
using Investly.PL.General.Services;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IInvestorService _investorService;
        private readonly IJWTService _jWTService;
        private readonly IUserService _userService;
        private readonly IHelper _helper;
        public AuthController(IInvestorService investorService,IJWTService jWTService, IUserService userService, IHelper helper)
        {
            _investorService = investorService;
            _jWTService = jWTService;
            _userService = userService;
            _helper = helper;
        }
        [HttpPost("register-investor")]
        public IActionResult RegisterInvestor([FromForm] Dtos.InvestorDto investorDto)
        {
            var picpath = _helper.UploadFile(investorDto.User.PicFile, "investor", "profilePic");
            var frontIdPath = _helper.UploadFile(investorDto.User.FrontIdPicFile, "investor", "nationalIdPic");
            var backIdPath = _helper.UploadFile(investorDto.User.BackIdPicFile, "investor", "nationalIdPic");

            investorDto.User.ProfilePicPath = picpath;
            investorDto.User.FrontIdPicPath = frontIdPath;
            investorDto.User.BackIdPicPath = backIdPath;
            var result = _investorService.Add(investorDto,null);
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
