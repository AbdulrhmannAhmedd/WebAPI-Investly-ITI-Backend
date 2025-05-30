using System;
using System.Collections.Generic;

namespace Investly.DAL.Entities;

public partial class User
{
    public int Id { get; set; }

    public string FirstName { get; set; } = null!;

    public string LastName { get; set; } = null!;

    public string Email { get; set; } = null!;

    public string HashedPassword { get; set; } = null!;

    public DateOnly? DateOfBirth { get; set; }

    public string? Gender { get; set; }

    public string? PhoneNumber { get; set; }

    public string NationalId { get; set; } = null!;

    public string? FrontIdPicPath { get; set; }

    public string? BackIdPicPath { get; set; }

    public string? ProfilePicPath { get; set; }

    public int UserType { get; set; }

    public int? GovernmentId { get; set; }

    public int? CityId { get; set; }

    public string? Address { get; set; }

    public int Status { get; set; }

    public int? CreatedBy { get; set; }

    public DateTime? CreatedAt { get; set; }

    public int? UpdatedBy { get; set; }

    public DateTime? UpdatedAt { get; set; }

    public virtual ICollection<Business> BusinessCreatedByNavigations { get; set; } = new List<Business>();

    public virtual ICollection<BusinessStandardAnswer> BusinessStandardAnswerCreatedByNavigations { get; set; } = new List<BusinessStandardAnswer>();

    public virtual ICollection<BusinessStandardAnswer> BusinessStandardAnswerUpdatedByNavigations { get; set; } = new List<BusinessStandardAnswer>();

    public virtual ICollection<Business> BusinessUpdatedByNavigations { get; set; } = new List<Business>();

    public virtual ICollection<Category> CategoryCreatedByNavigations { get; set; } = new List<Category>();

    public virtual ICollection<CategoryStandard> CategoryStandardCreatedByNavigations { get; set; } = new List<CategoryStandard>();

    public virtual ICollection<CategoryStandard> CategoryStandardUpdatedByNavigations { get; set; } = new List<CategoryStandard>();

    public virtual ICollection<CategoryStandardsKeyWord> CategoryStandardsKeyWordCreatedByNavigations { get; set; } = new List<CategoryStandardsKeyWord>();

    public virtual ICollection<CategoryStandardsKeyWord> CategoryStandardsKeyWordUpdatedByNavigations { get; set; } = new List<CategoryStandardsKeyWord>();

    public virtual ICollection<Category> CategoryUpdatedByNavigations { get; set; } = new List<Category>();

    public virtual City? City { get; set; }

    public virtual User? CreatedByNavigation { get; set; }

    public virtual ICollection<Feedback> FeedbackCreatedByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackUpdatedByNavigations { get; set; } = new List<Feedback>();

    public virtual ICollection<Feedback> FeedbackUserIdToNavigations { get; set; } = new List<Feedback>();

    public virtual Founder? Founder { get; set; }

    public virtual Government? Government { get; set; }

    public virtual ICollection<User> InverseCreatedByNavigation { get; set; } = new List<User>();

    public virtual ICollection<User> InverseUpdatedByNavigation { get; set; } = new List<User>();

    public virtual Investor? Investor { get; set; }

    public virtual ICollection<InvestorContactRequest> InvestorContactRequestCreatedByNavigations { get; set; } = new List<InvestorContactRequest>();

    public virtual ICollection<InvestorContactRequest> InvestorContactRequestUpdatedByNavigations { get; set; } = new List<InvestorContactRequest>();

    public virtual ICollection<Notification> NotificationCreatedByNavigations { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationUpdatedByNavigations { get; set; } = new List<Notification>();

    public virtual ICollection<Notification> NotificationUserIdToNavigations { get; set; } = new List<Notification>();

    public virtual ICollection<Standard> StandardCreatedByNavigations { get; set; } = new List<Standard>();

    public virtual ICollection<Standard> StandardUpdatedByNavigations { get; set; } = new List<Standard>();

    public virtual User? UpdatedByNavigation { get; set; }
}
