using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos.IRepos
{
    public interface IAnalysisRepo
    {
        public int GetTotalBusinessIdeasCount(int activeBusinessStatus);
        public int GetTotalFoundersCount(int activeUserStatus);
        public int GetTotalInvestorsCount(int activeUserStatus);
        public int GetTotalAcceptedContactRequestsCount(int acceptedContactRequestStatus);
        int GetFoundersJoinedCount(int activeUserStatus, DateTime startDate, DateTime endDate);
        int GetInvestorsJoinedCount(int activeUserStatus, DateTime startDate, DateTime endDate);
        int GetIdeasSubmittedCount(int activeBusinessStatus, DateTime startDate, DateTime endDate);
        IQueryable<object> GetContactRequestsCountByMonth(int acceptedContactRequestStatus);
        IQueryable<object> GetIdeasCountByCategory(int activeBusinessStatus);
        IQueryable<object> GetMostActiveInvestors(int topN);
        IQueryable<object> GetMostSupportedFounders(int acceptedContactRequestStatus, int topN);
        IQueryable<object> GetUserCountsByGovernment(int activeUserStatus);
        IQueryable<object> GetUserCountsByCity(int activeUserStatus);
        IQueryable<object> GetBusinessIdeasCountByStage(int activeBusinessStatus);
        IQueryable<object> GetAvgAiRatePerCategory(int activeBusinessStatus);
        IQueryable<object> GetIdeasCountByInvestmentType(int activeBusinessStatus);

    }
}
