using Investly.DAL.Entities;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Seeding
{
    public class DataSeeding
    {
        private readonly AppDbContext _appDbContext;
        public DataSeeding(AppDbContext appDbContext)
        {
            this._appDbContext = appDbContext;

        }

        public void SuperAdminSeed()
        {
            var admin = new User
            {
                FirstName = "Super",
                LastName = "Admin",
                Email = "SuperAdmin@gmail.com",
                HashedPassword = BCrypt.Net.BCrypt.HashPassword("123456"),
                UserType = 1,
                Status = 1,
                CreatedAt = DateTime.UtcNow,
                NationalId = "12345678912345",
            };
            if (!_appDbContext.Users.Any(u => u.Email == admin.Email))
            {
                _appDbContext.Users.Add(admin);
                _appDbContext.SaveChanges();
            }
        }

    }
}
