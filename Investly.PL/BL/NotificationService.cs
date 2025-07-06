using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.Hubs;
using Investly.PL.IBL;
using Microsoft.AspNetCore.SignalR;

namespace Investly.PL.BL
{
    public class NotificationService : INotficationService
    {
        private readonly IMapper _mapper;
        private readonly IUnitOfWork _unitOfWork;
        private readonly IHubContext<NotificationHub> _notifcationHubContext;
        public NotificationService(IMapper mapper, IUnitOfWork unitOfWork, IHubContext<NotificationHub> hubContext)
        {
            _mapper = mapper;
            _unitOfWork = unitOfWork;
            _notifcationHubContext = hubContext;
        }


        #region Admin
        public PaginatedNotificationsDto GetallPaginatedNotifications(NotificationSearchDto search)
        {
            try
            {
                var notifications = _unitOfWork.NotificationRepo.GetAll(n =>
         (
             (
                 (search.Status == null || search.Status == 0) && n.Status != (int)NotificationsStatus.Deleted
             ) ||
             (
                 search.Status != null && search.Status != 0 && n.Status == search.Status
             )
         )
         &&
         (search.isRead == null || n.IsRead == search.isRead)
         &&
         (search.UserTypeTo == null || search.UserTypeTo == 0 || n.UserTypeTo == search.UserTypeTo)
         &&
         (search.UserTypeFrom == null || search.UserTypeFrom == 0 || n.UserTypeFrom == search.UserTypeFrom)
         &&
         (
             string.IsNullOrEmpty(search.SearchInput)
             ||
             (
                 (n.Title.Contains(search.SearchInput)) ||
                 (n.UserIdToNavigation.FirstName.Contains(search.SearchInput)) ||
                 (n.UserIdToNavigation.LastName.Contains(search.SearchInput)) ||
                 (n.UserIdToNavigation.Email.Contains(search.SearchInput)) ||
                 (n.CreatedByNavigation.FirstName.Contains(search.SearchInput)) ||
                 (n.CreatedByNavigation.LastName.Contains(search.SearchInput)) ||
                 (n.CreatedByNavigation.Email.Contains(search.SearchInput))
             )
         ),
         includeProperties: "CreatedByNavigation,UpdatedByNavigation,UserIdToNavigation"
     )
     .OrderByDescending(n => n.CreatedAt);


                var PaginatedData = notifications.Skip(((search.PageNumber > 0 ? search.PageNumber : 1) - 1) * (search.PageSize > 0 ? search.PageSize : 5))
                      .Take(search.PageSize > 0 ? search.PageSize : 5)
                      .ToList();
                var NotificationPaginated = new PaginatedNotificationsDto
                {
                    notifications = _mapper.Map<List<NotificationDto>>(PaginatedData),
                    CurrentPage = (search.PageNumber > 0) ? search.PageNumber : 1,
                    PageSize = (search.PageSize > 0) ? search.PageSize : 5,
                    TotalCount = notifications.Count(),
                };
                return NotificationPaginated;
            }
            catch (Exception ex)
            {

                return null;
            }
        }

        public async Task<int> SendNotification(NotificationDto notification, int? LoggedInUser, int LoggedInUserType)
        {
            try
            {
                int res = 0;
                if (notification == null)
                {
                    return -1;
                }
                var newnotification = _mapper.Map<Notification>(notification);
                newnotification.CreatedBy = LoggedInUser;
                newnotification.UserTypeFrom = LoggedInUserType;
                newnotification.UserTypeTo = notification.UserTypeTo;
                newnotification.UserIdTo = notification.UserIdTo;
                newnotification.CreatedAt = DateTime.Now;
                newnotification.Body = notification.Body;
                newnotification.Title = notification.Title;
                newnotification.IsRead = 0;
                newnotification.Status = (int)NotificationsStatus.Active;
                _unitOfWork.NotificationRepo.Insert(newnotification);
                res = _unitOfWork.Save();
                if (res > 0)
                {
                    await this.NotifyUser(notification.UserIdTo.ToString());
                }
                return res;

            }
            catch (Exception ex)
            {

                return -2;
            }
        }
        public int ChnageStatus(int NotificationId, int Status, int? LoggedInUser)
        {
            try
            {
                if (NotificationId <= 0 || Status <= 0)
                {
                    return -1;
                }
                var notification = _unitOfWork.NotificationRepo.GetById(NotificationId);
                if (notification == null)
                {
                    return -2;
                }
                if (LoggedInUser != notification.CreatedBy && LoggedInUser != notification.UserIdTo)
                {
                    return -3;
                }
                notification.Status = Status;
                notification.UpdatedBy = LoggedInUser;
                notification.UpdatedAt = DateTime.Now;
                _unitOfWork.NotificationRepo.Update(notification);
                var res = _unitOfWork.Save();

                return res;
            }
            catch (Exception ex)
            {
                return -4;
            }
        }

