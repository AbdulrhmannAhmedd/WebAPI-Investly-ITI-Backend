using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Notification
{
    public int Id { get; set; }

    public string? Title { get; set; }

    public string? Body { get; set; }

    public int UserTypeFrom { get; set; }

    public int UserTypeTo { get; set; }

    public int UserIdTo { get; set; }

    public int IsRead { get; set; }

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User UserIdToNavigation { get; set; } = null!;
}
