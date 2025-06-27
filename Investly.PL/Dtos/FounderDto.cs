using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.IBL;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity;
using System.ComponentModel.DataAnnotations;
using System.Security.Claims;

namespace Investly.PL.Dtos
{
    public class FounderDto
    {
        public int? Id { get; set; }

        public int? UserId { get; set; }

        public UserDto User { get; set; }
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


    public class UpdateFounderDto
    {
        //[Required]
        //public int Id { get; set; }

        [Required(ErrorMessage = "First name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "First name must be between 2 and 50 characters.")]
        public string FirstName { get; set; }

        [Required(ErrorMessage = "Last name is required.")]
        [StringLength(50, MinimumLength = 2, ErrorMessage = "Last name must be between 2 and 50 characters.")]
        public string LastName { get; set; }

        [Phone(ErrorMessage = "Invalid phone number format.")]
        [StringLength(20, ErrorMessage = "Phone number can't exceed 20 characters.")]
        //[UniquePhoneNumber]
        public string? PhoneNumber { get; set; }

        [RegularExpression("^(Male|Female)?$", ErrorMessage = "Gender must be either 'Male' or 'Female'.")]
        public string? Gender { get; set; }

        [Required(ErrorMessage = "Government is required.")]
        public int? GovernmentId { get; set; }  

        [Required(ErrorMessage = "City is required.")]
        public int? CityId { get; set; }

        [StringLength(200, ErrorMessage = "Address cannot exceed 200 characters.")]
        public string? Address { get; set; }
        [DataType(DataType.Date)]
        //[MinimumAge(21)]
        public DateOnly? DateOfBirth { get; set; }

        public bool Equals(UpdateFounderDto? other)
        {
            if (other == null) return false;

            return FirstName == other.FirstName &&
                   LastName == other.LastName &&
                   PhoneNumber == other.PhoneNumber &&
                   Gender == other.Gender &&
                   GovernmentId == other.GovernmentId &&
                   CityId == other.CityId &&
                   Address == other.Address &&
                   DateOfBirth == other.DateOfBirth;
        }

        public override bool Equals(object? obj) => Equals(obj as UpdateFounderDto);

        public override int GetHashCode()
        {
            var hash = new HashCode();
            hash.Add(FirstName);
            hash.Add(LastName);
            hash.Add(PhoneNumber);
            hash.Add(Gender);
            hash.Add(GovernmentId);
            hash.Add(CityId);
            hash.Add(Address);
            hash.Add(DateOfBirth);
            return hash.ToHashCode();

        }

    }

    public class ChangePasswordDto
    {
        public string email { get; set; }
        //[ValidateCurrentPassword]
        [Required(ErrorMessage = "Current password is required")]
        [DataType(DataType.Password)]
        public string CurrentPassword { get; set; } = null!;

        [Required(ErrorMessage = "New password is required")]
        [DataType(DataType.Password)]
        [StringLength(100, MinimumLength = 8, ErrorMessage = "Password must be between 8 and 100 characters")]
        [RegularExpression(@"^(?=.*[a-z])(?=.*[A-Z])(?=.*\d)(?=.*[!@#$%^&*(),.?\""{}|<>]).*$",
            ErrorMessage = "Password must contain at least: 1 uppercase, 1 lowercase, 1 number, and 1 special character (!@#$%^&*(),.?\"{}|<>)")]
        public string NewPassword { get; set; } = null!;

        [Required(ErrorMessage = "Please confirm your new password")]
        [DataType(DataType.Password)]
        [Compare("NewPassword", ErrorMessage = "Passwords do not match")]
        public string ConfirmNewPassword { get; set; } = null!;
    }







    // validation for date
    //public class MinimumAgeAttribute : ValidationAttribute
    //{
    //    private readonly int _minAge;

    //    public MinimumAgeAttribute(int minAge)
    //    {
    //        _minAge = minAge;
    //        ErrorMessage = $"Age must be at least {_minAge} years.";
    //    }

