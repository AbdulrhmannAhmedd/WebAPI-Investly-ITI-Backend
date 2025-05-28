using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Government
{
    public int Id { get; set; }

    public string? NameAr { get; set; }

    public string? NameEn { get; set; }

    public virtual ICollection<City> Cities { get; set; } = new List<City>();

    public virtual ICollection<User> Users { get; set; } = new List<User>();
}
