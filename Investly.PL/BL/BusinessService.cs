using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;
using Investly.PL.General;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;
using Microsoft.AspNetCore.Mvc;
using Investly.PL.General.Services.IServices;

namespace Investly.PL.BL
{
    public class BusinessService : IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHelper _helper;

        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper, IHelper helper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _helper = helper;
        }

        public BusinessListDto GetAllBusinesses(BusinessSearchDto searchDto)
        {
            try
            {
                IQueryable<Business> businessesQuery = _unitOfWork.BusinessRepo.GetAll(
                    filter: null,
                    includeProperties: "Category,Founder.User,BusinessStandardAnswers.Standard,City,Government"
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
                newIdea.Airate=BusinessIdea.AiBusinessEvaluations?.TotalWeightedScore ;
                newIdea.GeneralAiFeedback=BusinessIdea.AiBusinessEvaluations?.GeneralFeedback;
                newIdea.AiBusinessStandardsEvaluations=_mapper.Map<List<AiBusinessStandardsEvaluation>>(BusinessIdea.AiBusinessEvaluations?.Standards);
                newIdea.CreatedBy = LoggedInUser;
                newIdea.CreatedAt = DateTime.UtcNow;
                newIdea.Category = null;

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
                if (newIdea.AiBusinessStandardsEvaluations != null)

                {
                    foreach (var standard in newIdea.AiBusinessStandardsEvaluations)
                    {
                        standard.CreatedBy = LoggedInUser;
                        standard.CreatedAt = DateTime.Now;
                        standard.CategoryStandard = null;
                        
                    }
                }

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

        public int AddBusinessIdeaAiEvaluation(AiBusinessEvaluationDto dto, int LoggedInUser)
        {
            try
            {
                List<AiBusinessStandardsEvaluation> aiBusinessStandardsEvaluations = new List<AiBusinessStandardsEvaluation>();
                var business = _unitOfWork.BusinessRepo.GetById(dto.BusinessId??0);
                if (business == null)
                {
                    return -3; // Business not found
                }

                foreach (var answer in dto.Standards)
                {
                    var standardAnswer = new AiBusinessStandardsEvaluation
                    {
                        BusinessId = dto.BusinessId??0,
                        CategoryStandardId = answer.StandardCategoryId,
                        AchievementScore = answer.AchievementScore,
                        CreatedBy = LoggedInUser,
                        CreatedAt = DateTime.UtcNow,
                        Weight = answer.Weight,
                        WeightedContribution = answer.WeightedContribution
                    };
                    aiBusinessStandardsEvaluations.Add(standardAnswer);
                }
                _unitOfWork.AiBusinessEvaluationRepo.AddRange(aiBusinessStandardsEvaluations);
                int res= _unitOfWork.Save();
                if (res > 0)
                {
                    business.Airate = dto.TotalWeightedScore;
                    business.GeneralAiFeedback=dto.GeneralFeedback;
                    business.UpdatedAt = DateTime.UtcNow;
                    business.UpdatedBy = LoggedInUser;
                    _unitOfWork.BusinessRepo.Update(business);
                   return _unitOfWork.Save();
                 
                }
                else
                {
                    return 0; // No changes saved
                }

            }
            catch (Exception ex)
            {

                return -1;
            }
        }

        public int UpdateBusinessIdea(BusinessDto BusinessIdea, int? LoggedInUser)
        {
            try
            {
                if (BusinessIdea == null || BusinessIdea.Id <= 0)
                {
                    return -2; // Invalid input
                }
                var existingBusiness = _unitOfWork.BusinessRepo.GetById(BusinessIdea.Id);
               
                if (existingBusiness == null)
                {
                    return -3; // Business not found
                }

                //update files 
                this.HandleBusinessIdeaFiles(BusinessIdea, existingBusiness.FilePath, existingBusiness.Images);


                //remove previous data
                _unitOfWork.BusinessRepo.RemoveRangeStandardAnswers(BusinessIdea.Id);
                _unitOfWork.BusinessRepo.RemoveRangAiStandardEvaluations(BusinessIdea.Id);

                int? createdBy = existingBusiness.CreatedBy;
                DateTime? createdAt = existingBusiness.CreatedAt;

                existingBusiness = _mapper.Map(BusinessIdea,existingBusiness);
                existingBusiness.Category = null;
                existingBusiness.Airate = BusinessIdea.AiBusinessEvaluations.TotalWeightedScore;
                existingBusiness.GeneralAiFeedback = BusinessIdea.AiBusinessEvaluations.GeneralFeedback;
                existingBusiness.AiBusinessStandardsEvaluations = _mapper.Map<List<AiBusinessStandardsEvaluation>>(BusinessIdea.AiBusinessEvaluations.Standards);
                existingBusiness.Status = (int)BusinessIdeaStatus.Pending;
                existingBusiness.UpdatedBy = LoggedInUser;
                existingBusiness.UpdatedAt = DateTime.UtcNow;
                existingBusiness.CreatedBy = createdBy;
                existingBusiness.CreatedAt = createdAt;
                _unitOfWork.BusinessRepo.Update(existingBusiness);
                var result = _unitOfWork.Save();

                return result; // Indicate success
            }

            catch (Exception ex)
            {
                Console.WriteLine($"Error in UpdateBusinessIdea: {ex.Message}");
                return -1; // Indicate an error occurred
            }

        }
        public List<BusinessDto> GetFounderBusinessIdeas(int LoggedInUserIdFounder)
        {
            try
            {
                var founder = _unitOfWork.FounderRepo.FirstOrDefault(u=>u.UserId==LoggedInUserIdFounder);
                if (founder == null)
                {
                    return new List<BusinessDto>(); // Return an empty list if FounderId is invalid
                }

                var businessIdeas = _unitOfWork.BusinessRepo.GetAll(
                    b => b.Founder.UserId == LoggedInUserIdFounder && b.Status != (int)BusinessIdeaStatus.Deleted,
                    "Founder,Category,InvestorContactRequests,AiBusinessStandardsEvaluations.CategoryStandard.Standard,Government,City,BusinessStandardAnswers"
                ).ToList();

                if (businessIdeas == null || !businessIdeas.Any())
                {
                    return new List<BusinessDto>(); // Return an empty list if no business ideas are found
                }

                return _mapper.Map<List<BusinessDto>>(businessIdeas);
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFounderBusinessIdeas: {ex.Message}");
                return new List<BusinessDto>(); // Return an empty list in case of an error
            }
        }





        private void HandleBusinessIdeaFiles(BusinessDto businessDto, string? existingFilePath = null, string? existingImages = null)
        {
            // Handle Idea File
            if (businessDto.IdeaFile != null)
            {
                if (!string.IsNullOrEmpty(existingFilePath))
                {
                    _helper.DeleteFile(existingFilePath);
                }

                var filePath = _helper.UploadFile(businessDto.IdeaFile, "founder", "IdeaFile");
                businessDto.FilePath = filePath;
            }

            // Handle Image Files
            if (businessDto.ImageFiles?.Any() == true)
            {
                if (!string.IsNullOrEmpty(existingImages))
                {
                    var oldImagePaths = existingImages.Split(';');
                    foreach (var oldImagePath in oldImagePaths)
                    {
                        _helper.DeleteFile(oldImagePath);
                    }
                }

                var imagePaths = new List<string>();
                foreach (var imageFile in businessDto.ImageFiles)
                {
                    var imagePath = _helper.UploadFile(imageFile, "founder", "BusinessImages");
                    if (!string.IsNullOrEmpty(imagePath))
                    {
                        imagePaths.Add(imagePath);
                    }
                }

                businessDto.Images = string.Join(";", imagePaths);
            }
        }
    }
}