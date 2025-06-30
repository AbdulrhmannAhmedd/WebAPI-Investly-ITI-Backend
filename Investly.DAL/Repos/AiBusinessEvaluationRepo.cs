using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class AiBusinessEvaluationRepo : Repo<AiBusinessStandardsEvaluation>, IAiBusinessEvaluationRepo
    {
        private readonly AppDbContext db;
        public AiBusinessEvaluationRepo(AppDbContext db) : base(db)
        {
            this.db = db;
        }
    }
}
