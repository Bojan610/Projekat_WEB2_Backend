using AutoMapper;
using ConsumerDelivererAPI.Dto;
using ConsumerDelivererAPI.Interfaces;
using ConsumerDelivererAPI.Models;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Threading.Tasks;
using System.Threading;
using System;
using ConsumerDelivererAPI.Infrastructure;
using System.Linq;

namespace ConsumerDelivererAPI.Services
{
    public class DelivererService : IDelivererService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly ProductOrderDbContext _dbContext;
        private readonly object lockObject = new object();

        private static Tuple<double, double>[] array = new Tuple<double, double>[100];

        public static Tuple<double, double>[] Array { get => array; set => array = value; }

        public DelivererService(IMapper mapper, IConfiguration config, ProductOrderDbContext dbContext)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;
        }

        public List<OrderDto> GetOrders()
        {
            List<OrderDto> retList = new List<OrderDto>();
            foreach (Order item in _dbContext.Orders.ToList())
            {
                if (item.Status == "waiting")
                    retList.Add(_mapper.Map<OrderDto>(item));
            }

            return retList;
        }

        public bool PickUpOrder(PickupOrderDto order)
        {           
            lock (lockObject)
            {
                Order or = _dbContext.Orders.Find(order.Id);
                if (or == null)
                    return false;

                or.Status = "picked up";
                or.DelivererEmail = order.EmailDeliverer;
                _dbContext.SaveChanges();              
            }
            Tuple<double, double> t1 = new Tuple<double, double>(0, 0);
            Array[order.Id] = t1;
          
            Task t = new Task(delegate { StartStopWatch(order.Id); });
            t.Start();           

            return true;
        }

        public OrderDto GetCurrentOrder(string email)
        {
            Thread.Sleep(100);
            OrderDto order = new OrderDto();
            order.Id = -1;           

            var orders = _dbContext.Orders.ToList();
            foreach (Order item in orders)
            {
                if (item.DelivererEmail == email && item.Status == "picked up")
                    return _mapper.Map<OrderDto>(item);
            }

            return order;
        }

        private void StartStopWatch(int i)
        {
            Random rand = new Random();
            int timer = rand.Next(100, 200);

            while (timer > 0)
            {
                Thread.Sleep(1000);
                timer--;

                double minutes = Math.Floor((double)timer / 60);
                double seconds = timer - (minutes * 60);

                lock (lockObject)
                {
                    Array[i] = new Tuple<double, double>(minutes, seconds);
                }
            }
        }

        public StopWatchDto GetTime(int id)
        {
            StopWatchDto stopWatchDto = new StopWatchDto();
            stopWatchDto.Minutes = Array[id].Item1;
            stopWatchDto.Seconds = Array[id].Item2;

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
                if (item.Status == "finished" && item.DelivererEmail == email)
                    retList.Add(_mapper.Map<OrderDto>(item));
            }

            return retList;
        }

        public List<ProductDto> GetAllProductsAdmin()
        {
            return _mapper.Map<List<ProductDto>>(_dbContext.Products.ToList());
        }

        public bool AddNewProductAdmin(ProductDto product)
        {
            if (product.ProductName == "")
                return false;

            lock (lockObject)
            {
                _dbContext.Products.Add(_mapper.Map<Product>(product));
                _dbContext.SaveChanges();
            }
            return true;
        }

        public List<OrderDto> GetOrdersAdmin()
        {
            return _mapper.Map<List<OrderDto>>(_dbContext.Orders.ToList());
        }
    }
}
