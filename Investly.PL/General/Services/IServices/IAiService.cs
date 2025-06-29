using Investly.PL.Dtos;

namespace Investly.PL.General.Services.IServices
{
    public interface IAiService
    {
        Task<string>EvaluateIdea(string ideaText, List<StandardDto> standards);
    }
}
