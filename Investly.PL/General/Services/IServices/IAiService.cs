using Investly.PL.Dtos;

namespace Investly.PL.General.Services.IServices
{
    public interface IAiService
    {
        Task<string>EvaluateIdea(List<BusinessStandardAnswerDto> businessStandardAnswers , List<StandardCategoryDto> standards);
        Task<ResponseDto<List<StandardItemAiDto>>> GenerateStandardsForCategory(string category);
    }
}
