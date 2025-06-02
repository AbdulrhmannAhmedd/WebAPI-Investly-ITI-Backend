using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class UnitOfWork:IUnitOfWork
    {
        private readonly AppDbContext _db;
        private IInvestorRepo _InvestorRepo;
        private IUserRepo _UserRepo;
<<<<<<< HEAD
        private IBusinessRepo _BusinessRepo;

=======
        private IGovernmentRepo _GovernmentRepo;
        private ICityRepo _CityRepo;
>>>>>>> 4d53adaf67a365785fd38eecd30404860c64ab31
        public IInvestorRepo InvestorRepo => _InvestorRepo ??= new InvestorRepo(_db);
        public IUserRepo UserRepo => _UserRepo ??= new UserRepo(_db);
        public IGovernmentRepo GovernmentRepo => _GovernmentRepo ??= new GovermentRepo(_db);
        public ICityRepo CityRepo => _CityRepo ??= new CityRepo(_db);

        public IBusinessRepo BusinessRepo => _BusinessRepo ??= new BusinessRepo(_db);


        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            
        }
        public int Save()
        {
          int res=  _db.SaveChanges();
          return res;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
