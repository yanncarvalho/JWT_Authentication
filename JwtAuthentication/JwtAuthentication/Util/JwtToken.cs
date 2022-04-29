using JwtAuthentication.Controllers;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace JwtAuthentication.Util
{
    public class JwtToken
    {
        private readonly IConfiguration _config;

        public JwtToken(IConfiguration config)
        {
            this._config = config;
        }

        public string GenerateToken(User user)
        {    

            var tokenHandler = new JwtSecurityTokenHandler();
           
            var secret = Encoding.ASCII.GetBytes(_config.GetValue<string>("JwtToken:Secret"));
            var tokenDescriptor = new SecurityTokenDescriptor
            {
                Subject = new ClaimsIdentity(new Claim[]
                {
                    new Claim(ClaimTypes.Name, user.Username)
                }), 
                Expires = DateTime.UtcNow.AddDays(30),
                SigningCredentials = new SigningCredentials(new SymmetricSecurityKey(secret), SecurityAlgorithms.HmacSha256Signature)
            };
            var token = tokenHandler.CreateToken(tokenDescriptor);
            return tokenHandler.WriteToken(token);
        }
    }

}
