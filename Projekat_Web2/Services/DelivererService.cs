using AutoMapper;
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
    public class DelivererService : IDelivererService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly WebAppDbContext _dbContext;
        private readonly object lockObject = new object();

        private static Tuple<double, double>[] array = new Tuple<double, double>[100];

        public static Tuple<double, double>[] Array { get => array; set => array = value; }

        //private int counter = 0;
        //private bool[] busy = new bool[100];


        public DelivererService(IMapper mapper, IConfiguration config, WebAppDbContext dbContext)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;

        }

        public RetStringDto VerifyCheck(string email)
        {
            RetStringDto retStringDto = new RetStringDto();
            User user = _dbContext.Users.Find(email);
            if (user == null)
                return null;

            retStringDto.RetValue = ((Deliverer)user).Verified.ToString();

            return retStringDto;
        }

        public List<OrderDto> GetOrders()
        {
            List<OrderDto> retList = new List<OrderDto>();
            foreach (Order item in ConsumerService.orders.ToList())
            {
                if (item.Status == "waiting")
                    retList.Add(_mapper.Map<OrderDto>(item));
            }

            return retList;
        }

        public bool PickUpOrder(PickupOrderDto order)
        {
            //User user = _dbContext.Users.Find(order.Email);
            User user = ConsumerService.users.First(x => x.Email == order.EmailDeliverer);
            if (user == null)
                return false;

            lock (lockObject)
            {
                Order or = ConsumerService.orders.First(x => x.Id == order.Id);
                if (or == null)
                    return false;

                or.Status = "picked up";

                //ConsumerService.orders.Remove(_mapper.Map<Order>(order));
                //_dbContext.Orders.Add(_mapper.Map<Order>(order));
                //_dbContext.SaveChanges();
             
                ((Deliverer)user).CurrentOrder_Del = or;
             
                Tuple<double, double> t1 = new Tuple<double, double>(0, 0);
                Array[order.Id] = t1;
                       
                Task t = new Task(delegate { StartStopWatch(order.Id); });
                t.Start();
                            
                //_dbContext.SaveChanges();
            }
            return true;
        }

        public OrderDto GetCurrentOrder(string email)
        {
            OrderDto order = new OrderDto();
            order.Id = -1;

            //User user = _dbContext.Users.Find(order.Email);
            User user = ConsumerService.users.First(x => x.Email == email);
            if (user == null)
                return null;

            if (((Deliverer)user).CurrentOrder_Del == null)
                return order;
            else
                return _mapper.Map<OrderDto>(((Deliverer)user).CurrentOrder_Del);
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
            Order or = ConsumerService.orders.First(x => x.Id == i);
            or.Status = "finished";

            foreach (User item in ConsumerService.users)
            {
                if (item.UserKind == "deliverer")
                {
                    if (((Deliverer)item).CurrentOrder_Del.Id == i)
                    {
                        ((Deliverer)item).CurrentOrder_Del = null;
                        break;
                    }
                }
            }
            foreach (User item in ConsumerService.users)
            {
                if (item.UserKind == "consumer")
                {
                    if (((Consumer)item).CurrentOrder_Con.Id == i)
                    {
                        ((Consumer)item).CurrentOrder_Con = null;
                        break;
                    }
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
