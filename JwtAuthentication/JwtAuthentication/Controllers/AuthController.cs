using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;

namespace JwtAuthentication.Controllers
{
    [Route("/")]
     [ApiController]
    public class AuthController : ControllerBase
    {
        [HttpPost]
        [Consumes(MediaTypeNames.Application.Json)]
        public void SaveUser([FromBody] User user) => Console.WriteLine(user.Name);

        [HttpGet]
        public void GetUser()
        {
            Console.WriteLine("aaa");
         }
    }
}
