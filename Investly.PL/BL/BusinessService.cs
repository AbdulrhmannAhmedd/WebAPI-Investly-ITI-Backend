using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Mvc;
using Microsoft.EntityFrameworkCore;
using System.Diagnostics.Metrics;
using System.Linq;
using static Microsoft.EntityFrameworkCore.DbLoggerCategory;

namespace Investly.PL.BL
{
    public class BusinessService : IBusinessService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;
        private readonly IHelper _helper;
        private readonly INotficationService _notificationService;

        public BusinessService(IUnitOfWork unitOfWork, IMapper mapper, IHelper helper,INotficationService notificationService)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _helper = helper;
            _notificationService = notificationService;
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

        public async Task<BusinessListDtoForExplore> GetAllBusinessesForExploreAsync(BusinessSeachForExploreDto searchDto , int? loggedInUser)
        {
            try
            {
                var SearchInputToLower = searchDto.SearchInput?.ToLower() ?? string.Empty;

                // To check if the user logged in and he is an investor
                int? investorId = null;
                bool isLoggedInInvestor = false;
                Investor? investorDetails = null;

                if (loggedInUser.HasValue)
                {
                    investorDetails = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.UserId == loggedInUser.Value);
                    if (investorDetails != null)
                    {
                        investorId = investorDetails.Id;
                        isLoggedInInvestor = true;
                    }
                }

                var savedSearchDto = ApplyInvestorPreferences(searchDto, investorDetails);

                var businessQuery = _unitOfWork.BusinessRepo.FindAll(
                    item => (
                        (
                            String.IsNullOrEmpty(SearchInputToLower) ||
                            (item.Title.ToLower().Contains(SearchInputToLower)) ||
                            (item.Founder.User.FirstName.ToLower().Contains(SearchInputToLower)) ||
                            (item.Founder.User.LastName.ToLower().Contains(SearchInputToLower)) ||
                            ((item.Founder.User.FirstName + " " + item.Founder.User.LastName).ToLower().Contains(SearchInputToLower))
                        ) &&
                        (!savedSearchDto.CategoryId.HasValue || savedSearchDto.CategoryId.Value == 0 || item.CategoryId == savedSearchDto.CategoryId.Value) &&
                        (!savedSearchDto.GovernmentId.HasValue || savedSearchDto.GovernmentId.Value == 0 || item.GovernmentId == savedSearchDto.GovernmentId.Value) &&
                        (!savedSearchDto.MinCapital.HasValue || item.Capital >= savedSearchDto.MinCapital.Value) &&
                        (!savedSearchDto.MaxCapital.HasValue || item.Capital <= savedSearchDto.MaxCapital.Value) &&
                        (!savedSearchDto.MinAiRate.HasValue || item.Airate >= savedSearchDto.MinAiRate.Value) &&
                        (!savedSearchDto.DesiredInvestmentType.HasValue || savedSearchDto.DesiredInvestmentType.Value == 0 || item.DesiredInvestmentType == savedSearchDto.DesiredInvestmentType.Value) &&
                        (item.Status == (int)BusinessIdeaStatus.Active)
                    )
                    , properties: "Founder.User,Category,Government,InvestorContactRequests"
                ).OrderByDescending(b => b.CreatedAt);

                var businesses = await businessQuery.ToListAsync(); // I added this await here for getting the data first then apply stage filter in memory..

                var filteredBusinesses = businesses.Where(item => ApplyStageFilter(savedSearchDto, item, investorDetails)).ToList();

                int skip = (searchDto.PageSize * (searchDto.PageNumber > 0 ? searchDto.PageNumber -1 : 1));

                var paginatedBusinesses = filteredBusinesses
                                            .Skip(skip)
                                            .Take(searchDto.PageSize)
                                            .ToList();

                var businessDtos = _mapper.Map<List<DisplayBusinessToExploreSectionDto>>(paginatedBusinesses);

                foreach(var businessDto in businessDtos)
                {

                    if (isLoggedInInvestor && investorId.HasValue)
                    {

                        // Check if the investor's status is active
                        bool isInvestorActive = investorDetails?.User?.Status == (int)UserStatus.Active;

                        if (!isInvestorActive)
                        {
                            // If investor is not active, they cannot request contact
                            businessDto.ContactRequestStatus = null;
                            businessDto.CanRequestContact = false;
                        }
                        else
                        {

                            // Checking if the investor has already requested contact with this business

                            var business = paginatedBusinesses.FirstOrDefault(b => b.Id == businessDto.Id);
                            var existingContactRequest = business?.InvestorContactRequests.FirstOrDefault(icr => icr.InvestorId == investorId.Value && icr.Status != (int)ContactRequestStatus.Deleted);

                            if (existingContactRequest != null)
                            {
                                businessDto.ContactRequestStatus = (ContactRequestStatus)existingContactRequest.Status; // Sending the status of the existing contact request between investor & this business
                                businessDto.CanRequestContact = false; // Contact request already exists so investor cannot request again
                            }
                            else
                            {
                                businessDto.ContactRequestStatus = null;
                                businessDto.CanRequestContact = true; // Investor can request contact with this business
                            }
                        }

                    }
                    else
                    {
                        // in case of user not logged in or user type is not onvestor
                        businessDto.ContactRequestStatus = null;
                        businessDto.CanRequestContact = false;
                    }
                }

                InvestorPreferencesDto? investorPreferences = null;

                if(investorDetails != null)
                {
                    investorPreferences = new InvestorPreferencesDto
                    {
                        MinFunding = investorDetails.MinFunding,
                        MaxFunding = investorDetails.MaxFunding,
                        InvestingType = investorDetails.InvestingType,
                        InterestedBusinessStages = investorDetails.InterestedBusinessStages
                    };
                }

                return new BusinessListDtoForExplore
                {
                    Businesses = businessDtos,
                    TotalCount = filteredBusinesses.Count(),
                    InvestorPreferences = investorPreferences,
                    CurrentPage = searchDto.PageNumber,
                    PageSize = searchDto.PageSize
                };

            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public BusinessSeachForExploreDto ApplyInvestorPreferences(BusinessSeachForExploreDto searchDto, Investor? investorDetails)
        {
            if (investorDetails == null)
                return searchDto;

            var savedSearchDto = new BusinessSeachForExploreDto
            {
                PageSize = searchDto.PageSize,
                PageNumber = searchDto.PageNumber,
                SearchInput = searchDto.SearchInput,
                CategoryId = searchDto.CategoryId,
                GovernmentId = searchDto.GovernmentId,
                MinAiRate = searchDto.MinAiRate,
                UseDefaultPreferences = searchDto.UseDefaultPreferences,


                // investor preferences only if not overridden by investor's search input
                Stage = searchDto.Stage, // i will handle it in applyfilterstages i memory later

                // i will apply investor preferences if UseDefaultPreferences is true
                MinCapital = searchDto.UseDefaultPreferences ?
                    (searchDto.MinCapital ?? (investorDetails.MinFunding > 0 ? investorDetails.MinFunding : null)) :
                    searchDto.MinCapital,
                MaxCapital = searchDto.UseDefaultPreferences ?
                    (searchDto.MaxCapital ?? (investorDetails.MaxFunding > 0 ? investorDetails.MaxFunding : null)) :
                    searchDto.MaxCapital,
                DesiredInvestmentType = searchDto.UseDefaultPreferences ?
                    (searchDto.DesiredInvestmentType ?? (investorDetails.InvestingType > 0 ? investorDetails.InvestingType : null)) :
                    searchDto.DesiredInvestmentType
            };

            return savedSearchDto;
        }

        public bool ApplyStageFilter(BusinessSeachForExploreDto searchDto, Business item, Investor? investorDetails)
        {
            // If user provided specific stage filter --> (takes precedence)
            if (searchDto.Stage.HasValue && searchDto.Stage.Value > 0)
            {
                return item.Stage == searchDto.Stage.Value;
            }

            // If there's preferences --> check if business stage matches any preferred stage
            if (searchDto.UseDefaultPreferences && investorDetails != null && !string.IsNullOrEmpty(investorDetails.InterestedBusinessStages))
            {

                if (!item.Stage.HasValue)
                {
                    return false;
                }

                string businessStage = item.Stage.Value.ToString();

                return investorDetails.InterestedBusinessStages.Split(',', StringSplitOptions.RemoveEmptyEntries)
                       .Any(stage => stage.Trim() == businessStage);
            }

            // If UseDefaultPreferences is false or no stage preferences --> retrive all
            return true;
        }

        public BusinessDetailsDto GetBusinessDetails(int businessId, int? loggedInUser)
        {
                var business = _unitOfWork.BusinessRepo.FirstOrDefault(
                    b => b.Id == businessId && b.Status == (int)BusinessIdeaStatus.Active,
                    includeProperties: "Founder.User,Category,City,Government,BusinessStandardAnswers.Standard,InvestorContactRequests"
                );

                if (business == null)
                {
                    throw new KeyNotFoundException($"Business with ID {businessId} not found or not active.");
                }

                int? investorId = null;
                bool isLoggedInInvestor = false;

                // I'm checking if the user is logged in and if he is an investor
                if (loggedInUser.HasValue)
                {
                    var investor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.UserId == loggedInUser.Value);
                    if(investor !=null)
                    {
                        investorId = investor.Id;
                        isLoggedInInvestor = true;
                    }
                }

                var businessDetails = _mapper.Map<BusinessDetailsDto>(business);

                if (!isLoggedInInvestor)
                {
                    businessDetails.FilePath = null;
                    businessDetails.BusinessStandardAnswers = new List<BusinessStandardAnswerDto>();
                    businessDetails.isInvestor = false;
                }

            if (isLoggedInInvestor && investorId.HasValue)
            {

                var investor = _unitOfWork.InvestorRepo.FirstOrDefault(i => i.Id == investorId.Value, "User");
                bool isInvestorActive = investor?.User?.Status == (int)UserStatus.Active;

                if (!isInvestorActive)
                {
                    // investor is not active then he cannot request contact
                    businessDetails.ContactRequestStatus = null;
                    businessDetails.CanRequestContact = false;
                }
                else
                {

                    var existingContactRequest = business.InvestorContactRequests.FirstOrDefault(
                        icr => icr.InvestorId == investorId.Value && icr.Status != (int)ContactRequestStatus.Deleted
                    );

                    if (existingContactRequest != null)
                    {
                        businessDetails.ContactRequestStatus = (ContactRequestStatus)existingContactRequest.Status;
                        businessDetails.CanRequestContact = false;
                    }
                    else
                    {
                        businessDetails.ContactRequestStatus = null;
                        businessDetails.CanRequestContact = true;
                    }
                }
            }
            else
            {
                businessDetails.ContactRequestStatus = null;
                businessDetails.CanRequestContact = false;
            }

            return businessDetails;
        }

