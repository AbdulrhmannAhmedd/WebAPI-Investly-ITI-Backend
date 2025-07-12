using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class PasswordResetTokenRepo : Repo<PasswordResetToken>, IPasswordResetTokenRepo
    {
        public PasswordResetTokenRepo(AppDbContext db) : base(db)
        {
        }
    }
}
