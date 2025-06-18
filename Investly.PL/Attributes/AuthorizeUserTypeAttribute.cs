using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;
using System.Security.Claims;

namespace Investly.PL.Attributes
{
    public class AuthorizeUserTypeAttribute :Attribute, IAuthorizationFilter
    {
        private readonly int _userType;
        private readonly IUserService _userServicel;
        public AuthorizeUserTypeAttribute(int userType, IUserService userService)
        {
            _userType = userType;
            _userServicel = userService;
        }
        public void OnAuthorization(AuthorizationFilterContext context)
        {
            var user = context.HttpContext.User;
            if (!user.Identity.IsAuthenticated)
            {
                context.Result = new JsonResult(new ResponseDto<object>
                {
                    Data = null,
                    IsSuccess = false,
                    Message = "Unauthorized:user is not authenticated",
                    StatusCode = StatusCodes.Status403Forbidden
                })
                {
                    StatusCode = StatusCodes.Status403Forbidden
                };
            }
            else
            {
                var userTypeClaim = user.Claims.FirstOrDefault(c => c.Type == "userType");
                var userStatusClaim = user.Claims.FirstOrDefault(c => c.Type == "status");
                var userEmailClaim = user.Claims.FirstOrDefault(c => c.Type == ClaimTypes.Email);
                var usertokenVersionClaim= user.Claims.FirstOrDefault(c => c.Type == "tokenVersion");  
                var userFromDb = _userServicel.GetByEmail(userEmailClaim?.Value);
                int? dbTokenVersion = userFromDb?.TokenVersion;
                int.TryParse(usertokenVersionClaim?.Value, out var claimTokenVersion);
                bool t = claimTokenVersion !=( dbTokenVersion ?? 0);
                if (
                    userFromDb == null
                    || userTypeClaim == null
                    || int.Parse(userTypeClaim.Value) != _userType
                    ||t

                    )
                {
                    context.Result = new JsonResult(new ResponseDto<object>
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "Unauthorized: user type does not match",
                        StatusCode = StatusCodes.Status401Unauthorized
                    })
                    {
                        StatusCode = StatusCodes.Status401Unauthorized
                    };
                }else if (userStatusClaim.Value != userFromDb.Status.ToString())
                {
                    context.Result = new JsonResult(new ResponseDto<object>
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "data mismatch",
                        StatusCode = StatusCodes.Status409Conflict
                    })
                    {
                        StatusCode = StatusCodes.Status409Conflict
                    };

                }
          
            }

        }
    }
}
