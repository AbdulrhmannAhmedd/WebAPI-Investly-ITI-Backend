using Investly.PL.Dtos;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Investly.PL.Controllers
{
    [Route("api/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpPost("create-feedback")]
        public async Task<IActionResult> CreateFeedback([FromBody] FeedbackCreateDto dto)
        {
            if (!ModelState.IsValid)
            {
                var errors = ModelState.Values
                    .SelectMany(v => v.Errors)
                    .Select(e => e.ErrorMessage)
                    .ToList();

                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = string.Join("; ", errors),
                    Data = string.Join("; ", errors)
                });
            }

            try
            {
                // Extract userId from claims
                int? userId = null;
                if (User.Identity != null && User.Identity.IsAuthenticated)
                {
                    var userIdClaim = User.FindFirst("id");
                    if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedId))
                    {
                        userId = parsedId;
                    }

                    var emailClaim = User.FindFirst(ClaimTypes.Email);
                    if (emailClaim != null)
                    {
                        dto.Email = emailClaim.Value;
                    }
                }

                
                // Pass userId to the service
                await _feedbackService.CreateFeedbackAsync(dto, userId);

                return Ok(new ResponseDto<string>
                {
                    IsSuccess = true,
                    StatusCode = 200,
                    Message = "Feedback submitted successfully.",
                    Data = "Feedback submitted successfully."
                });
            }
            catch (ValidationException ex)
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 400,
                    Message = ex.Message,
                    Data = ex.Message
                });
            }
            catch (DbUpdateException)
            {
                return StatusCode(500, new ResponseDto<string>
                {
                    IsSuccess = false,
                    StatusCode = 500,
                    Message = "Database update error",
                    Data = "Database update error"
                });
            }
            catch (Exception)
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
