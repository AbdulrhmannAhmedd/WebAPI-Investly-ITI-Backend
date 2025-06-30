using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Business
{
    public int Id { get; set; }

    public int FounderId { get; set; }

    public int CategoryId { get; set; }

    public string Title { get; set; } = null!;

    public decimal? Airate { get; set; }

    public int? Stage { get; set; }

    public string? Location { get; set; }

    public decimal? Capital { get; set; }

    public bool IsDrafted { get; set; }

    public string? FilePath { get; set; }

    public int? Status { get; set; }
    public string? RejectedReason { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }
    public int? GovernmentId { get; set; }

    public int? CityId { get; set; }
    public string? Description { get; set; }

    public int? DesiredInvestmentType { get; set; }
    public string? Images { get; set; }
    public virtual ICollection<BusinessStandardAnswer> BusinessStandardAnswers { get; set; } = new List<BusinessStandardAnswer>();

    public virtual Category Category { get; set; } = null!;

    public virtual User? CreatedByNavigation { get; set; }

    public virtual Founder Founder { get; set; } = null!;

    public virtual ICollection<InvestorContactRequest> InvestorContactRequests { get; set; } = new List<InvestorContactRequest>();

    public virtual User? UpdatedByNavigation { get; set; }
    public virtual City? City { get; set; }
    public virtual Government? Government { get; set; }
}
