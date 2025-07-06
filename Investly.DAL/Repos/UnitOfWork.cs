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
        private IBusinessRepo _BusinessRepo;
        private IFounderRepo _FounderRepo;
        private IBusinessStandardAnswerRepo _BusinessAnswerRepo;
        private IInvestorContactRequestRepo _InvestorContactRequestRepo;
        private ICategoryRepo _CategoryRepo;
        private IStandardRepo _StandardRepo;


        private IGovernmentRepo _GovernmentRepo;
        private ICityRepo _CityRepo;
        private INotificationRepo _NotificationRepo;
        private IFeedbackRepo _FeedbackRepo;
        private IAiBusinessEvaluationRepo _AiBusinessEvaluationRepo;
        private ICategoryStandardRepo _CategoryStandardRepo;

        private IAnalysisRepo _AnalysisRepo;
        public IInvestorRepo InvestorRepo => _InvestorRepo ??= new InvestorRepo(_db);
        public IUserRepo UserRepo => _UserRepo ??= new UserRepo(_db);
        public IGovernmentRepo GovernmentRepo => _GovernmentRepo ??= new GovermentRepo(_db);
        public ICityRepo CityRepo => _CityRepo ??= new CityRepo(_db);
        public IFounderRepo FounderRepo => _FounderRepo ??= new FounderRepo(_db);

        public IBusinessRepo BusinessRepo => _BusinessRepo ??= new BusinessRepo(_db);
        public INotificationRepo NotificationRepo => _NotificationRepo ??= new NotificationRepo(_db);  
        public IFeedbackRepo FeedbackRepo => _FeedbackRepo ??= new FeedbackRepo(_db);

        public IBusinessStandardAnswerRepo BusinessStandardAnswerRepo=>_BusinessAnswerRepo ??= new BusinessStandardAnswerRepo(_db);
        public IInvestorContactRequestRepo InvestorContactRequestRepo => _InvestorContactRequestRepo ??= new InvestorContactRequestRepo(_db);
        public ICategoryRepo CategoryRepo=>_CategoryRepo??=new CategroyRepo(_db);
        public IStandardRepo StandardRepo=>_StandardRepo??new StandardRepo(_db);
        public IAiBusinessEvaluationRepo AiBusinessEvaluationRepo => _AiBusinessEvaluationRepo ??= new AiBusinessEvaluationRepo(_db);
        public ICategoryStandardRepo CategoryStandardRepo => _CategoryStandardRepo ??= new CategoryStandardRepo(_db);

        public IAnalysisRepo AnalysisRepo => _AnalysisRepo ??= new AnalysisRepo(_db);

        public UnitOfWork(AppDbContext db)
        {
            _db = db;
            
        }
        public int Save()
        {
          int res=  _db.SaveChanges();
          return res;
        }

        public async Task<int> SaveAsync ()
        {
            int res = await _db.SaveChangesAsync();
            return res;
        }

        public void Dispose()
        {
            _db.Dispose();
        }

    }
}
