using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class CategoryStandard
{
    public int Id { get; set; }

    public int StandardId { get; set; }

    public int CategoryId { get; set; }

    public int Weight { get; set; }

    public int? Starus { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Category Category { get; set; } = null!;

    public virtual ICollection<CategoryStandardsKeyWord> CategoryStandardsKeyWords { get; set; } = new List<CategoryStandardsKeyWord>();

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Standard Standard { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
