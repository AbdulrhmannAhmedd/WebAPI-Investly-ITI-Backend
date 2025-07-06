using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.General;
using Investly.PL.IBL;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Investly.PL.BL
{
    public class UserService:IUserService
    {
         private readonly IUnitOfWork _unitOfWork;
         private readonly IMapper _mapper;
        public UserService(IUnitOfWork unitOfWork, IMapper mapper)
        {
            _unitOfWork = unitOfWork;
            _mapper = mapper;

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



    }
}
