using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    internal class FounderRepo : Repo<Founder>,IFounderRepo
    {
        private readonly AppDbContext _db;
        public FounderRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