        public NotifcationsTotalActiveDeletedDto GetTotalNotificationsActiveDeleted()
        {
            try
            {
                var notifications = _unitOfWork.NotificationRepo.GetAll().ToList();
                int ActiveNotifications = notifications.Count(n => n.Status == (int)NotificationsStatus.Active);
                int DeletedNotifications = notifications.Count(n => n.Status == (int)NotificationsStatus.Deleted);
                NotifcationsTotalActiveDeletedDto res = new NotifcationsTotalActiveDeletedDto
                { TotalActive = ActiveNotifications, TotalDeleted = DeletedNotifications };
                return res;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        #endregion

        public int getUserNotificationUnreadCount(int loggedInUserId)
        {
            try
            {
                var count = _unitOfWork.NotificationRepo.getCountUnRead(loggedInUserId);
                return count;

            }
            catch (Exception ex)
            {
                return -1;

            }
        }
        public async Task NotifyUser(string UserId)
        {
            int count = _unitOfWork.NotificationRepo.getCountUnRead(int.Parse(UserId));
            // int count = 7;
            await _notifcationHubContext.Clients.User(UserId).SendAsync("RecieveNotificationCount", count);

        }
        public PaginatedNotificationsDto GetUserNotifications(NotificationSearchDto search, int userId)
        {
            try
            {
                var notifications = _unitOfWork.NotificationRepo.GetAll(n =>
                    n.UserIdTo == userId &&
                    (
                        ((search.Status == null || search.Status == 0) && n.Status != (int)NotificationsStatus.Deleted) ||
                        (search.Status != null && search.Status != 0 && n.Status == search.Status)
                    )
                    &&
                    (search.isRead == null || n.IsRead == search.isRead)
                    &&
                    (
                        string.IsNullOrEmpty(search.SearchInput)
                        ||
                        (
                            (n.Title != null && n.Title.Contains(search.SearchInput)) ||
                            (n.Body != null && n.Body.Contains(search.SearchInput))
                        )
                    ),
                    includeProperties: "CreatedByNavigation,UpdatedByNavigation"
                )
                .OrderByDescending(n => n.CreatedAt);

                var paginatedData = notifications
                    .Skip(((search.PageNumber > 0 ? search.PageNumber : 1) - 1) * (search.PageSize > 0 ? search.PageSize : 5))
                    .Take(search.PageSize > 0 ? search.PageSize : 5)
                    .ToList();

                var notificationPaginated = new PaginatedNotificationsDto
                {
                    notifications = _mapper.Map<List<NotificationDto>>(paginatedData),
                    CurrentPage = (search.PageNumber > 0) ? search.PageNumber : 1,
                    PageSize = (search.PageSize > 0) ? search.PageSize : 5,
                    TotalCount = notifications.Count(),
                };
                return notificationPaginated;
            }
            catch (Exception ex)
            {
                return null;
            }
        }
        public async Task<int> MarkAllUserNotificationsAsRead(int userId)
        {
            try
            {
                var unreadNotifications = _unitOfWork.NotificationRepo
                    .GetAll(n => n.UserIdTo == userId && n.IsRead == 0 && n.Status == (int)NotificationsStatus.Active)
                    .ToList();

                if (!unreadNotifications.Any())
                {
                    return 0;
                }

                foreach (var notification in unreadNotifications)
                {
                    notification.IsRead = 1;
                    _unitOfWork.NotificationRepo.Update(notification);
                }

                var res = _unitOfWork.Save();

                if (res > 0)
                {
                    await NotifyUser(userId.ToString());
                }

                return res;
            }
            catch (Exception ex)
            {
                Console.WriteLine($"Error in MarkAllUserNotificationsAsRead: {ex.Message}");
                return -1;
            }
        }
        public List<NotificationDto> GetUnreadNotifications(int? loggedinuser)
        {
            try
            {
                var notifcations = _unitOfWork.NotificationRepo.GetAll(n => n.UserIdTo == loggedinuser && n.IsRead == 0).OrderByDescending(n=>n.CreatedAt).Take(3);
                var res = _mapper.Map<List<NotificationDto>>(notifcations);
              

                return res;
            }
            catch (Exception ex)
            {
                {
                    return null;
                }
            }
        }
    }
}
