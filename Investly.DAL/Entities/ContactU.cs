using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class ContactU
{
    public int Id { get; set; }

    public string? Name { get; set; }

    public string Description { get; set; } = null!;
}
