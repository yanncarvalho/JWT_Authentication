using JwtAuthentication.Dto;
using JwtAuthentication.Models;
using JwtAuthentication.Util;

namespace JwtAuthentication.Services.ServicesImpl
{
    public class UserService : IUserService
    {
        private readonly UserContext _context;


        public UserService(UserContext context)
        {
            _context = context;
        }

        public string? AuthenticateUser(string username, string password)
        {
            User? result = _context.Users.Where(x => x.Username == username).FirstOrDefault();
            if (result != null && result.PasswordCompare(password))
            {
                return JwtToken.GenerateToken(result);
            } else return null;
        }
        public bool Delete(int id)
        {
            var user = Get(id);
            if(user != null)
            {
                _context.Users.Attach(user);
                _context.Users.Remove(user);
                _context.SaveChanges();
                return true;

            }

            return false;

        }

        public User? Get(int id)
        {
            return _context.Users.Find(id);  
        }

        public bool HasUsername(string username)
        {
            return _context.Users.Where(x => x.Username == username).FirstOrDefault() != null;
        }

        public void Save(User user)
        {
            _context.Users.Add(user);
            _context.SaveChanges();
        }

        void IUserService.Update(User originalUser, UserDto.Update updateUser)
        {
            _context.Users.Attach(originalUser);

            if (updateUser.Name != null)
            {
                originalUser.Name = updateUser.Name;
            }
            if (updateUser.Password != null)
            {
                originalUser.Password = updateUser.Password;
            }
            if (updateUser.Username != null)
            {
                originalUser.Username = updateUser.Username;
            }

            _context.SaveChanges();
        }
    }
}
