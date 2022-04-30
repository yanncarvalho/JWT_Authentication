using JwtAuthentication.Models;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Util
{
    public class JwtToken
    {
        private static byte[] _secret = null;
        public static byte[] Secret { set => _secret = value; }
  
        public static string GenerateToken(User user)
        {    

            var tokenHandler = new JwtSecurityTokenHandler();

       

            var tokenDescriptor = new SecurityTokenDescriptor
            {

                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.NameIdentifier, (user.Id + ushort.MaxValue).ToString())
                }),
                Expires = DateTime.UtcNow.AddDays(30),
        
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(_secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
