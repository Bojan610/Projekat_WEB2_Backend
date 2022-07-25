using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Projekat_Web2.DTO;
using Projekat_Web2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Controllers
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
    }
}
