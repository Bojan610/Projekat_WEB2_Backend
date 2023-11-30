using Projekat_Web2.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Interfaces
{
    public interface IDelivererService
    {
        List<OrderDto> GetOrders();
        bool PickUpOrder(PickupOrderDto order);
        OrderDto GetCurrentOrder(string email);
        StopWatchDto GetTime(int id);
        List<OrderDto> GetPreviousOrders(string email);
        bool ChangeOrderStatus(int orderId);
        List<ProductDto> GetAllProductsAdmin();
        bool AddNewProductAdmin(ProductDto product);
        List<OrderDto> GetOrdersAdmin();
    }
}
