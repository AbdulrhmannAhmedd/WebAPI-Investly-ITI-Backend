using Investly.PL.Dtos;

namespace Investly.PL.IBL
{
    public interface IUserService
    {
        public UserDto? GetByEmail(string Email);
        public UserDto? GetById(int Id);
        public int UpdateToken(int userId,bool? doIncrement);

        public Task<List<DropdownDto>> GetAppropiateUserForFeedback(int userId);

    }
}
