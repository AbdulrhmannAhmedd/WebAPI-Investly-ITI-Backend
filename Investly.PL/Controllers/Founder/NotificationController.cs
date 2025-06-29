using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Founder
{
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotficationService _notificationService;

        public NotificationController(INotficationService notificationService)
        {
            _notificationService = notificationService;
        }

        [HttpGet("user")]
        public IActionResult GetNotificationsForUser([FromQuery] NotificationSearchDto search)
        {
            var userId = User.GetUserId();

            if (userId == null)
            {
                return Unauthorized(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Unauthorized: User ID not found in token claims.",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var notifications = _notificationService.GetUserNotifications(search, userId.Value);

            if (notifications == null)
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while retrieving notifications.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }

            return Ok(new ResponseDto<PaginatedNotificationsDto>
            {
                IsSuccess = true,
                Message = "User notifications retrieved successfully.",
                Data = notifications,
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpGet("unread-count")]
        public ResponseDto<object> GetUnreadNotificationCount()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return new ResponseDto<object> 
                {
                    IsSuccess = false,
                    Message = "Unauthorized: User ID not found in token claims.",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                };
            }

            var res = _notificationService.getFounderNotificationUnreadCount(userId.Value);

            if (res >= 0)
            {
                return new ResponseDto<object>
                {
                    Data = res,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Unread notification count retrieved successfully"
                };
            }
            else
            {
                return new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status400BadRequest,
                    Message = "Something went wrong while retrieving unread notification count"
                };
            }
        }

        [HttpPut("mark-all-as-read")]
        public async Task<IActionResult> MarkAllAsRead()
        {
            var userId = User.GetUserId();
            if (userId == null)
            {
                return Unauthorized(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Unauthorized: User ID not found in token claims.",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            var result = await _notificationService.MarkAllUserNotificationsAsRead(userId.Value);

            if (result >= 0) // result indicates number of rows affected or 0 if none
            {
                return Ok(new ResponseDto<object>
                {
                    IsSuccess = true,
                    Message = result > 0 ? $"{result} notifications marked as read." : "No unread notifications found.",
                    Data = result,
                    StatusCode = StatusCodes.Status200OK
                });
            }
            else
            {
                return StatusCode(StatusCodes.Status500InternalServerError, new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "An error occurred while marking notifications as read.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }
        }
        [HttpPut("{id}/soft-delete")] // A specific route for soft-delete
        public IActionResult SoftDeleteNotification(int id)
        {
            var loggedInUserId = User.GetUserId(); // Get the ID of the logged-in user

            if (loggedInUserId == null)
            {
                return Unauthorized(new ResponseDto<object>
                {
                    IsSuccess = false,
                    Message = "Unauthorized: User ID not found in token claims.",
                    Data = null,
                    StatusCode = StatusCodes.Status401Unauthorized
                });
            }

            // Call the shared ChnageStatus method with the Deleted status enum value
            int res = _notificationService.ChnageStatus(id, (int)NotificationsStatus.Deleted, loggedInUserId);
            ResponseDto<object> Data;

            if (res > 0)
            {
                Data = new ResponseDto<object>
                {
                    IsSuccess = true,
                    Data = null,
                    Message = "Notification soft-deleted successfully.",
                    StatusCode = StatusCodes.Status200OK
                };
                return Ok(Data);
            }
            else
            {
                // Handle different error codes from ChnageStatus
                string errorMessage;
                int statusCode;
                switch (res)
                {
                    case -1:
                        errorMessage = "Invalid notification ID.";
                        statusCode = StatusCodes.Status400BadRequest;
                        break;
                    case -2:
                        errorMessage = "Notification not found.";
                        statusCode = StatusCodes.Status404NotFound;
                        break;
                    case -3:
                        errorMessage = "You do not have permission to delete this notification.";
                        statusCode = StatusCodes.Status403Forbidden; // Forbidden if not creator/recipient
                        break;
                    case -4:
                    default:
                        errorMessage = "An error occurred during notification soft-deletion.";
                        statusCode = StatusCodes.Status500InternalServerError;
                        break;
                }
                Data = new ResponseDto<object>
                {
                    IsSuccess = false,
                    Data = null,
                    Message = errorMessage,
                    StatusCode = statusCode
                };
                return StatusCode(statusCode, Data); // Use StatusCode for non-200 responses
            }
        }
    }
}
