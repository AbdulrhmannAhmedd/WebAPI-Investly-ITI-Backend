using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Microsoft.EntityFrameworkCore;


namespace Investly.DAL.Repos
{
    public class AnalysisRepo:IAnalysisRepo
    {
        private readonly AppDbContext _db;

        public AnalysisRepo(AppDbContext db)
        {
            _db = db;
        }

        public int GetTotalBusinessIdeasCount(int activeBusinessStatus)
        {
            return _db.Businesses.Count(b => b.Status == activeBusinessStatus && !b.IsDrafted);
        }

        public int GetTotalFoundersCount(int activeUserStatus)
        {
            return _db.Founders.Count(f => f.User.Status == activeUserStatus);
        }

        public int GetTotalInvestorsCount(int activeUserStatus)
        {
            return _db.Investors.Count(i => i.User.Status == activeUserStatus);
        }

        public int GetTotalAcceptedContactRequestsCount(int acceptedContactRequestStatus)
        {
            return _db.InvestorContactRequests.Count(icr => icr.Status == acceptedContactRequestStatus);
        }
        public int GetFoundersJoinedCount(int activeUserStatus, DateTime startDate, DateTime endDate)
        {
            return _db.Founders
                      .Include(f => f.User) 
                      .Count(f => f.User.Status == activeUserStatus &&
                                  f.User.CreatedAt >= startDate &&
                                  f.User.CreatedAt <= endDate);
        }

        public int GetInvestorsJoinedCount(int activeUserStatus, DateTime startDate, DateTime endDate)
        {
            return _db.Investors
                      .Include(i => i.User) 
                      .Count(i => i.User.Status == activeUserStatus &&
                                  i.User.CreatedAt >= startDate &&
                                  i.User.CreatedAt <= endDate);
        }

        public int GetIdeasSubmittedCount(int activeBusinessStatus, DateTime startDate, DateTime endDate)
        {
            return _db.Businesses
                      .Count(b => b.Status == activeBusinessStatus &&
                                  !b.IsDrafted && 
                                  b.CreatedAt >= startDate &&
                                  b.CreatedAt <= endDate);
        }

        public IQueryable<object> GetContactRequestsCountByMonth(int acceptedContactRequestStatus)
        {
            
            return _db.InvestorContactRequests
                      .Where(icr => icr.Status == acceptedContactRequestStatus && icr.CreatedAt.HasValue)
                      .GroupBy(icr => new { Year = icr.CreatedAt.Value.Year, Month = icr.CreatedAt.Value.Month })
                      .Select(g => new
                      {
                          Year = g.Key.Year,
                          Month = g.Key.Month,
                          Count = g.Count()
                      })
                      .OrderBy(x => x.Year)
                      .ThenBy(x => x.Month);
        }
        public IQueryable<object> GetIdeasCountByCategory(int activeBusinessStatus)
        {
            return _db.Businesses
                      .Where(b => b.Status == activeBusinessStatus && !b.IsDrafted)
                      .GroupBy(b => b.Category) 
                      .Select(g => new
                      {
                          CategoryId = g.Key.Id,
                          CategoryName = g.Key.Name,
                          IdeasCount = g.Count()
                      })
                      .OrderBy(x => x.CategoryName);
        }
        public IQueryable<object> GetMostActiveInvestors(int topN)
        {
            return _db.InvestorContactRequests
                      .Include(icr => icr.Investor)
                          .ThenInclude(i => i.User) 
                      .GroupBy(icr => icr.InvestorId)
                      .Select(g => new
                      {
                          InvestorId = g.Key,
                          InvestorName = g.First().Investor.User != null ? $"{g.First().Investor.User.FirstName} {g.First().Investor.User.LastName}" : "Unknown Investor",
                          RequestCount = g.Count()
                      })
                      .OrderByDescending(x => x.RequestCount)
                      .Take(topN);
        }

        public IQueryable<object> GetMostSupportedFounders(int acceptedContactRequestStatus, int topN)
        {
            return _db.InvestorContactRequests
                      .Where(icr => icr.Status == acceptedContactRequestStatus) 
                      .Include(icr => icr.Business)
                          .ThenInclude(b => b.Founder)
                              .ThenInclude(f => f.User) 
                      .GroupBy(icr => icr.Business.FounderId)
                      .Select(g => new
                      {
                          FounderId = g.Key,
                          FounderName = g.First().Business.Founder.User != null ? $"{g.First().Business.Founder.User.FirstName} {g.First().Business.Founder.User.LastName}" : "Unknown Founder",
                          SupportedIdeasCount = g.Count() 
                      })
                      .OrderByDescending(x => x.SupportedIdeasCount)
                      .Take(topN);
        }

        public IQueryable<object> GetUserCountsByGovernment(int activeUserStatus)
        {
            return _db.Users
                      .Where(u => u.Status == activeUserStatus && u.GovernmentId.HasValue) 
                      .Include(u => u.Government) 
                      .GroupBy(u => u.Government)
                      .Select(g => new
                      {
                          GovernmentId = g.Key.Id,
                          GovernmentName = g.Key.NameEn, 
                          UserCount = g.Count()
                      })
                      .OrderByDescending(x => x.UserCount);
        }

        public IQueryable<object> GetUserCountsByCity(int activeUserStatus)
        {
            return _db.Users
                      .Where(u => u.Status == activeUserStatus && u.CityId.HasValue) 
                      .Include(u => u.City) 
                      .GroupBy(u => u.City) 
                      .Select(g => new
                      {
                          CityId = g.Key.Id,
                          CityName = g.Key.NameEn,
                          UserCount = g.Count()
                      })
                      .OrderByDescending(x => x.UserCount);
        }

        public IQueryable<object> GetBusinessIdeasCountByStage(int activeBusinessStatus)
        {
            return _db.Businesses
                      .Where(b => b.Status == activeBusinessStatus && !b.IsDrafted && b.Stage.HasValue)
                      .GroupBy(b => b.Stage)
                      .Select(g => new
                      {
                          Stage = g.Key,
                          IdeasCount = g.Count()
                      })
                      .OrderBy(x => x.Stage);
        }

        public IQueryable<object> GetAvgAiRatePerCategory(int activeBusinessStatus)
        {
            return _db.Businesses
                       .Where(b => b.Status == activeBusinessStatus && !b.IsDrafted && b.Airate.HasValue)
                       .Include(b => b.Category) 
                       .GroupBy(b => b.Category)
                       .Select(g => new
                       {
                           CategoryId = g.Key.Id,
                           CategoryName = g.Key.Name,
                           AverageAiRate = g.Average(b => b.Airate)
                       })
                       .OrderByDescending(x => x.AverageAiRate);
        }

        public IQueryable<object> GetIdeasCountByInvestmentType(int activeBusinessStatus)
        {
            return _db.Businesses
                      .Where(b => b.Status == activeBusinessStatus && !b.IsDrafted && b.DesiredInvestmentType.HasValue)
                      .GroupBy(b => b.DesiredInvestmentType)
                      .Select(g => new
                      {
                          InvestmentType = g.Key,
                          IdeasCount = g.Count()
                      })
                      .OrderBy(x => x.InvestmentType);
        }
    }
}
