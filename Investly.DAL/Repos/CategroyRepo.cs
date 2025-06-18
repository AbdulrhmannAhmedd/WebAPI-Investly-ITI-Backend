using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class CategroyRepo : Repo<Category>, ICategoryRepo
    {
        AppDbContext db;
        public CategroyRepo(AppDbContext _db) : base(_db)
        {
            db= _db;
        }
    }
}
