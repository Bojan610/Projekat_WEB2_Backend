using AutoMapper;
using Microsoft.Extensions.Configuration;
using Projekat_Web2.DTO;
using Projekat_Web2.Infrastructure;
using Projekat_Web2.Interfaces;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Services
{
    public class ConsumerService : IConsumerService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly WebAppDbContext _dbContext;
        private readonly object lockObject = new object();
        public static List<User> users;
        public static List<Product> products;
        public static List<Order> orders = new List<Order>();
        public static int counter = 0;

        public ConsumerService(IMapper mapper, IConfiguration config, WebAppDbContext dbContext)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;

            if (users == null)
            {
                users = new List<User>();
                List<User> usersDB = _dbContext.Users.ToList();
                foreach (User item in usersDB)
                    users.Add(item);
            }

            if (products == null)
            {
                products = new List<Product>();
                List<Product> productsDB = _dbContext.Products.ToList();
                foreach (Product item in productsDB)
                    products.Add(item);
            }
        }

       
        public List<ProductDto> GetAllProducts()
        {
            return _mapper.Map<List<ProductDto>>(_dbContext.Products.ToList());
        }

        public bool AddProductToCart(AddToCartModelDto model)
        {
            //User user = _dbContext.Users.Find(model.email);
            User user = users.First(x => x.Email == model.email);
            if (user == null)
                return false;

            Product product = _dbContext.Products.Find(model.ProductId);
            if (product == null)
                return false;

            lock (lockObject)
            {
                if (((Consumer)user).Products == null)
                    ((Consumer)user).Products = new List<Product>();

                for (int i = 0; i < model.Quantity; i++)
                {
                    //ConsumerProduct consumerProduct = new ConsumerProduct();
                   // consumerProduct.Consumer = (Consumer)user;
                    //consumerProduct.Product = product;

                    ((Consumer)user).Products.Add(product);
                    //_dbContext.SaveChanges();
                }              
            }

            return true;
        }

        public List<ProductDto> GetMyProducts(string email)
        {
            //User user = _dbContext.Users.Find(email);
            User user = users.First(x => x.Email == email);
            if (user == null)
                return null;

            //List<ConsumerProduct> list = _dbContext.ConsumerProduct.ToList();

            /*ICollection<ConsumerProduct> consumerProducts = ((Consumer)user).Products;
            List<Product> temp = new List<Product>();
            foreach (ConsumerProduct item in list)
            {
                if (item.ConsumerID == email)
                    temp.Add(_dbContext.Products.Find(item.ProductID));
            }*/

            return _mapper.Map<List<ProductDto>>(((Consumer)user).Products);
        }

        public bool CancelItem(AddToCartModelDto model)
        {
            //User user = _dbContext.Users.Find(model.email);
            User user = users.First(x => x.Email == model.email);
            if (user == null)
                return false;

            Product product = ((Consumer)user).Products.First(x => x.Id == model.ProductId);
            if (product == null)
                return false;

            lock (lockObject)
            {
                /*List<ConsumerProduct> list = _dbContext.ConsumerProduct.ToList();

                List<Product> temp = new List<Product>();
                foreach (ConsumerProduct item in list)
                {
                    if (item.ConsumerID == model.email && item.ProductID == model.ProductId)
                    {
                        _dbContext.ConsumerProduct.Remove(item);
                        break;
                    }
                }*/

                ((Consumer)user).Products.Remove(product);
                //_dbContext.SaveChanges();
            }
            return true;
        }

        public bool MakeOrder(OrderDto order)
        {
            //User user = _dbContext.Users.Find(order.Email);
            User user = users.First(x => x.Email == order.Email);
            if (user == null)
                return false;

            lock (lockObject)
            {
                order.Id = counter;
                counter++;
                orders.Add(_mapper.Map<Order>(order));              

                //_dbContext.Orders.Add(_mapper.Map<Order>(order));
                //_dbContext.SaveChanges();

                ((Consumer)user).CurrentOrder_Con = orders.First(x => x.Id == order.Id);
                //((Consumer)user).CurrentOrder_Con = _dbContext.Orders.Find(order.Id);
                ((Consumer)user).Products = null;
                //_dbContext.SaveChanges();
            }
            return true;
        }

        public OrderDto GetCurrentOrder(string email)
        {
            OrderDto order = new OrderDto();
            order.Id = -1;

            //User user = _dbContext.Users.Find(email);
            User user = users.First(x => x.Email == email);
            if (user == null)
                return null;

            if (((Consumer)user).CurrentOrder_Con == null)
                return order;
            else
                return _mapper.Map<OrderDto>(((Consumer)user).CurrentOrder_Con);
        }

        public StopWatchDto GetTime(int id)
        {
            StopWatchDto stopWatchDto = new StopWatchDto();
            stopWatchDto.Minutes = DelivererService.Array[id].Item1;
            stopWatchDto.Seconds = DelivererService.Array[id].Item2;

            return stopWatchDto;
        }

        public List<OrderDto> GetPreviousOrders(string email)
        {
            List<OrderDto> retList = new List<OrderDto>();
            foreach (Order item in ConsumerService.orders.ToList())
            {
                if (item.Status == "finished")
                    retList.Add(_mapper.Map<OrderDto>(item));
            }

            return retList;
        }
    }
}
