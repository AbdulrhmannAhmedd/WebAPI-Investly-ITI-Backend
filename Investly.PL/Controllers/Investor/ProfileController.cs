using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Investor
{
    [Route("api/investor/[controller]")]
    [TypeFilter(typeof(AuthorizeUserTypeAttribute), Arguments = new object[] { (int)UserType.Investor })]
    public class ProfileController : Controller
    {
        private readonly IInvestorService _investorService;
        private readonly IHelper _helper;
        public ProfileController(IInvestorService investorService, IHelper helper)
        {
            _investorService = investorService;
            _helper = helper;
        }
        [HttpGet]
        public IActionResult GetProfileData()
        {
            var res = _investorService.GetInvestorByUserId(User.GetUserId());
            if (res == null)
            {
                var response = new ResponseDto<InvestorDto>
                {
                    Data = res,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status404NotFound,
                    Message = "investor retrived failed"
                };
                return NotFound(response);
            }
            else
            {
                var response = new ResponseDto<InvestorDto>
                {
                    Data = res,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "investor retrived successfully"
                };
                return Ok(response);
            }
        }
        [HttpPut]
        public ResponseDto<InvestorDto> Put([FromForm] InvestorDto data)
        {

            var oldinvestor = _investorService.GetById(data.Id ?? 0);

            data.User.Status = (int)UserStatus.Pending;
            var result = _investorService.Update(data, User.GetUserId());
            ResponseDto<InvestorDto> response;
            if (result > 0)
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = true,
                    Message = "Investor updated successfully.",
                    Data = _investorService.GetById(data.Id ?? 0),
                    StatusCode = StatusCodes.Status200OK,
                    RefreshTokenRequired = true,
                };

            }
            else
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update investor.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };


            }
            return response;
        }
        [HttpPatch]
        public ResponseDto<InvestorDto> UpdateProfile(IFormFile profilepic)
        {

            var oldinvestor = _investorService.GetInvestorByUserId(User.GetUserId());


            if (profilepic != null)
            {
                if (!string.IsNullOrEmpty(oldinvestor.User.ProfilePicPath))
                {
                    var deleteResult = _helper.DeleteFile(oldinvestor.User.ProfilePicPath);

                }

                var picpath = _helper.UploadFile(profilepic, "investor", "profilePic");
                oldinvestor.User.ProfilePicPath = picpath;
            }
      
            var result = _investorService.UpdateProfilePicture(oldinvestor.User.ProfilePicPath, User.GetUserId());
            ResponseDto<InvestorDto> response;
            if (result > 0)
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = true,
                    Message = "Investor Profile pic updated successfully.",
                    Data = _investorService.GetById(oldinvestor.Id ?? 0),
                    StatusCode = StatusCodes.Status200OK,
                    RefreshTokenRequired = true,
                };

            }
            else
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update profile pic investor.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };


            }
            return response;
        }
        [HttpPatch("NationalId")]
        public ResponseDto<InvestorDto> UpdateNationalId(IFormFile FrontIdPic, IFormFile BackIdPic)
        {

            var oldinvestor = _investorService.GetInvestorByUserId(User.GetUserId());


            if (FrontIdPic != null)
            {
                if (!string.IsNullOrEmpty(oldinvestor.User.FrontIdPicPath))
                {
                    var deleteResult = _helper.DeleteFile(oldinvestor.User.FrontIdPicPath);

                }

                var picpath = _helper.UploadFile(FrontIdPic, "investor", "profilePic");
                oldinvestor.User.FrontIdPicPath = picpath;
            }
            if (BackIdPic != null)
            {
                if (!string.IsNullOrEmpty(oldinvestor.User.BackIdPicPath))
                {
                    var deleteResult = _helper.DeleteFile(oldinvestor.User.BackIdPicPath);

                }

                var picpath = _helper.UploadFile(BackIdPic, "investor", "profilePic");
                oldinvestor.User.BackIdPicPath = picpath;
            }
           
            var result = _investorService.UpdateNationalId(oldinvestor.User.FrontIdPicPath, oldinvestor.User.BackIdPicPath, User.GetUserId());
            ResponseDto<InvestorDto> response;
            if (result > 0)
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = true,
                    Message = "Investor Profile pic updated successfully.",
                    Data = oldinvestor,
                    StatusCode = StatusCodes.Status200OK,
                    RefreshTokenRequired = true,
                };

            }
            else
            {
                response = new ResponseDto<InvestorDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update profile pic investor.",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };


            }
            return response;
        }
        [HttpPatch("ChangePassword")]
        public ResponseDto<ChangePasswordDto> ChangePassword([FromBody]ChangePasswordDto passwordDto)
        {

            var result = _investorService.ChangePassword(passwordDto, User.GetUserId());
            ResponseDto<ChangePasswordDto> response;
            if (result == 0)
            {
                response = new ResponseDto<ChangePasswordDto>
                {
                    IsSuccess = false,
                    Message = "Investor Not Found.",
                    Data = null,
                    StatusCode = StatusCodes.Status404NotFound,
                  
                };

            }
            else if (result == -2)
            {
                response = new ResponseDto<ChangePasswordDto>
                {
                    IsSuccess = false,
                    Message = "incorrect Current Password .",
                    Data = null,
                    StatusCode = StatusCodes.Status400BadRequest,
                    
                };

            }
            else if(result>0)
            {
                response = new ResponseDto<ChangePasswordDto>
                {
                    IsSuccess = true,
                    Message = "Investor Password updated .",
                    Data = null,
                    StatusCode = StatusCodes.Status200OK,
                   
                };
            }
            else
            {
                response = new ResponseDto<ChangePasswordDto>
                {
                    IsSuccess = false,
                    Message = "Failed to update Password ",
                    Data = null,
                    StatusCode = StatusCodes.Status500InternalServerError
                };


            }
            return response;
        }
    }

   
    }

