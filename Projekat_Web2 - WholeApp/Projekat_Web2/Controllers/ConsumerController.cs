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
    [Route("api/consumer")]
    [ApiController]
    public class ConsumerController : Controller
    {
        private readonly IConsumerService _consumerService;

        public ConsumerController(IConsumerService consumerService)
        {
            _consumerService = consumerService;
        }

        [HttpGet("getAllProducts")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetAllProducts()
        {
            return Ok(_consumerService.GetAllProducts());
        }


        [HttpPost("addProductToCart")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult AddProductToCart(AddToCartModelDto model)
        {
            return Ok(_consumerService.AddProductToCart(model));
        }

        [HttpGet("getMyProducts/{email}")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetMyProducts(string email)
        {
            return Ok(_consumerService.GetMyProducts(email));
        }

        [HttpPost("cancelProduct")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult CancelProduct(AddToCartModelDto model)
        {
            return Ok(_consumerService.CancelItem(model));
        }

        [HttpPost("makeOrder")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult MakeOrder(OrderDto order)
        {
            return Ok(_consumerService.MakeOrder(order));
        }

        [HttpGet("currentOrder/{email}")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetCurrentOrder(string email)
        {
            return Ok(_consumerService.GetCurrentOrder(email));
        }


        [HttpGet("getTime/{id}")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetTime(int id)
        {
            return Ok(_consumerService.GetTime(id));
        }

        [HttpGet("getPreviousOrders/{email}")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult GetPreviousOrders(string email)
        {
            return Ok(_consumerService.GetPreviousOrders(email));
        }

        [HttpPost("changeOrderStatus")]
        [Authorize(Roles = "consumer")]
        [Authorize(Policy = "SamoOdabrani")]
        public IActionResult ChangeOrderStatus(RetStringDto param)
        {
            return Ok(_consumerService.ChangeOrderStatus(param.RetValueNumer));
        }
    }
}
