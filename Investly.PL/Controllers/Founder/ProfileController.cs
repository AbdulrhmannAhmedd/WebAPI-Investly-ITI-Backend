using Investly.PL.Attributes;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace Investly.PL.Controllers.Founder
{
    [Route("api/founder/[controller]")]
    [ApiController]
    [TypeFilter(typeof(AuthorizeUserTypeAttribute), Arguments = new object[] { (int)UserType.Founder })]

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

    }
}