        public int SoftDeleteBusiness(int businessId, int? loggedUserId, string? loggedInEmail)
        {
            return UpdateBusinessStatus(businessId, BusinessIdeaStatus.Deleted, loggedUserId, loggedInEmail);
        }

        public int UpdateBusinessStatus(int businessId, BusinessIdeaStatus newStatus, int? loggedUserId,string? loggedInEmail=null, string? rejectedReason = null )
        {
            try
            {
                var business = _unitOfWork.BusinessRepo.FirstOrDefault(b=>b.Id==businessId,"Founder");
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
                var res = _unitOfWork.Save();
                if(res>0&&loggedInEmail=="SuperAdmin@gmail.com")
                {
                    NotificationDto notification = new NotificationDto
                    {
                        Title = "Idea Status.",
                        Body = $"Your Idea '{business.Title}' has been {(UserStatus)newStatus}.",
                        UserTypeTo = (int)UserType.Founder,
                        UserIdTo = business.Founder.UserId,

                    };
                    _notificationService.SendNotification(notification, loggedUserId, (int)UserType.Staff);
                }
                return res;
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
                Founder founder=null;
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
                     founder = _unitOfWork.FounderRepo.FirstOrDefault(f=>f.UserId==LoggedInUser.Value,"User");
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
                    if (BusinessIdea.AiBusinessEvaluations?.TotalWeightedScore>50&&BusinessIdea.IsDrafted==false)
                    {
                        var superAdmin = _unitOfWork.UserRepo.FirstOrDefault(u=>u.Email=="SuperAdmin@gmail.com");
                        NotificationDto notification = new NotificationDto
                        {
                            Title = "New Idea",
                            Body = $"Founder {founder.User.FirstName} {founder.User.LastName}  Wants to Add Idea '{BusinessIdea.Title}'.",
                            UserTypeTo = (int)UserType.Staff,
                            UserIdTo =superAdmin.Id ,

                        };
                        _notificationService.SendNotification(notification, LoggedInUser, (int)UserType.Founder);
                    }
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
              
                if (result>0&&BusinessIdea.AiBusinessEvaluations?.TotalWeightedScore > 50 && BusinessIdea.IsDrafted == false)
                {
                    var founder = _unitOfWork.UserRepo.GetById(LoggedInUser.Value);
                    var superAdmin= _unitOfWork.UserRepo.FirstOrDefault(u=>u.Email=="SuperAdmin@gmail.com");
                    NotificationDto notification = new NotificationDto
                    {
                        Title = "Idea Update Request",
                        Body = $"Founder {founder.FirstName} {founder.LastName}  Wants to Update Idea '{BusinessIdea.Title}'.",
                        UserTypeTo = (int)UserType.Staff,
                        UserIdTo = superAdmin.Id,

                    };
                    _notificationService.SendNotification(notification, LoggedInUser, (int)UserType.Founder);
                }
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
                    "Founder,Category,InvestorContactRequests.Investor.User,AiBusinessStandardsEvaluations.CategoryStandard.Standard,Government,City,BusinessStandardAnswers"
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