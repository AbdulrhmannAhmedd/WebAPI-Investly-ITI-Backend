using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;

namespace Investly.DAL.Repos
{
    public class BusinessRepo : Repo<Business>, IBusinessRepo
    {
        private readonly AppDbContext _db;

        public BusinessRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
        public Tuple<int, int, int, int> GetBusinessCountsByStatus(int activeStatus, int inactiveStatus, int rejectedStatus, int pendingStatus, int deletedStatus)
        {
            var allBusinesses = _db.Businesses
                                   .Where(b => b.IsDrafted == false && b.Status != deletedStatus) 
                                   .ToList();
            int totalActive = allBusinesses.Count(b => b.Status == activeStatus);
            int totalInactive = allBusinesses.Count(b => b.Status == inactiveStatus);
            int totalRejected = allBusinesses.Count(b => b.Status == rejectedStatus);
            int totalPending = allBusinesses.Count(b => b.Status == pendingStatus);

            return new Tuple<int, int, int, int>(totalActive, totalInactive, totalRejected, totalPending);
        }
    }
}
