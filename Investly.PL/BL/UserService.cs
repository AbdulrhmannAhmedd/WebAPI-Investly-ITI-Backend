using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.General.Services.IServices;
using Investly.PL.IBL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;
using System.Security.Cryptography;

namespace Investly.PL.BL
{
    public class UserService:IUserService
    {
         private readonly IUnitOfWork _unitOfWork;
         private readonly IMapper _mapper;
        private readonly IEmailSender _emailSender;
        private readonly IConfiguration _config;

        public UserService(IUnitOfWork unitOfWork, IMapper mapper, IEmailSender emailSender, IConfiguration config)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;
            _emailSender = emailSender;
            _config = config;
        }

        public UserDto? GetByEmail(string Email)
        {
            try
            {

                var user = _unitOfWork.UserRepo.FirstOrDefault(u => u.Email == Email);
                if (user == null)
                {
                    return null; 
                }
                var userDto= _mapper.Map<UserDto>(user);
                return userDto;
            }
            catch (Exception ex)
            {
                return null;
            }
        }

        public UserDto? GetById(int Id)
        {
            try
            {
                var user = _unitOfWork.UserRepo.GetById(Id);
                if (user == null)
                {
                    return null;
                }
                return _mapper.Map<UserDto>(user);

            }
            catch(Exception ex)
            {
                return null;
            }
        }

        public int UpdateToken(int userId, bool? doIncrement)
        {
            int res = 0;
            try
            {
                var user = _unitOfWork.UserRepo.FirstOrDefault(u => u.Id == userId);
                if (user != null)
                {
                    user.TokenVersion = (bool)doIncrement ? user.TokenVersion==null?1:user.TokenVersion++ : null;
                    user.UpdatedAt = DateTime.UtcNow;
                    _unitOfWork.UserRepo.Update(user);
                    res = _unitOfWork.Save();
                }
                return res;

            }
            catch (Exception ex)
            {
              return res;
            }
        }


        public async Task<List<DropdownDto>> GetAppropiateUserForFeedback(int userId)
        {
            var userType = await _unitOfWork.UserRepo.GetByIdAsync(userId);

            if (userType.UserType == 3) 
            {

                return await _unitOfWork.InvestorContactRequestRepo.FindAll()
                    .Where(icr => icr.Status == (int)ContactRequestStatus.Accepted&& 
                                 icr.Business.Founder.UserId == userId)
                    .Select(icr => new DropdownDto
                    {
                        Id = icr.Investor.User.Id,
                        Name = $"{icr.Investor.User.FirstName} {icr.Investor.User.LastName}"
                    })
                    .Distinct()
                    .ToListAsync();
            }
            else if (userType.UserType == 2) 
            {
                return await _unitOfWork.InvestorContactRequestRepo.FindAll()
                    .Where(icr => icr.Status == (int)ContactRequestStatus.Accepted && 
                                 icr.Investor.UserId == userId)
                    .Select(icr => new DropdownDto
                    {
                        Id = icr.Business.Founder.User.Id,
                        Name = $"{icr.Business.Founder.User.FirstName} {icr.Business.Founder.User.LastName}"
                    })
                    .Distinct()
                    .ToListAsync();
            }

            return new List<DropdownDto>();
        }

        public async Task<string> RequestToChangePasswordAsync(PasswordResetRequestDto model)
        {
            var user = await _unitOfWork.UserRepo.FirstOrDefaultAsync(user => user.Email == model.Email);
            if (user == null)
                return "If your email exists in our system, you'll receive a password reset link";

            var token = GenerateSecureToken();

            var resetToken = new PasswordResetToken
            {
                UserId = user.Id,
                Token = token,
                Expiration = DateTime.UtcNow.AddHours(1)
            };

            await _unitOfWork.PasswordTokenRepo.InsertAsync(resetToken);
            await _unitOfWork.SaveAsync();

            var resetLink = $"{_config["ClientApp:BaseUrl"]}/reset-password?email={Uri.EscapeDataString(model.Email)}&token={Uri.EscapeDataString(token)}";

            await _emailSender.SendEmailAsync(
                model.Email,
                "Password Reset Request",
                $"Hello {user.Email},<br><br>Please reset your password by <a href='{resetLink}'>clicking here</a>.<br><br>This link will expire in 1 hour.");

            return "If your email exists in our system, you'll receive a password reset link";
        }

        private string GenerateSecureToken()
        {
            var tokenBytes = new byte[32];
            using (var rng = RandomNumberGenerator.Create())
            {
                rng.GetBytes(tokenBytes);
            }
            return Convert.ToBase64String(tokenBytes);
        }



    }
}
