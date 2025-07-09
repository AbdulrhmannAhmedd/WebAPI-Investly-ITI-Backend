using Investly.DAL.Entities;
using Investly.PL.Dtos;
using Investly.PL.General;
using System.Collections.Generic;

namespace Investly.PL.IBL
{
    public interface IBusinessService
    {
        #region Admin
        public BusinessListDto GetAllBusinesses(BusinessSearchDto searchDto);
        public int SoftDeleteBusiness(int businessId, int? loggedUserId, string? loggedInEmail);
        public int UpdateBusinessStatus(int businessId, BusinessIdeaStatus newStatus, int? loggedUserId,string? loggedInEmail=null, string? rejectedReason = null);
        public BusinessCountsDto GetBusinessIdeasCounts();
        #endregion

        public int AddBusinessIdea(BusinessDto BusinessIdea,int? LoggedInUser);
        public int UpdateBusinessIdea(BusinessDto BusinessIdea, int? LoggedInUser);
        public int AddBusinessIdeaAiEvaluation(AiBusinessEvaluationDto dto, int LoggedInUser);
        public List<BusinessDto> GetFounderBusinessIdeas(int LoggedInUserIdFounder );
        public Task<BusinessListDtoForExplore> GetAllBusinessesForExploreAsync(BusinessSeachForExploreDto searchDto, int? loggedInUser);
        public BusinessSeachForExploreDto ApplyInvestorPreferences(BusinessSeachForExploreDto searchDto, Investor? investorDetails);
        public bool ApplyStageFilter(BusinessSeachForExploreDto searchDto, Business item, Investor? investorDetails);
        public BusinessDetailsDto GetBusinessDetails(int businessId, int? loggedInUser);
    }
}