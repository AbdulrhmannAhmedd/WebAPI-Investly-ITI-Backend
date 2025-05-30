using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class CategoryStandardsKeyWord
{
    public int Id { get; set; }

    public int CategoryStandardId { get; set; }

    public string KeyWord { get; set; } = null!;

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual CategoryStandard CategoryStandard { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }
}
