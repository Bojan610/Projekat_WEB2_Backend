using ConsumerDelivererAPI.Dto;
using System.Collections.Generic;

namespace ConsumerDelivererAPI.Interfaces
{
    public interface IConsumerService
    {
        List<ProductDto> GetAllProducts();
        bool AddProductToCart(AddToCartModelDto model);
        List<ProductDto> GetMyProducts(string email);
        bool CancelItem(AddToCartModelDto model);

        bool MakeOrder(OrderDto order);

        OrderDto GetCurrentOrder(string email);
        StopWatchDto GetTime(int id);
        List<OrderDto> GetPreviousOrders(string email);
        bool ChangeOrderStatus(int orderId);
    }
}
