using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IUserService
    {
        public UserDto? GetByEmail(string Email);
    }
}
