using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investly.DAL.Entities;

namespace Investly.DAL.Repos.IRepos
{
    public interface IBusinessRepo:IRepo<Business>
    {
        Tuple<int, int, int, int> GetBusinessCountsByStatus(int activeStatus, int inactiveStatus, int rejectedStatus, int pendingStatus, int deletedUserStatus);
        int RemoveRangAiStandardEvaluations(int businessId);
        int RemoveRangeStandardAnswers(int businessId);


    }
}
