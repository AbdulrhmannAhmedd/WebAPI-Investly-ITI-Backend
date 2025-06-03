namespace Investly.PL.General
{
    public enum UserType
    {
        Staff = 1,
        Investor = 2,
        Founder = 3,
    }
    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Deleted = 3,
    }
    public enum InvestorInvestingType
    {
        Experience = 1,
        Money = 2,
        Both = 3
    }
    public enum BusinessIdeaStatus
    {
        Active = 1,
        Inactive = 2,
        Deleted = 3,
        Pending = 4,
        Rejected = 5,
    }
    public enum Stage
    {
        Startup=1,
        Growth=2,
        Maturity=3,
        Decline=4,
        Renewal=5
    }


}
