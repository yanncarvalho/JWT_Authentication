using System.ComponentModel.DataAnnotations;

namespace JwtAuthentication.Dto
{
    public class UserDto
    {
        public class Update
        {
            public string? Username { get; set; }

            public string? Password { get; set; }

            public string? Name { get; set; }

        }
        public class Login
        {
            [Required]
            public string? Username { get; set; }

            [Required]
            public string? Password { get; set; }

        }


    }
}

