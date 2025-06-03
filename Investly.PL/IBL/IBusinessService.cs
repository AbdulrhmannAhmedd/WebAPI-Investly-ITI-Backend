using Investly.PL.Dtos;
using Investly.PL.General;
using System.Collections.Generic;

namespace Investly.PL.IBL
{
    public interface IBusinessService
    {
        public BusinessListDto GetAllBusinesses(BusinessSearchDto searchDto);
        public int SoftDeleteBusiness(int businessId, int? loggedUserId);
        public int UpdateBusinessStatus(int businessId, BusinessIdeaStatus newStatus, int? loggedUserId,string? rejectedReason = null);
        public BusinessCountsDto GetBusinessIdeasCounts();

    }
}