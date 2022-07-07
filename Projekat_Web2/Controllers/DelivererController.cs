using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat_Web2.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Controllers
{
    [Route("api/deliverer")]
    [ApiController]
    public class DelivererController : Controller
    {
        private readonly IDelivererService _delivererService;

        public DelivererController(IDelivererService delivererService)
        {
            _delivererService = delivererService;
        }


        [HttpGet("verifyCheck/{email}")]
        public IActionResult VerifyCheck(string email)
        {
            return Ok(_delivererService.VerifyCheck(email));
        }

    }
}
