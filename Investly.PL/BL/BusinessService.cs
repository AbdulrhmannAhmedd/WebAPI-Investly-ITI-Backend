using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;
using Investly.PL.General;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Investly.PL.BL
{
    public class BusinessService : IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public BusinessListDto GetAllBusinesses(BusinessSearchDto searchDto)
        {
            try
            {
                IQueryable<Business> businessesQuery = _unitOfWork.BusinessRepo.GetAll(
                    filter: null,
                    includeProperties: "Category,Founder.User"
                ).AsQueryable();

                businessesQuery = businessesQuery.Where(b =>
                    b.IsDrafted == false &&
                    b.Status != (int)BusinessIdeaStatus.Deleted 
                );

                if (!string.IsNullOrEmpty(searchDto.SearchInput))
                {
                    string searchLower = searchDto.SearchInput.ToLower();
                    businessesQuery = businessesQuery.Where(b =>
                        b.Title.ToLower().Contains(searchLower) ||
                        (b.Location != null && b.Location.ToLower().Contains(searchLower)) ||
                        (b.Founder != null && b.Founder.User != null && (
                            (b.Founder.User.FirstName + " " + b.Founder.User.LastName).ToLower().Contains(searchLower) ||
                            b.Founder.User.FirstName.ToLower().Contains(searchLower) ||
                            b.Founder.User.LastName.ToLower().Contains(searchLower)
                        )) ||
                        (b.Category != null && b.Category.Name != null && b.Category.Name.ToLower().Contains(searchLower))
                    );
                }

                if (searchDto.CategoryId.HasValue && searchDto.CategoryId.Value > 0)
                {
                    businessesQuery = businessesQuery.Where(b => b.CategoryId == searchDto.CategoryId.Value);
                }
             

                if (searchDto.FounderId.HasValue && searchDto.FounderId.Value > 0)
                {
                    businessesQuery = businessesQuery.Where(b => b.FounderId == searchDto.FounderId.Value);
                }

                if (searchDto.Stage.HasValue && searchDto.Stage.Value > 0)
                {
                    businessesQuery = businessesQuery.Where(b => b.Stage == searchDto.Stage.Value);
                }

                int totalCount = businessesQuery.Count();

                int skip = (searchDto.PageNumber - 1) * searchDto.PageSize;
                var paginatedBusinesses = businessesQuery
                                            .OrderByDescending(b => b.CreatedAt)
                                            .Skip(skip)
                                            .Take(searchDto.PageSize)
                                            .ToList();

                var businessDtos = _mapper.Map<List<BusinessDto>>(paginatedBusinesses);

                return new BusinessListDto
                {
                    Businesses = businessDtos,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in BusinessService.GetAllBusinesses: {ex.Message}");
                return new BusinessListDto
                {
                    Businesses = new List<BusinessDto>(),
                    TotalCount = 0
                };
            }
        }
        public int SoftDeleteBusiness(int businessId, int? loggedUserId)
        {
            return UpdateBusinessStatus(businessId, BusinessIdeaStatus.Deleted, loggedUserId);
        }

        public int UpdateBusinessStatus(int businessId, BusinessIdeaStatus newStatus, int? loggedUserId, string? rejectedReason = null)
        {
            try
            {
                var business = _unitOfWork.BusinessRepo.GetById(businessId);
                if (business == null)
                {
                    return -1;
                }

                if (newStatus == BusinessIdeaStatus.Rejected)
                {
                    if (string.IsNullOrWhiteSpace(rejectedReason)) 
                    {
                        return -4;
                    }
                    business.RejectedReason = rejectedReason.Trim();
                }
                else
                {
                    business.RejectedReason = null;
                }

                business.Status = (int)newStatus;
                business.UpdatedAt = DateTime.UtcNow;
                business.UpdatedBy = loggedUserId;

                _unitOfWork.BusinessRepo.Update(business);
                return _unitOfWork.Save();
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBusinessStatus: {ex.Message}");
                return -3;
            }
        }
        public BusinessCountsDto GetBusinessIdeasCounts()
        {
            try
            {
                var counts = _unitOfWork.BusinessRepo.GetBusinessCountsByStatus(
                    (int)BusinessIdeaStatus.Active,
                    (int)BusinessIdeaStatus.Inactive,
                    (int)BusinessIdeaStatus.Rejected,
                    (int)BusinessIdeaStatus.Pending,
                    (int)UserStatus.Deleted 
                );

                return new BusinessCountsDto
                {
                    TotalActive = counts.Item1,
                    TotalInactive = counts.Item2,
                    TotalRejected = counts.Item3,
                    TotalPending = counts.Item4
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetBusinessStatisticsCounts: {ex.Message}");
                return new BusinessCountsDto();
            }
        }

        public int AddBusinessIdea(BusinessDto BusinessIdea, int? LoggedInUser)
        {
           try
            {
                if (BusinessIdea == null)
                {
                    return -2;
                }
                var newIdea=_mapper.Map<Business>(BusinessIdea);
               
                newIdea.CreatedBy = LoggedInUser;
               

                if (LoggedInUser != null)
                {
                    var founder = _unitOfWork.FounderRepo.FirstOrDefault(f=>f.UserId==LoggedInUser.Value);
                    if (founder != null)
                    {
                      
                        newIdea.FounderId = founder.Id;
                    }
                }
               if(newIdea.BusinessStandardAnswers != null)

                {
                    foreach(var answer in newIdea.BusinessStandardAnswers)
                    {
                        answer.CreatedBy = LoggedInUser;
                        answer.CreatedAt = DateTime.Now;
                    }
                }
                newIdea.CreatedAt = DateTime.Now;
                newIdea.Category = null;
                newIdea.Status = (int)BusinessIdeaStatus.Pending;
                _unitOfWork.BusinessRepo.Insert(newIdea);
                var res = _unitOfWork.Save();
                if (res>0)
                {
                    return 1;
                }else
                {
                    return 0;
                }

            }
            catch (Exception ex)
            {
              
                return -1;
            }
        }
    }
}