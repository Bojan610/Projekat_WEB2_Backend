using AutoMapper;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;
using Projekat_Web2.DTO;
using Projekat_Web2.Infrastructure;
using Projekat_Web2.Interfaces;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading;
using System.Threading.Tasks;

namespace Projekat_Web2.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly WebAppDbContext _dbContext;
        private readonly object lockObject = new object();      

        public ConsumerService(IMapper mapper, IConfiguration config, WebAppDbContext dbContext)
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
            //User user = _dbContext.Users.Find(model.email);
            //User user = users.First(x => x.Email == model.email);
            //if (user == null)
                //return false;

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
            //User user = _dbContext.Users.Find(email);
            //User user = users.First(x => x.Email == email);
            //if (user == null)
                //return null;

            //var products = _dbContext.Products.Where(product => product.Consumers.Any(j => j.Email == email));

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
            //User user = _dbContext.Users.Find(model.email);
            //User user = users.First(x => x.Email == model.email);
            //if (user == null)
               // return false;
      
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
            //User user = _dbContext.Users.Find(order.Email);
            //User user = users.First(x => x.Email == order.Email);
            //if (user == null)
                //return false;

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
               
                /*foreach (Product item in products)
                {
                    orderToAdd.Products.Add(_dbContext.Products.Find(item.Id));
                }
                _dbContext.SaveChanges();
                */

                //((Consumer)user).CurrentOrder_Con = orders.First(x => x.Id == order.Id);
                //((Consumer)user).CurrentOrder_Con = orderToAdd;
                //_dbContext.SaveChanges();

                /*var u = _dbContext.Users.Include(c => ((Consumer)c).Products).First(i => i.Email == user.Email);
                foreach (Product item in ((Consumer)u).Products.ToList())
                {                 
                     ((Consumer)u).Products.Remove(item);
                }

                _dbContext.SaveChanges();*/
            }
            return true;
        }

        public OrderDto GetCurrentOrder(string email)
        {
            Thread.Sleep(100);
            OrderDto order = new OrderDto();
            order.Id = -1;

            //User user = _dbContext.Users.Find(email);
            //User user = users.First(x => x.Email == email);
            //if (user == null)
            //return null;

            /*var u = _dbContext.Users.Include(c => ((Consumer)c).CurrentOrder_Con).First(i => i.Email == user.Email);
            if (((Consumer)u).CurrentOrder_Con == null)
                return order;
            else
            {
                var retOrder = _mapper.Map<OrderDto>(((Consumer)u).CurrentOrder_Con);
                foreach (OrderProductDetails opd in _dbContext.OrderProductDetails.ToList())
                {
                    if (opd.OrderId == retOrder.Id)
                    {
                        Product product = _dbContext.Products.Find(opd.ProductId);
                        for (int i = 0; i < opd.Quantity; i++)
                        {
                            retOrder.Products.Add(product);
                        }
                    }                     
                }
                return retOrder;
            }*/
            OrderDto retOrder = null;
            foreach (OrderProductDetails opd in _dbContext.OrderProductDetails.ToList())
            {
                if (opd.ConsumerId == email && opd.OrderId != -1)
                {
                    if (retOrder == null)
                        retOrder = _mapper.Map<OrderDto>(_dbContext.Orders.Find(opd.OrderId));
                    
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
            //var del = _dbContext.Orders.Include(del => del.DelivererCurrent).First(i => i.Id == orderId);
            //var con = _dbContext.Orders.Include(con => con.ConsumerCurrent).First(i => i.Id == orderId);
            if (or != null)
            {
                lock (lockObject)
                {
                    or.Status = "finished";
                    //del.DelivererCurrent = null;
                    //con.ConsumerCurrent = null;
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
