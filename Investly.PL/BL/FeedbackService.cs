using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;
using System;
using System.Linq;
using Investly.PL.General;
using System.ComponentModel.DataAnnotations;
using System.Threading.Tasks;

namespace Investly.PL.BL
{
    public class FeedbackService : IFeedbackService
    {
        private readonly IUnitOfWork _unitOfWork;
        private readonly IMapper _mapper;

        public FeedbackService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
        }

        public FeedbackListDto GetAllFeedbacks(FeedbackSearchDto searchDto)
        {
            try
            {
                IQueryable<Feedback> feedbacksQuery = _unitOfWork.FeedbackRepo.GetAll(
                    filter: null,
                    includeProperties: "UserIdToNavigation,CreatedByNavigation"
                ).AsQueryable();

                if (searchDto.StatusFilter.HasValue && searchDto.StatusFilter.Value > 0)
                {
                    feedbacksQuery = feedbacksQuery.Where(f => f.Status == searchDto.StatusFilter.Value);
                }
                else
                {
                    feedbacksQuery = feedbacksQuery.Where(f => f.Status != (int)UserStatus.Deleted);
                }

                if (!string.IsNullOrEmpty(searchDto.SearchInput))
                {
                    string searchLower = searchDto.SearchInput.ToLower();
                    feedbacksQuery = feedbacksQuery.Where(f =>
                        (f.Description != null && f.Description.ToLower().Contains(searchLower)) ||
                        (f.UserIdToNavigation != null && (
                            f.UserIdToNavigation.FirstName.ToLower().Contains(searchLower) ||
                            f.UserIdToNavigation.LastName.ToLower().Contains(searchLower) ||
                            (f.UserIdToNavigation.FirstName + " " + f.UserIdToNavigation.LastName).ToLower().Contains(searchLower)
                        )) ||
                        (f.CreatedByNavigation != null && (
                            f.CreatedByNavigation.FirstName.ToLower().Contains(searchLower) ||
                            f.CreatedByNavigation.LastName.ToLower().Contains(searchLower) ||
                            (f.CreatedByNavigation.FirstName + " " + f.CreatedByNavigation.LastName).ToLower().Contains(searchLower)
                        ))
                    );
                }

                if (searchDto.UserTypeFromFilter.HasValue && searchDto.UserTypeFromFilter.Value > 0)
                {
                    feedbacksQuery = feedbacksQuery.Where(f =>
                        f.CreatedByNavigation != null && f.CreatedByNavigation.UserType == searchDto.UserTypeFromFilter.Value);
                }

                if (searchDto.UserTypeToFilter.HasValue && searchDto.UserTypeToFilter.Value > 0)
                {
                    var targetType = (FeedbackTargetType)searchDto.UserTypeToFilter.Value;

                    if (targetType == FeedbackTargetType.System)
                    {
                        feedbacksQuery = feedbacksQuery.Where(f => f.FeedbackType == (int)FeedbackTargetType.System);
                    }
                    else if (targetType == FeedbackTargetType.Investor || targetType == FeedbackTargetType.Founder)
                    {
                        feedbacksQuery = feedbacksQuery.Where(f =>
                            f.UserIdToNavigation != null &&
                            f.UserIdToNavigation.UserType == (int)targetType &&
                            f.FeedbackType == (int)targetType);
                    }
                }
                int totalCount = feedbacksQuery.Count();

                int skip = (searchDto.PageNumber - 1) * searchDto.PageSize;
                var paginatedFeedbacks = feedbacksQuery
                                                     .OrderByDescending(f => f.CreatedAt)
                                                     .Skip(skip)
                                                     .Take(searchDto.PageSize)
                                                     .ToList();

                var feedbackDtos = _mapper.Map<List<FeedbackDto>>(paginatedFeedbacks);

                return new FeedbackListDto
                {
                    List = feedbackDtos,
                    TotalCount = totalCount
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetAllFeedbacks: {ex.Message}");
                return new FeedbackListDto
                {
                    List = new List<FeedbackDto>(),
                    TotalCount = 0
                };
            }
        }

        public int DeleteFeedback(int feedbackId, int? loggedUserId, int actionType)
        {
            try
            {
                var feedback = _unitOfWork.FeedbackRepo.GetById(feedbackId);
                if (feedback == null)
                {
                    return -1; 
                }

                UserStatus newStatus = (UserStatus)actionType;

                if (newStatus != UserStatus.Inactive && newStatus != UserStatus.Deleted && newStatus != UserStatus.Active)
                {
                    return -2; 
                }

                feedback.Status = actionType;
                feedback.UpdatedAt = DateTime.UtcNow;
                feedback.UpdatedBy = loggedUserId;

                _unitOfWork.FeedbackRepo.Update(feedback);
                return _unitOfWork.Save(); 
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in SoftDeleteFeedback: {ex.Message}");
                return -3; 
            }
        }

        public FeedbackCountsDto GetFeedbackStatisticsCounts()
        {
            try
            {
                var counts = _unitOfWork.FeedbackRepo.GetFeedbackCountsByStatus(
                    (int)UserStatus.Active,
                    (int)UserStatus.Inactive
                );

                return new FeedbackCountsDto
                {
                    TotalActive = counts.Item1,
                    TotalInactive = counts.Item2
                };
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in GetFeedbackStatisticsCounts: {ex.Message}");
                return new FeedbackCountsDto();
            }

         }
        public async Task CreateFeedbackAsync(FeedbackCreateDto dto, int? currentUserId)
        {
            
            if (string.IsNullOrWhiteSpace(dto.Email))
                throw new ValidationException("Email is required.");

            // Validation based on Feedback Type
            switch (dto.FeedbackType)
            {
                case FeedbackTargetType.System:
                    if (dto.UserIdTo != null)
                        throw new ValidationException("UserIdTo must be null for system feedback.");
                    break;

                case FeedbackTargetType.Founder:
                case FeedbackTargetType.Investor:
                    if (dto.UserIdTo == null)
                        throw new ValidationException("UserIdTo is required when targeting a Founder or Investor.");
                    break;

                default:
                    throw new ValidationException("Invalid feedback type.");
            }

            // Authenticated User
            if (currentUserId != null)
            {
                dto.CreatedBy = currentUserId;
            }

            bool isAnonymous = currentUserId == null;

            if (isAnonymous && dto.FeedbackType != FeedbackTargetType.System)
            {
                throw new ValidationException("Anonymous users can only submit feedback to the system.");
            }


            // Mapping to Entity
            var feedback = new Feedback
            {
                Email = dto.Email,
                Subject = dto.Subject,
                Description = dto.Description,
                FeedbackType = (int)dto.FeedbackType,
                UserIdTo = dto.UserIdTo,
                CreatedBy = dto.CreatedBy,
                Status = dto.Status
            };

            await _unitOfWork.FeedbackRepo.InsertAsync(feedback);
            await _unitOfWork.SaveAsync();
        }

    }
}