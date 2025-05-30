using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Founder
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual User User { get; set; } = null!;
}
