using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class User
{
    public int Id { get; set; }

    public string? FirstName { get; set; }

    public string? LastName { get; set; }

    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public string? PhoneNumber { get; set; }

    public DateOnly? DateOfBirth { get; set; }

    public bool? Gender { get; set; }

    public string NationalId { get; set; } = null!;

    public string? FrontIdPicPath { get; set; }

    public string? BackIdPicPath { get; set; }

    public byte[]? ProfilePicPath { get; set; }

    public int UserType { get; set; }

    public int? GovernmentId { get; set; }

    public int? CityId { get; set; }

    public string? Address { get; set; }

    public int Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual City? City { get; set; }

    public virtual ICollection<Founder> Founders { get; set; } = new List<Founder>();

    public virtual Government? Government { get; set; }

    public virtual ICollection<Investor> Investors { get; set; } = new List<Investor>();
}
