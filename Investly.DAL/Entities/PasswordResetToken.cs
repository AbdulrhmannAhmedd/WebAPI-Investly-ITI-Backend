using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Investly.DAL.Entities;

public partial class PasswordResetToken
{
    public int Id { get; set; }
    public string Token { get; set; }
    public DateTime Expiration { get; set; }
    public bool IsUsed { get; set; } = false;
    public int UserId { get; set; }
    public User User { get; set; }
}

