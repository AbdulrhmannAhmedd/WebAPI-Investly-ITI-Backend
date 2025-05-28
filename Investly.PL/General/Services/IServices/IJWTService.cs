using Investly.PL.Dtos;

namespace Investly.PL.General.Services.IServices
{
    public interface IJWTService
    {
        public string GenerateToken(UserDto user);
    }
}
