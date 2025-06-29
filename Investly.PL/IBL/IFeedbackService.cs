using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IFeedbackService
    {
        FeedbackListDto GetAllFeedbacks(FeedbackSearchDto searchDto);
        int DeleteFeedback(int feedbackId, int? loggedUserId, int actionType);
        FeedbackCountsDto GetFeedbackStatisticsCounts();
    }
}
