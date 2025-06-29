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
        public ProfileController(IInvestorService investorService,  IHelper helper)
        {
            _investorService=investorService;
            _helper=helper;
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


            if (data.User.PicFile != null)
            {
                if (!string.IsNullOrEmpty(oldinvestor.User.ProfilePicPath))
                {
                    var deleteResult = _helper.DeleteFile(oldinvestor.User.ProfilePicPath);

                }

                var picpath = _helper.UploadFile(data.User.PicFile, "investor", "profilePic");
                data.User.ProfilePicPath = picpath;
            }
            if (data.User.FrontIdPicFile != null)
            {
                if (!string.IsNullOrEmpty(oldinvestor.User.FrontIdPicPath))
                {
                    _helper.DeleteFile(oldinvestor.User.FrontIdPicPath);
                }
                var frontIdPath = _helper.UploadFile(data.User.FrontIdPicFile, "investor", "nationalIdPic");
                data.User.FrontIdPicPath = frontIdPath;
            }
            if (data.User.BackIdPicFile != null)
            {
                if (!string.IsNullOrEmpty(oldinvestor.User.BackIdPicPath))
                {
                    _helper.DeleteFile(oldinvestor.User.BackIdPicPath);
                }
                var backIdPath = _helper.UploadFile(data.User.BackIdPicFile, "investor", "nationalIdPic");
                data.User.BackIdPicPath = backIdPath;
            }

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
    }
}
