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
        private readonly AppDbContext _db;
        public CategroyRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public Tuple<int, int> GetTotalActiveInactive(int activeStatus, int inactiveStatus)
        {
            var allCategories = _db.Categories.AsQueryable();
            var totalActive = allCategories.Count(c => c.Status == activeStatus);
            var totalInactive = allCategories.Count(c => c.Status == inactiveStatus);
            return new Tuple<int, int>(totalActive, totalInactive);
        }

    }
}
