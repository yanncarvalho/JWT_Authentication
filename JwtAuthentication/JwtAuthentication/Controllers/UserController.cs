using JwtAuthentication.Dto;
using JwtAuthentication.Models;
using JwtAuthentication.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Net.Mime;
using System.Security.Claims;
using Microsoft.AspNetCore.Authentication.JwtBearer;

namespace JwtAuthentication.Controllers
{


    [Route("user")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private static int? UserIdByClaim(Claim userId)
        {

            if(int.TryParse(userId.Value, out int parsed) )
            {
                return parsed - ushort.MaxValue;
            }

            return null;
        }

        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost]
        [Produces(MediaTypeNames.Application.Json)]
        [Consumes(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status201Created)]
        [AllowAnonymous]
        [Route("save")]
        public ActionResult Save ([FromBody] User user)
        {
           if (_userService.HasUsername(user.Username))
            {
                return Conflict(new { message = "Username already exists" });
            }
            else
            {
                _userService.Save(user);
                return StatusCode(201);
            }

        }



        [HttpPost]
        [Route("login")]
        [AllowAnonymous]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        public ActionResult Login([FromBody] LoginDto login)
        {
          string? token = _userService.AuthenticateUser(login.Username, login.Password);

          if(token != null)
          {
                return Ok(new { token = token });
          }
          else
          {
             return Unauthorized (new { message = "Authentication is not valid" });
          }
        }

        [HttpGet]
        [Route("get")]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult GetUser()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

                int? parsed = UserIdByClaim(userId);
            if (parsed.HasValue)
            {
                var user = _userService.Get(parsed.Value);
                if (user != null)
                {
                    return Ok(new { Name = user.Name, Username = user.Username });
                }

            }
             return NotFound(new { message = "User not found" });
        }

        [HttpDelete]
        [Route("delete")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Delete()
        {
            var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

            int? parsed = UserIdByClaim(userId);

            if (parsed.HasValue)
            {
              bool deleted = _userService.Delete(parsed.Value);

                if (deleted)
                {
                    return Ok();
                }
            }
           return Conflict(new { message = "Delete operation was not succeded" });

        }

        [HttpPut]
        [Route("update")]
        [Consumes(MediaTypeNames.Application.Json)]
        [Produces(MediaTypeNames.Application.Json)]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [Authorize(AuthenticationSchemes = JwtBearerDefaults.AuthenticationScheme)]
        public ActionResult Update([FromBody] UpdateDto updateUsr)
        {
           var userId = User.Claims.FirstOrDefault(x => x.Type == ClaimTypes.NameIdentifier);

           int? parsed = UserIdByClaim(userId);
           if (parsed.HasValue)
            {
                var originalUsr = _userService.Get(parsed.Value);
                if(originalUsr != null)
                {
                    if (updateUsr.Username != null && !String.Equals(originalUsr.Username, updateUsr.Username) && _userService.HasUsername(updateUsr.Username))
                    {
                        return Conflict(new { message = "Username already exists" });
                    }
                    _userService.Update(originalUsr, updateUsr);
                     return Ok();
                }
            }
            return Conflict(new { message = "Update operation was not succeded" });
        }


    }
}
