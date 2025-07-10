using Investly.DAL.Entities;
using Investly.PL.Attributes;
using Investly.PL.BL;
using Investly.PL.Dtos;
using Investly.PL.Extentions;
using Investly.PL.General;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace Investly.PL.Controllers.Founder
{
    [Route("api/founder/[controller]")]
    //[Authorize]
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

          var aiResponse = await _aiService.EvaluateIdea(businessDto.BusinessStandardAnswers, standards);
          /*  var aiResponse = @"
{
  ""standards"": [
    {
      ""standardCategoryId"": 3,
      ""name"": ""Attraction & Destination Appeal"",
      ""weight"": 15,
      ""achievementScore"": 100,
      ""weightedContribution"": 15,
      ""feedback"": ""Your answer effectively highlights the unique features of your destination, including natural landscapes, cultural heritage, and hospitality. To enhance it further, consider adding specific examples of festivals or local cuisine to make it even more appealing.""
    },
    {
      ""standardCategoryId"": 4,
      ""name"": ""Customer Experience & Satisfaction"",
      ""weight"": 20,
      ""achievementScore"":100,
      ""weightedContribution"": 20,
      ""feedback"": ""Your response is insufficient as it only contains 'ttt'. Please provide detailed strategies or practices you implement to ensure a positive experience for your guests, such as customer service training, feedback mechanisms, or personalized services.""
    },
    {
      ""standardCategoryId"": 5,
      ""name"": ""Accommodation & Facilities"",
      ""weight"": 25,
      ""achievementScore"": 100,
      ""weightedContribution"": 25,
      ""feedback"": ""Your answer is lacking as it only contains 'ttt'. Please describe the types of accommodations you offer, their amenities, and any unique facilities that enhance the guest experience.""
    },
    {
      ""standardCategoryId"": 6,
      ""name"": ""Transportation & Accessibility"",
      ""weight"": 25,
      ""achievementScore"": 100,
      ""weightedContribution"": 25,
      ""feedback"": ""Your response is inadequate as it only contains 'tt'. Please elaborate on the transportation options available, such as public transport, shuttle services, or proximity to major transport hubs, to demonstrate accessibility for visitors.""
    },
    {
      ""standardCategoryId"": 7,
      ""name"": ""Cultural & Local Engagement"",
      ""weight"": 15,
      ""achievementScore"": 100,
      ""weightedContribution"": 15,
      ""feedback"": ""Your answer is insufficient as it only contains 'ttt'. Please provide specific examples of how your project incorporates local culture and engages the community, such as partnerships with local artists or cultural events.""
    }
  ],
  ""totalWeightedScore"": 100,
  ""generalFeedback"": ""Your strengths lie in the appeal of your destination, but significant improvements are needed in the areas of customer experience, accommodation, transportation, and cultural engagement. Focus on providing detailed and specific information in your responses to enhance your overall evaluation.""
}";*/
            var options = new JsonSerializerOptions { PropertyNameCaseInsensitive = true };

            var aiResultJson = JsonSerializer.Deserialize<AiBusinessEvaluationDto>(aiResponse, options);
           
            return Ok(new ResponseDto<AiBusinessEvaluationDto>
            {
                Data = aiResultJson,
                IsSuccess = true,
                Message = "Business idea evaluated successfully.",
                StatusCode = StatusCodes.Status200OK
            });


        }

        [HttpPost("add-ai-evaluation")]
        public async Task<IActionResult> AddAiBusinessEvaluation([FromBody] AiBusinessEvaluationDto aiEvaluationResult)
        {
            if (aiEvaluationResult == null)
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "Invalid AI evaluation data.",
                    StatusCode = StatusCodes.Status400BadRequest
                });
            }
            int res = _businessService.AddBusinessIdeaAiEvaluation(aiEvaluationResult, User.GetUserId() ?? 0);
            if (res > 0)
            {
                return Ok(new ResponseDto<string>
                {
                    IsSuccess = true,
                    Message = "AI evaluation result added successfully.",
                    StatusCode = StatusCodes.Status200OK


                });
            }
            else
            {
                return BadRequest(new ResponseDto<string>
                {
                    IsSuccess = false,
                    Message = "Failed to add AI evaluation result.",
                    StatusCode = StatusCodes.Status500InternalServerError
                });
            }

        }

        [HttpPost]
        public async Task<IActionResult> AddBusinessIdea([FromForm] BusinessDto BusinessIdea)
        {

            if (BusinessIdea.IdeaFile != null)
            {
                var Filepath = _helper.UploadFile(BusinessIdea.IdeaFile, "founder", "IdeaFile");
                BusinessIdea.FilePath = Filepath;
            }
            if (BusinessIdea.ImageFiles?.Count() > 0)
            {
                var imagePaths = new List<string>();
                foreach (var imageFile in BusinessIdea.ImageFiles)
                {
                    var imagePath = _helper.UploadFile(imageFile, "founder", "BusinessImages");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        imagePaths.Add(imagePath);
                    }
                }
                BusinessIdea.Images = string.Join(";", imagePaths);

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


        [HttpPut]
        public async Task<IActionResult> UpdateBusinessIdea([FromForm] BusinessDto BusinessIdea)
        {

            int res = _businessService.UpdateBusinessIdea(BusinessIdea, User.GetUserId());
            if (res > 0)
            {
                var response = new ResponseDto<BusinessDto>
                {
                    Data = BusinessIdea,
                    IsSuccess = true,
                    StatusCode = StatusCodes.Status200OK,
                    Message = "Business Idea updated Sucessfully"
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
                    Message = "Business Idea updated Failed"
                };
                return Ok(response);
            }
        }



        [HttpGet("all")]
        public IActionResult GetFouderIdeas()
        {
            var result = _businessService.GetFounderBusinessIdeas(User.GetUserId() ?? 0);

            return Ok(new ResponseDto<List<BusinessDto>>
            {
                Data = result,
                IsSuccess = result.Count > 0,
                Message = result.Count > 0
                    ? "Business ideas retrieved successfully."
                    : "No business ideas found.",
                StatusCode = StatusCodes.Status200OK
            });
        }

        [HttpPut("{Id}")]
        public IActionResult DeleteBusinessIdea(int Id)
        {
            var result = _businessService.UpdateBusinessStatus(Id, BusinessIdeaStatus.Deleted,null);
            if (result > 0)
            {
                return Ok(new ResponseDto<string> { Data = "ok", IsSuccess = true, Message = "business idea was deleted succeffully" });
            }
            else
            {
                return NotFound(new ResponseDto<string> { Data = "ok", IsSuccess = false, Message = "delelation faild" });
            }


        }
    }
}
