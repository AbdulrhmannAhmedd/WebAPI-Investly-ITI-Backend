using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;

namespace Investly.PL.Controllers.Founder
{
    [Route("api/founder/[controller]")]
    [ApiController]
    //[TypeFilter(typeof(AuthorizeUserTypeAttribute), Arguments = new object[] { (int)UserType.Founder })]

    public class ProfileController : ControllerBase
    {
   
        private readonly IFounderService _founderService;

        public ProfileController(  IFounderService founderService)
        {

            _founderService = founderService;
            
        }
        [HttpGet]
        public ResponseDto<FounderDto> GetProfileData()
        {
            var res = _founderService.GetFounderByUserId(User.GetUserId() ?? 0);
           
            if (res != null)
            {
                return new ResponseDto<FounderDto>
                {
                    Data = res,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "data retrived successfully"
                };

            }
            else
            {
                return new ResponseDto<FounderDto>
                {
                    Data = res,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "something went wrong"
                };

            }
        }
        [HttpPut("{email}")]
        public IActionResult UpdateFounder(string email, [FromBody] UpdateFounderDto founderDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = string.Join("; ", errors),
                    Data = null
                };

                return BadRequest(response); // StatusCode 400
            }

            try
            {
                var (isUpdated, founderData) = _founderService.UpdateFounderData(email, founderDto);

                var response = new ResponseDto<string>
                {
                    IsSuccess = true,
                    StatusCode = isUpdated ? 200 : 304,
                    Message = isUpdated ? "Update successful" : "No changes detected",
                    Data = "Update successful"
                };

                return StatusCode(response.StatusCode, response); // Either 200 or 304
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = ex.Message,
                    Data = null
                };

                return NotFound(response); // StatusCode 404
            }
            catch (DbUpdateException)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Database update error",
                    Data = null
                };

                return StatusCode(500, response);
            }
            catch (Exception)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "An unexpected error occurred",
                    Data = null
                };

                return StatusCode(500, response);
            }
        }


        [HttpPost("change-password")]
        public IActionResult ChangePassword([FromBody] ChangePasswordDto changePasswordDto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                bool hasCurrentPasswordError = errors.Any(e =>
                    e.Contains("Current password is incorrect") ||
                    e.Contains("User not found"));

                int statusCode = hasCurrentPasswordError ? 401 : 400;

                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = statusCode,
                    Message = string.Join("; ", errors),
                    Data = string.Join("; ", errors)
                };

                return StatusCode(statusCode, response);
            }

            try
            {
                var result = _founderService.ChangePassword(changePasswordDto);

                var response = new ResponseDto<string>
                {
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Password changed successfully",
                    Data = "Password changed successfully"
                };

                return Ok(response);
            }
            catch (ArgumentException ex)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = ex.Message,
                    Data = ex.Message
                };

                return BadRequest(response);
            }
            catch (KeyNotFoundException ex)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = ex.Message,
                    Data = ex.Message
                };

                return NotFound(response);
            }
            catch (DbUpdateException)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Database update error",
                    Data = "Database update error"
                };

                return StatusCode(500, response);
            }
            catch (Exception)
            {
                var response = new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "An unexpected error occurred",
                    Data = "An unexpected error occurred"
                };

                return StatusCode(500, response);
            }
        }

        [HttpPatch("profile-picture")]
        public IActionResult UpdateFounderProfilePicture([FromForm] UpdateProfilePicDto model)
        {
            try
            {
                var result = _founderService.UpdateProfilePicture(model);

                return Ok(new ResponseDto<string>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Profile picture updated successfully.",
                    Data = model.Email 
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception ex)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "An error occurred while updating the profile picture.",
                    Data = null
                });
            }
        }

        [HttpPatch("national-id")]
        public IActionResult UpdateNationalIdImages([FromForm] UpdateNationalIdDto model)
        {
            try
            {
                var result = _founderService.UpdateNationalIdImages(model); // this still returns bool

                var message = "National ID images updated successfully.";
                return Ok(new ResponseDto<string>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = message,
                    Data = message
                });
            }
            catch (KeyNotFoundException ex)
            {
                return NotFound(new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 404,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (ArgumentException ex)
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = null
                });
            }
            catch (Exception)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "An unexpected error occurred while updating national ID images.",
                    Data = null
                });
            }
        }



    }
}
