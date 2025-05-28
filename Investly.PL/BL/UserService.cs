using AutoMapper;
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
                return _mapper.Map<UserDto>(user);
            }
            catch (Exception ex)
            {
                return null;
            }
        }
    }
}
