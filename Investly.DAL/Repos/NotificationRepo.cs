using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Repos
{
    public class NotificationRepo : Repo<Notification>, INotificationRepo
    {
        private readonly AppDbContext _db;
        public NotificationRepo(AppDbContext db) : base(db)
        {
            _db = db;
        }

        public int getCountUnRead(int userIdTo)
        {
            int res = 0;
            res = _db.Notifications.Count(n => n.UserIdTo == userIdTo && n.IsRead!=1 && n.Status==1);
            return res;
        }
    }
}
