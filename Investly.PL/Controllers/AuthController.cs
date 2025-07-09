using Investly.DAL.Repos;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Attributes;
using Investly.PL.BL;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.General.Services;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using Microsoft.IdentityModel.Tokens;
using static System.Runtime.InteropServices.JavaScript.JSType;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IInvestorService _investorService;
        private readonly IFounderService _founderService;   
        private readonly IJWTService _jWTService;
        private readonly IUserService _userService;
        private readonly INotficationService _notificationService;
        private readonly IHelper _helper;
  

        public AuthController(IInvestorService investorService,IJWTService jWTService, IUserService userService, IHelper helper, IFounderService founderService, INotficationService notificationService)
        {
            _investorService = investorService;
            _jWTService = jWTService;
            _userService = userService;
            _helper = helper;
            _founderService = founderService;
            _notificationService = notificationService;
        }
        [HttpPost("register-investor")]
        public IActionResult RegisterInvestor([FromForm] Dtos.InvestorDto investorDto)
        {

            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
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
                var superAdmin = _userService.GetByEmail("SuperAdmin@gmail.com");
                NotificationDto notification = new NotificationDto
                {
                    Title = "New Investor Registration",
                    Body = $"New investor {investorDto.User.FirstName} {investorDto.User.LastName} has just registered an account.",
                    UserTypeTo = (int)UserType.Staff,
                    UserIdTo = superAdmin.Id,
                 
                };
                _notificationService.SendNotification(notification,investor.UserId, (int)UserType.Investor);
          

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

        [HttpPost("register-founder")]

        public IActionResult RegisterFounder([FromForm] FounderDto userDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
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
            var picpath = _helper.UploadFile(userDto.User.PicFile, "founder", "profilePic");
            var frontIdPath = _helper.UploadFile(userDto.User.FrontIdPicFile, "founder", "nationalIdPic");
            var backIdPath = _helper.UploadFile(userDto.User.BackIdPicFile, "founder", "nationalIdPic");

            userDto.User.ProfilePicPath = picpath;
            userDto.User.FrontIdPicPath = frontIdPath;
            userDto.User.BackIdPicPath = backIdPath;
            var result = _founderService.Add(userDto,null);
            if (result > 0)
            {
                var superAdmin = _userService.GetByEmail("SuperAdmin@gmail.com");
                NotificationDto notification = new NotificationDto
                {
                    Title = "New Foudner Registration",
                    Body = $"New Founder {userDto.User.FirstName} {userDto.User.LastName} has just registered an account.",
                    UserTypeTo = (int)UserType.Staff,
                    UserIdTo = superAdmin.Id,

                };
                _notificationService.SendNotification(notification, result, (int)UserType.Founder);
               
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = "Founder registered successfully.",
                    Data = null,
                    StatusCode = StatusCodes.Status201Created
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "An error occurred while registering the founder.",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }



        [HttpPost("login-staff")]
        public IActionResult LoginStaff([FromBody] UserLoginDto loginDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
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
            if (user == null||user.UserType!=(int)UserType.Staff || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword))
            {
                return BadRequest(new ResponseDto<object>
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


        [HttpPost("login")]
        public async Task<IActionResult> Login([FromBody] UserLoginDto loginDto)
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
            var test = BCrypt.Net.BCrypt.HashPassword(loginDto.Password);

            var user = _userService.GetByEmail(loginDto.Email);

            if (user == null
                ||user.UserType==(int)UserType.Staff 
                || !BCrypt.Net.BCrypt.Verify(loginDto.Password, user.HashedPassword)
                || user.Status==(int)UserStatus.Inactive
                || user.Status == (int)UserStatus.Deleted
                )
            {
                return BadRequest(new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Invalid email or password.",
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            var token = _jWTService.GenerateToken(user);
            //await _notificationService.NotifyUser(user.Id.ToString());
            return Ok(new ResponseDto<string>
            {
                Data = token,
                IsSuccess = true,
                Message = "Login successful.",
                StatusCode = StatusCodes.Status200OK
            });
        }


        [Authorize]
        [HttpGet("refresh-token")]
        public IActionResult GetUser()
        {
            var userId = User.GetUserId();
            if (userId==null)
            {
                return Unauthorized(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Unauthorized: user email not found in claims.",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }
            var user = _userService.GetById(userId??0);
            if (user == null)
            {
                return BadRequest(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "User not found.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                });
            }
            var token = _jWTService.GenerateToken(user);
            
            return Ok(new ResponseDto<string>
            {
                IsSuccess = true,
                Message = "User retrieved successfully.",
                Data = token,
                StatusCode = StatusCodes.Status200OK
            });
        }



        [HttpGet("appropriate-feedback-users")]
        [Authorize]
        public async Task<IActionResult> GetAppropiateUsersForFeedback()
        {
            try
            {
                var userIdClaim = User.FindFirst("id");
                if (userIdClaim == null || !int.TryParse(userIdClaim.Value, out int userId))
                {
                    return Unauthorized(new ResponseDto<string>
                    {
                        IsSuccess = false,
                        StatusCode = 401,
                        Message = "User ID not found in claims",
                        Data = "User ID not found in claims"
                    });
                }

                var users = await _userService.GetAppropiateUserForFeedback(userId);

                return Ok(new ResponseDto<List<DropdownDto>>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Appropriate users retrieved successfully",
                    Data = users
                });
            }
            catch (DbUpdateException ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Database error occurred",
                    Data = "Database error occurred"
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "An unexpected error occurred",
                    Data = "An unexpected error occurred"
                });
            }
        }





    }
}
