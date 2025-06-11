namespace Investly.PL.Dtos
{
    public class InvestorDto
    {
        public int? Id { get; set; }
        public int? UserId { get; set; }
        public UserDto User { get; set; }
        public int? InvestingType { get; set; }
        public string? InterestedBusinessStages { get; set; }
        public int? MinFunding { get; set; }
        public int? MaxFunding { get; set; }
    }
    public class InvestorSearchDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }  
        public string? SearchInput { get; set; }
        public int ? GovernmentId { get; set; } 
        public string ?Gender { get; set; }   
        public int Status { get; set; }  

    }

    public class InvestorDtoWithPagination
    {
        public List<InvestorDto> List { get; set; } = new List<InvestorDto>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
    public class InvestorTotalActiveIactiveDto
    {
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
    }



}
