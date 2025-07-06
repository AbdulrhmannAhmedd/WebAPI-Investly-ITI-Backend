using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;

namespace Investly.DAL.Repos
{
    public class CategoryStandardRepo : Repo<CategoryStandard>, ICategoryStandardRepo
    {
        private readonly AppDbContext db;
        public CategoryStandardRepo(AppDbContext _db) : base(_db)
        {
            db = _db;
        }
    }
}