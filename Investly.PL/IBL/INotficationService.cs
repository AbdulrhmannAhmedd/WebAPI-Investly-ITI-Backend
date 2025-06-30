using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface INotficationService
    {
        #region Admin
        public PaginatedNotificationsDto GetallPaginatedNotifications(NotificationSearchDto search);
        public Task<int> SendNotification(NotificationDto notification, int? LoggedInUser, int LoggedInUserType);
        public int ChnageStatus(int NotificationId,int Status, int? LoggedInUser);
        public NotifcationsTotalActiveDeletedDto GetTotalNotificationsActiveDeleted();
        #endregion
        public int getUserNotificationUnreadCount(int loggedInUserId);
        public PaginatedNotificationsDto GetUserNotifications(NotificationSearchDto search, int userId);
        public Task<int> MarkAllUserNotificationsAsRead(int userId);
        public Task NotifyUser(string UserId);
    }
}
