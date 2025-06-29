using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Admin
{
    [Route("api/admin/[controller]")]
    [ApiController]
    public class FeedbackController : ControllerBase
    {
        private readonly IFeedbackService _feedbackService;

        public FeedbackController(IFeedbackService feedbackService)
        {
            _feedbackService = feedbackService;
        }

        [HttpGet("all")] 
        public ResponseDto<FeedbackListDto> GetAllFeedbacks([FromQuery] FeedbackSearchDto searchDto)
        {
            var feedbacks = _feedbackService.GetAllFeedbacks(searchDto);
            return new ResponseDto<FeedbackListDto>
            {
                IsSuccess = true,
                Message = "Feedbacks retrieved successfully.",
                Data = feedbacks,
                StatusCode = StatusCodes.Status200OK
            };
        }

        [HttpPut("{id}/status-update")]
        public ResponseDto<object> UpdateFeedbackStatus(int id, [FromQuery] int actionType)
        {
            int? loggedUserId = null;
            var userIdClaim = User.FindFirst("id");
            if (userIdClaim != null && int.TryParse(userIdClaim.Value, out int parsedUserId))
            {
                loggedUserId = parsedUserId;
            }

            if (!Enum.IsDefined(typeof(UserStatus), actionType))
            {
                return new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Invalid action type provided. Please use a valid status code.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }

            var result = _feedbackService.DeleteFeedback(id, loggedUserId, actionType);
            ResponseDto<object> response;

            if (result > 0)
            {
                UserStatus statusForMessage = (UserStatus)actionType;
                string message = statusForMessage == UserStatus.Inactive ? "Feedback marked as inactive successfully." :
                                 statusForMessage == UserStatus.Deleted ? "Feedback marked as deleted successfully." :
                                 statusForMessage == UserStatus.Active ? "Feedback marked as active successfully." :
                                 "Feedback status updated successfully.";

                response = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = message,
                    Data = null,
                    StatusCode = StatusCodes.Status200OK
                };
            }
            else if (result == -1)
            {
                response = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Feedback not found.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound
                };
            }
            else if (result == -2)
            {
                response = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Invalid action type provided or not allowed for this operation.",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest
                };
            }
            else
            {
                response = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Failed to update feedback status.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };
            }
            return response;
        }

        [HttpGet("statistics-counts")]
        public ResponseDto<FeedbackCountsDto> GetFeedbackStatisticsCounts()
        {
            var counts = _feedbackService.GetFeedbackStatisticsCounts();
            return new ResponseDto<FeedbackCountsDto>
            {
                IsSuccess = true,
                Message = "Feedback statistics retrieved successfully.",
                Data = counts,
                StatusCode = StatusCodes.Status200OK
            };
        }
    }
}
