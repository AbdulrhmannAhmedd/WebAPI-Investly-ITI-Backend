using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Category
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Business> Businesses { get; set; } = new List<Business>();

    public virtual ICollection<CategoryStandard> CategoryStandards { get; set; } = new List<CategoryStandard>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
