using Investly.DAL.Entities;
using System.ComponentModel.DataAnnotations;

namespace Investly.PL.Dtos
{
    public class UserDto
    {
        public int Id { get; set; }
        public string FirstName { get; set; }
        public string LastName { get; set; }
        public string Email { get; set; }
        public string? HashedPassword { get; set; }
        public string? Password { get; set; }
        public string? PhoneNumber { get; set; }
        public int? UserType { get; set; } 
        public string NationalId { get; set; }

        public DateOnly? DateOfBirth { get; set; }

        public string? Gender { get; set; }

        public string? FrontIdPicPath { get; set; }

        public string? BackIdPicPath { get; set; }

        public string? ProfilePicPath { get; set; }

        public int? GovernmentId { get; set; }

        public int? CityId { get; set; }
        public string? Address { get; set; }
        public int? CreatedBy { get; set; }
        public int? UpdatedBy { get; set; }
        public int? Status { get; set; }
        public DateTime? CreatedAt { get; set; }
        public DateTime? UpdatedAt { get; set; }

        public IFormFile? PicFile { get; set; }
        public IFormFile? FrontIdPicFile { get; set; }
        public IFormFile? BackIdPicFile { get; set; }
        public int? TokenVersion { get; set; }
        public int NotificationCountUnRead { get; set; }
        public  GovernmentDto? Government { get; set; }
        public  CityDto? City { get; set; }

    }
    public class UserLoginDto
    {
        [Required]
        [EmailAddress]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }

    public class DropdownDto
    {
        public int Id { get; set; }
        public string Name { get; set; }
    }

}
