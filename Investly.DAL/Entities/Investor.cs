using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class Investor
{
    public int Id { get; set; }

    public int UserId { get; set; }

    public int InvestingType { get; set; }

    public virtual ICollection<InvestorContactRequest> InvestorContactRequests { get; set; } = new List<InvestorContactRequest>();

    public virtual User User { get; set; } = null!;
}
