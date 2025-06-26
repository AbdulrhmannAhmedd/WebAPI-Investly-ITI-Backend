using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class StandardRepo : Repo<Standard>, IStandardRepo
    {
        private readonly AppDbContext db;
        public StandardRepo(AppDbContext _db) : base(_db)
        {
            db = _db;
        }

    }
}
