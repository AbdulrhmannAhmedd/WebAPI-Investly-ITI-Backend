using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;

namespace Investly.DAL.Repos
{
    public class FeedbackRepo:Repo<Feedback>, IFeedbackRepo
    {
        private readonly AppDbContext _db;

        public FeedbackRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }
    }
}
