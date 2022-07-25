using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Projekat_Web2.DTO;
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
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult VerifyCheck(string email)
        {
            return Ok(_delivererService.VerifyCheck(email));
        }

        [HttpGet("getOrders")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetOrders()
        {
            return Ok(_delivererService.GetOrders());
        }

        [HttpPost("pickUpOrder")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult PickUpOrder(PickupOrderDto order)
        {
            return Ok(_delivererService.PickUpOrder(order));
        }

        [HttpGet("currentOrder/{email}")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetCurrentOrder(string email)
        {
            return Ok(_delivererService.GetCurrentOrder(email));
        }

        [HttpGet("getTime/{id}")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetTime(int id)
        {
            return Ok(_delivererService.GetTime(id));
        }

        [HttpGet("getPreviousOrders/{email}")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetPreviousOrders(string email)
        {
            return Ok(_delivererService.GetPreviousOrders(email));
        }
    }
}
