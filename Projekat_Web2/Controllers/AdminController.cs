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
    [Route("api/admin")]
    [ApiController]
    public class AdminController : Controller
    {
        private readonly IAdminService _adminService;

        public AdminController(IAdminService adminService)
        {
            _adminService = adminService;
        }

        [HttpGet("getProcessing")]
        public IActionResult GetProcessing()
        {
            return Ok(_adminService.GetProcessing());
        }

        [HttpGet("getDenied")]
        public IActionResult GetDenied()
        {
            return Ok(_adminService.GetDenied());
        }

        [HttpGet("getAccepted")]
        public IActionResult GetAccepted()
        {
            return Ok(_adminService.GetAccepted());
        }

        [HttpPost("acceptDeliverer")]
        public IActionResult AcceptDeliverer(RetStringDto email)
        {
            return Ok(_adminService.AcceptDeliverer(email.RetValue));
        }

        [HttpPost("declineDeliverer")]
        public IActionResult DeclineDeliverer(RetStringDto email)
        {
            return Ok(_adminService.DeclineDeliverer(email.RetValue));
        }
    }
}
