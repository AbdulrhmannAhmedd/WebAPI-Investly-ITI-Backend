using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
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
        }
     
    }
}
