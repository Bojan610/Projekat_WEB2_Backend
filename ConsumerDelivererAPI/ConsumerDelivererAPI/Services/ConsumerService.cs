using AutoMapper;
using ConsumerDelivererAPI.Dto;
using ConsumerDelivererAPI.Infrastructure;
using ConsumerDelivererAPI.Interfaces;
using ConsumerDelivererAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Linq;
using System.Threading;

namespace ConsumerDelivererAPI.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly ProductOrderDbContext _dbContext;
        private readonly object lockObject = new object();

        public ConsumerService(IMapper mapper, IConfiguration config, ProductOrderDbContext dbContext)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;
        }

        public List<ProductDto> GetAllProducts()
        {
            return _mapper.Map<List<ProductDto>>(_dbContext.Products.ToList());
        }

        public bool AddProductToCart(AddToCartModelDto model)
        {         
            Product product = _dbContext.Products.Find(model.ProductId);
            if (product == null)
                return false;

            lock (lockObject)
            {
                var opd = _dbContext.OrderProductDetails.ToList();
                bool found = false;
                foreach (OrderProductDetails item in opd)
                {
                    if (item.ConsumerId == model.email && item.ProductId == model.ProductId)
                    {
                        item.Quantity += model.Quantity;
                        _dbContext.SaveChanges();
                        found = true;
                        break;
                    }
                }

                if (!found)
                {
                    OrderProductDetails orderProductDetails = new OrderProductDetails();
                    orderProductDetails.ConsumerId = model.email;
                    orderProductDetails.DelivererId = "";
                    orderProductDetails.ProductId = model.ProductId;
                    orderProductDetails.Quantity = model.Quantity;
                    orderProductDetails.OrderId = -1;

                    _dbContext.OrderProductDetails.Add(orderProductDetails);
                    _dbContext.SaveChanges();
                }
            }

            return true;
        }

        public List<ProductDto> GetMyProducts(string email)
        {         
            var products = new List<Product>();
            var opd = _dbContext.OrderProductDetails.ToList();
            foreach (OrderProductDetails item in opd)
            {
                if (item.ConsumerId == email && item.OrderId == -1)
                {
                    Product product = _dbContext.Products.Find(item.ProductId);
                    for (int i = 0; i < item.Quantity; i++)
                    {
                        products.Add(product);
                    }
                }
            }

            return _mapper.Map<List<ProductDto>>(products);
        }

        public bool CancelItem(AddToCartModelDto model)
        {          
            lock (lockObject)
            {
                var opd = _dbContext.OrderProductDetails.ToList();
                foreach (OrderProductDetails item in opd)
                {
                    if (item.ConsumerId == model.email && item.ProductId == model.ProductId)
                    {
                        if (item.Quantity == 1)
                        {
                            _dbContext.OrderProductDetails.Remove(item);
                            _dbContext.SaveChanges();
                            return true;
                        }
                        else
                        {
                            item.Quantity -= 1;
                            _dbContext.SaveChanges();
                            return true;
                        }
                    }
                }
            }
            return false;
        }

        public bool MakeOrder(OrderDto order)
        {           
            lock (lockObject)
            {
                var products = _mapper.Map<List<Product>>(order.Products);

                order.Products = null;
                Order orderToAdd = _mapper.Map<Order>(order);
                _dbContext.Orders.Add(orderToAdd);
                _dbContext.SaveChanges();

                foreach (OrderProductDetails opd in _dbContext.OrderProductDetails.ToList())
                {
                    if (opd.ConsumerId == order.Email)
                        opd.OrderId = orderToAdd.Id;
                }
                _dbContext.SaveChanges();               
            }
            return true;
        }

        public OrderDto GetCurrentOrder(string email)
        {
            Thread.Sleep(100);
            OrderDto order = new OrderDto();
            order.Id = -1;
           
            OrderDto retOrder = null;
            foreach (OrderProductDetails opd in _dbContext.OrderProductDetails.ToList())
            {
                if (opd.ConsumerId == email && opd.OrderId != -1)
                {
                    if (retOrder == null)
                    {
                        retOrder = _mapper.Map<OrderDto>(_dbContext.Orders.Find(opd.OrderId));
                        retOrder.Products = new List<Product>();
                    }

                    Product product = _dbContext.Products.Find(opd.ProductId);
                    for (int i = 0; i < opd.Quantity; i++)
                    {
                        retOrder.Products.Add(product);
                    }
                }
            }
            if (retOrder != null)
                return retOrder;
            else
                return order;
        }

        public StopWatchDto GetTime(int id)
        {
            StopWatchDto stopWatchDto = new StopWatchDto();
            stopWatchDto.Minutes = DelivererService.Array[id].Item1;
            stopWatchDto.Seconds = DelivererService.Array[id].Item2;

            return stopWatchDto;
        }

        public bool ChangeOrderStatus(int orderId)
        {
            var or = _dbContext.Orders.Find(orderId);         
            if (or != null)
            {
                lock (lockObject)
                {
                    or.Status = "finished";                  
                    _dbContext.SaveChanges();
                    foreach (OrderProductDetails opd in _dbContext.OrderProductDetails.ToList())
                    {
                        if (opd.OrderId == orderId)
                        {
                            _dbContext.OrderProductDetails.Remove(opd);
                        }
                    }
                    _dbContext.SaveChanges();
                }
                return true;
            }

            return false;
        }

        public List<OrderDto> GetPreviousOrders(string email)
        {
            List<OrderDto> retList = new List<OrderDto>();
            foreach (Order item in _dbContext.Orders.ToList())
            {
                if (item.Status == "finished" && item.Email == email)
                    retList.Add(_mapper.Map<OrderDto>(item));
            }

            return retList;
        }
    }
}
