using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class BusinessStandardAnswer
{
    public int Id { get; set; }

    public int BusinessId { get; set; }

    public int StandardId { get; set; }

    public string Answer { get; set; } = null!;

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual Business Business { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Standard Standard { get; set; } = null!;

    public virtual User? UpdatedByNavigation { get; set; }
}
