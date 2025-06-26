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

        #region Founder

        public int getFounderNotificationUnreadCount(int loggedInUserId);

        #endregion

        public Task NotifyUser(string UserId);
    }
}
