using JwtAuthentication.Dto;
using JwtAuthentication.Models;

namespace JwtAuthentication.Services
{
    public interface IUserService
    {
        string? AuthenticateUser(String Password, String Username);

        bool Delete(int id);

        User? Get(int id);

        bool HasUsername(string username);

        void Update(User originalUser, UserDto.Update updateUser);

        void Save(User user);
    }
}
