﻿using ConsumerDelivererAPI.Dto;
using System.Collections.Generic;

namespace ConsumerDelivererAPI.Interfaces
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
