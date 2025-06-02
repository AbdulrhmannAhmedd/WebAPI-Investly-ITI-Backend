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
    }
}
