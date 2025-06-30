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
    [TypeFilter(typeof(AuthorizeUserTypeAttribute), Arguments = new object[] { (int)UserType.Founder })]

    public class BusinessController : Controller
    {
        private readonly IBusinessService _businessService;
        private readonly IStandardService _standardService;
        private readonly IHelper _helper;
        private readonly IAiService _aiService;

        public BusinessController(
              IBusinessService businessService
            , IHelper helper
            , IStandardService standardService
            , IAiService aiService

            )
        {
            _businessService = businessService;
            _helper = helper;
            _standardService = standardService;
            _aiService = aiService;
        }
        [HttpPost]
        public async Task<IActionResult> AddBusinessIdeaByFile([FromForm] BusinessDto BusinessIdea)
        {

            if (BusinessIdea.IdeaFile != null)
            {
                var Filepath = _helper.UploadFile(BusinessIdea.IdeaFile, "founder", "IdeaFile");
                BusinessIdea.FilePath = Filepath;
            }

            int res = _businessService.AddBusinessIdea(BusinessIdea, User.GetUserId());
            if (res > 0)
            {
                var response = new ResponseDto<BusinessDto>
                {
                    Data = BusinessIdea,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Business Idea Added Sucessfully"
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
                return Ok(response);
            }
        }

        [HttpPost("evaluate")]

        public async Task<IActionResult> EvaluateBusinessIdea([FromForm] BusinessDto businessDto)
        {
            if (businessDto == null)
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "Invalid business idea data.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            var standards = _standardService.GetStandardsCategories(businessDto.CategoryId);
            if (standards == null || standards.Count == 0)
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "No standards found for the selected category.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            

            //string ExtractedTxt;
            //using (var ms = new MemoryStream())
            //{
            //    await businessDto.IdeaFile.CopyToAsync(ms);
            //    ExtractedTxt = _helper.ExtractTxtFromFile(ms.ToArray(), businessDto.IdeaFile.FileName);

            //}
            // var aiResponse = await _aiService.EvaluateIdea(ExtractedTxt, standards);
            var aiResponse = @"
 {
  ""standards"": [
    {
      ""standardCategoryId"": 3,
      ""name"": ""What makes your tourism destination attractive to visitors?"",
      ""weight"": 15,
      ""achievementScore"": 70,
      ""weightedContribution"": 10.5,
      ""feedback"": ""The destination has unique features but lacks promotional visibility.""
    },
    {
      ""standardCategoryId"": 4,
      ""name"": ""How do you ensure a positive and enjoyable experience for your guests?"",
      ""weight"": 20,
      ""achievementScore"": 90,
      ""weightedContribution"": 18,
      ""feedback"": ""Excellent focus on customer service and feedback mechanisms.""
    },
    {
      ""standardCategoryId"": 5,
      ""name"": ""Describe the accommodation and facilities you provide."",
      ""weight"": 25,
      ""achievementScore"": 40,
      ""weightedContribution"": 10,
      ""feedback"": ""Accommodation is basic with room for improvement in amenities.""
    },
    {
      ""standardCategoryId"": 6,
      ""name"": ""How accessible is your destination via transportation?"",
      ""weight"": 25,
      ""achievementScore"": 60,
      ""weightedContribution"": 15,
      ""feedback"": ""Transport is available but could be more convenient for tourists.""
    },
    {
      ""standardCategoryId"": 7,
      ""name"": ""How does your project incorporate local culture and community?"",
      ""weight"": 15,
      ""achievementScore"": 80,
      ""weightedContribution"": 12,
      ""feedback"": ""Strong integration with local traditions and community support.""
    }
  ],
  ""totalWeightedScore"": 65.5
}";
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var aiResultJson = JsonSerializer.Deserialize<AiIdeaEvaluationResult>(aiResponse, options);

            return Ok(new ResponseDto<AiIdeaEvaluationResult>
            {
                Data = aiResultJson,
                IsSuccess = true,
                Message = "Business idea evaluated successfully.",
                StatusCode = StatusCodes.Status200OK
            });


        }
    }
}
