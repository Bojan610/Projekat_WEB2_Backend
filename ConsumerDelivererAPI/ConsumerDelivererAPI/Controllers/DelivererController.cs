using ConsumerDelivererAPI.Dto;
using ConsumerDelivererAPI.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Data;

namespace ConsumerDelivererAPI.Controllers
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

        [HttpPost("changeOrderStatus")]
        [Authorize(Roles = "deliverer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult ChangeOrderStatus(RetStringDto param)
        {
            return Ok(_delivererService.ChangeOrderStatus(param.RetValueNumer));
        }

        [HttpGet("getAllProducts")]
        [Authorize(Roles = "admin")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetAllProductsAdmin()
        {
            return Ok(_delivererService.GetAllProductsAdmin());
        }

        [HttpPost("addNewProduct")]
        [Authorize(Roles = "admin")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult AddNewProductAdmin(ProductDto product)
        {
            return Ok(_delivererService.AddNewProductAdmin(product));
        }

        [HttpGet("getOrdersAdmin")]
        [Authorize(Roles = "admin")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetOrdersAdmin()
        {
            return Ok(_delivererService.GetOrdersAdmin());
        }
    }
}