    //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //    {
    //        if (value is null)
    //            return ValidationResult.Success;


    //        if (value is DateOnly BOD)
    //        {
    //            var today = DateOnly.FromDateTime(DateTime.Today);
    //            int age = today.Year - BOD.Year;

    //            if (today < BOD.AddYears(age))
    //                age--;

    //            if (age < _minAge)
    //                return new ValidationResult(ErrorMessage);

    //        }
    //        return ValidationResult.Success;
    //    }
    //}


    //public class UniquePhoneNumberAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //    {
    //        if (value is null || (value is string str && string.IsNullOrWhiteSpace(str)))
    //            return ValidationResult.Success;

    //        try
    //        {
    //            // Get HTTP context accessor
    //            var httpContextAccessor = (IHttpContextAccessor)validationContext
    //                .GetRequiredService(typeof(IHttpContextAccessor));

    //            // Get route data
    //            var routeData = httpContextAccessor.HttpContext?.GetRouteData();
    //            var idFromRoute = routeData?.Values["id"]?.ToString();

    //            if (!int.TryParse(idFromRoute, out int routeId))
    //                return new ValidationResult("Invalid ID in route");

    //            // Get repository
    //            var userRepo = (IUserRepo)validationContext
    //                .GetRequiredService(typeof(IUserRepo));

    //            var existingUser = userRepo.FirstOrDefault(u =>
    //                u.PhoneNumber == value.ToString() &&
    //                u.Id != routeId); // Compare with ID from route

    //            return existingUser == null
    //                ? ValidationResult.Success
    //                : new ValidationResult("Phone number must be unique.");
    //        }
    //        catch
    //        {
    //            return ValidationResult.Success; // Fail gracefully
    //        }
    //    }
    //}


    // extension methods for founder 
    public static class FounderExtensions
    {
        public static UpdateFounderDto ToUpdateDto(this Founder founder)
        {
            return new UpdateFounderDto
            {
                FirstName = founder.User.FirstName,
                LastName = founder.User.LastName,
                PhoneNumber = founder.User.PhoneNumber,
                Gender = founder.User.Gender,
                GovernmentId = founder.User.GovernmentId,
                CityId = founder.User.CityId,
                Address = founder.User.Address,
                DateOfBirth = founder.User.DateOfBirth
            };
        }
    }

    //public class ValidateCurrentPasswordAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult IsValid(object value, ValidationContext validationContext)
    //    {
    //        var httpContext = validationContext.GetRequiredService<IHttpContextAccessor>().HttpContext;
    //        var userManager = validationContext.GetRequiredService<UserManager<IdentityUser>>();

    //        // Get current user
    //        var userId = httpContext.User.FindFirstValue(ClaimTypes.NameIdentifier);
    //        var user = userManager.FindByIdAsync(userId).Result;

    //        // Verify current password
    //        var currentPassword = value as string;
    //        if (!userManager.CheckPasswordAsync(user, currentPassword).Result)
    //        {
    //            return new ValidationResult("Current password is incorrect");
    //        }

    //        return ValidationResult.Success;
    //    }
    //}

    //public class ValidateCurrentPasswordAttribute : ValidationAttribute
    //{
    //    protected override ValidationResult? IsValid(object? value, ValidationContext validationContext)
    //    {
    //        var currentPassword = value as string;
    //        if (string.IsNullOrWhiteSpace(currentPassword))
    //            return new ValidationResult("Current password is required");

    //        // Access the full DTO model
    //        var model = (ChangePasswordDto)validationContext.ObjectInstance;

    //        // Resolve your user service from DI
    //        var userService = (IUserService)validationContext.GetService(typeof(IUserService))!;
    //        var user = userService.GetById(model.Id);

    //        if (user == null)
    //            return new ValidationResult("User not found");

    //        // Verify current password
    //        if (!BCrypt.Net.BCrypt.Verify(currentPassword, user.HashedPassword))
    //            return new ValidationResult("Current password is incorrect");

    //        return ValidationResult.Success;
    //    }
    //}



}

