using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos.IRepos
{
   public interface IUnitOfWork
    {
        public IInvestorRepo InvestorRepo { get; }
        public IUserRepo UserRepo { get; }
<<<<<<< HEAD
        public IBusinessRepo BusinessRepo { get; } 
=======
        public IGovernmentRepo GovernmentRepo { get; }
        public ICityRepo CityRepo { get; }
>>>>>>> 4d53adaf67a365785fd38eecd30404860c64ab31
        public int Save();
        public void Dispose();
    }
}
