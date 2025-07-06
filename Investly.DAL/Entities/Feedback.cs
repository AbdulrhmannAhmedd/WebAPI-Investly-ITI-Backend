using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Feedback
{
    public int Id { get; set; }
    public string Email { get; set; }
    public string Subject { get; set; }

    public string? Description { get; set; }

    public int FeedbackType { get; set; }

    public int? UserIdTo { get; set; }

    public int? Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual User? UpdatedByNavigation { get; set; }

    public virtual User UserIdToNavigation { get; set; } = null!;
}
