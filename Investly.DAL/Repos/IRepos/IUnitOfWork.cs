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
        public IBusinessRepo BusinessRepo { get; } 
        public IGovernmentRepo GovernmentRepo { get; }
        public ICityRepo CityRepo { get; }
        public IFounderRepo FounderRepo { get; }
        public INotificationRepo NotificationRepo { get; }
        public IBusinessStandardAnswerRepo BusinessStandardAnswerRepo { get; }
        public ICategoryRepo CategoryRepo { get; }
        public IStandardRepo StandardRepo { get; }
        public IFeedbackRepo FeedbackRepo { get; }
        public int Save();
        public void Dispose();

        public IInvestorContactRequestRepo InvestorContactRequestRepo { get; }


    }
}
