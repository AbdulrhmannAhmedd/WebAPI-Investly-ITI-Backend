using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class GovermentRepo:Repo<Government>, IGovernmentRepo
    {
        private readonly AppDbContext _appDbContext;
        public GovermentRepo(AppDbContext appDbContext) : base(appDbContext)
        {
            
                _appDbContext = appDbContext;
            
        }

        

    }
}
