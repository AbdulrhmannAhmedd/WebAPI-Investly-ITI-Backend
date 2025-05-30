using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
   public class InvestorRepo: Repo<Investor>, IInvestorRepo
    {
        private readonly AppDbContext _db;
        public InvestorRepo(AppDbContext db) : base(db)
        {
            this._db = db;
        }
        public Tuple<int, int> GetTotalActiveInactive(int activeStatus, int InactiveStatus)
        {
            var total = _db.Investors.Include(i=>i.User).ToList();
            var totalInactive =total.Count(i=>i.User.Status==InactiveStatus);
            var totalActive = total.Count(i => i.User.Status == activeStatus);
            return new Tuple<int, int>(totalActive, totalInactive);
        }
    }
}
