using Microsoft.EntityFrameworkCore;
using System.ComponentModel.DataAnnotations;
using System.Text.Json.Serialization;

namespace JwtAuthentication.Models
{
    public class User
    {
        private string _password;

        [Key]
        [JsonIgnore]
        public int Id { get; set; }

        [Required]
        public string Username { get; set; }

        [Required]
        public string Password { get => _password ; set => _password = Auth.BCryptPassword.HashGeneration(value); }

        [Required]
        public string? Name { get; set; }

        public bool PasswordCompare(string password)
        {
            return Auth.BCryptPassword.PasswordCompare(hash: _password, password);
        }
    }

}