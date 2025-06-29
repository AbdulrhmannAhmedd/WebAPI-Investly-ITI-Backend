using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;

namespace Investly.DAL.Repos
{
    public class FeedbackRepo:Repo<Feedback>, IFeedbackRepo
    {
        private readonly AppDbContext _db;

        public FeedbackRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public Tuple<int, int> GetFeedbackCountsByStatus(int activeStatus, int inactiveStatus)
        {
           
            var allFeedbacks = _db.Feedbacks.ToList();

            int totalActive = allFeedbacks.Count(f => f.Status == activeStatus);
            int totalInactive = allFeedbacks.Count(f => f.Status == inactiveStatus);

            return new Tuple<int, int>(totalActive, totalInactive);
        }
    }
}
