using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class City
{
    public int Id { get; set; }

    public int GovId { get; set; }

    public string NameAr { get; set; } = null!;

    public string NameEn { get; set; } = null!;

    public virtual Government Gov { get; set; } = null!;

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
