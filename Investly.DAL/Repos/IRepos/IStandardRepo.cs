using Investly.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos.IRepos
{
    public interface IStandardRepo:IRepo<Standard>
    {
        public List<CategoryStandard> categoryStandards(int CategoryId);
    }
}
