using Investly.DAL.Entities;

namespace Investly.PL.Dtos
{
    public class FounderDto
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }

        public  UserDto User { get; set; }
    }
    public class FounderSearchDto
    {
        public int PageSize { get; set; }
        public int PageNumber { get; set; }
        public string? SearchInput { get; set; }
        public int? GovernmentId { get; set; }
        public string? Gender { get; set; }
        public int? Status { get; set; }
    }
    public class FoundersPaginatedDto
    {
        public List<FounderDto> founders { get; set; } = new List<FounderDto>();
        public int TotalCount { get; set; }
        public int PageSize { get; set; }
        public int CurrentPage { get; set; }
    }
    public class FoundersTotalActiveIactiveDto
    {
        public int TotalActive { get; set; }
        public int TotalInactive { get; set; }
    }
}
