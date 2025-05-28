using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class UserRepo: Repo<User>, IUserRepo
    {
        private readonly AppDbContext _db;
        public UserRepo(AppDbContext db) : base(db)
        {
        }
    }
}
