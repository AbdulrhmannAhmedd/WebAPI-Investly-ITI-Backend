using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
   public class CityRepo: Repo<City>,ICityRepo
    {
        private readonly AppDbContext _appDbContext;
        public CityRepo(AppDbContext appDbContext):base(appDbContext) 
        {
            _appDbContext = appDbContext;
        }
    }
}
