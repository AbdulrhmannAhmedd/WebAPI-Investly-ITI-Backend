using AutoMapper;
using Investly.DAL.Entities;
using Investly.DAL.Repos.IRepos;
using Investly.PL.Dtos;
using Investly.PL.IBL;

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



    }
}
