using Investly.PL.Attributes;
using Investly.PL.BL;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Investly.PL.Controllers.Founder
{
    [Route("api/founder/[controller]")]
    [AuthorizeUserType(((int)UserType.Founder))]
    
    public class BusinessController : Controller
    {
        private readonly IBusinessService _businessService;
        private readonly IHelper _helper;
        public BusinessController(IBusinessService businessService, IHelper helper)
        {
            _businessService = businessService;
            _helper = helper;
        }
        [HttpPost]
        public IActionResult AddBusinessIdeaByFile([FromForm]BusinessDto BusinessIdea)
        {
           
            if (BusinessIdea.IdeaFile != null)
            {
                var Filepath = _helper.UploadFile(BusinessIdea.IdeaFile, "founder", "IdeaFile");
                BusinessIdea.FilePath = Filepath;
            }
           
            int res=_businessService.AddBusinessIdea(BusinessIdea,User.GetUserId());
            if(res>0)
            {
                var response = new ResponseDto<BusinessDto>
                {
                    Data = BusinessIdea,
                    IsSuccess = true,
                    StatusCode=StatusCodes.Status200OK,
                    Message="Business Idea Added Sucessfully"
                };
                return Ok(response);
            }
            else
            {
                var response = new ResponseDto<BusinessDto>
                {
                    Data = BusinessIdea,
                    IsSuccess = false,
                    StatusCode = StatusCodes.Status500InternalServerError,
                    Message = "Business Idea Added Failed"
                };
                return Ok(response );
            }
            return Ok();
        }
       
    
}
}
