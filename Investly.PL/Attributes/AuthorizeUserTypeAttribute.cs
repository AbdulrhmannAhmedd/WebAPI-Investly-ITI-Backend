using Investly.PL.Dtos;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Filters;

namespace Investly.PL.Attributes
{
    public class AuthorizeUserTypeAttribute :Attribute, IAuthorizationFilter
    {
        private readonly int _userType;
        public AuthorizeUserTypeAttribute(int userType)
        {
            _userType = userType;
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
                });
            }
            else
            {
                var userTypeClaim = user.Claims.FirstOrDefault(c => c.Type == "UserType");
                if (userTypeClaim == null || int.Parse(userTypeClaim.Value) != _userType)
                {
                    context.Result = new JsonResult(new ResponseDto<object>
                    {
                        Data = null,
                        IsSuccess = false,
                        Message = "Unauthorized: user type does not match",
                        StatusCode = StatusCodes.Status401Unauthorized
                    });
                }
            }

        }
    }
}
