using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;
using UserAdminAPI.DTO;
using UserAdminAPI.Interfaces;

namespace UserAdminAPI.Controllers
{
    [Route("api/users")]
    [ApiController]
    public class UserController : Controller
    {
        private readonly IUserService _userService;

        public UserController(IUserService userService)
        {
            _userService = userService;
        }


        [HttpPost("login")]
        public IActionResult Post([FromBody] LogInUserDto dto)
        {
            return Ok(_userService.Login(dto));
        }

        [HttpPost("register")]
        public IActionResult Regiter([FromBody] CreateUserDto dto)
        {
            return Ok(_userService.CreateUser(dto));
        }

        [HttpGet("{email}")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetByUsername(string email)
        {
            return Ok(_userService.GetUserByEmail(email));
        }


        [HttpPost("updateUser")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult UpdateUser([FromBody] UpdateUserDto dto)
        {
            return Ok(_userService.UpdateUser(dto));
        }

        [HttpPost("socialLogin")]
        public IActionResult SocialLogin([FromBody] SocialLoginDto model)
        {
            return Ok(_userService.SocialLogin(model));
        }

        [HttpGet("verifyCheck/{email}")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult VerifyCheckDeliverer(string email)
        {
            return Ok(_userService.VerifyCheckDeliverer(email));
        }
    }
}
