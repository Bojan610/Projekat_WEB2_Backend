using AutoMapper;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;
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
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly WebAppDbContext _dbContext;
        private readonly EmailConfiguration _emailConfig;
        private readonly object lockObject = new object();

        public AdminService(IMapper mapper, IConfiguration config, WebAppDbContext dbContext, EmailConfiguration emailConfig)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;
            _emailConfig = emailConfig;
        }


        public List<DisplayDelivererDto> GetProcessing()
        {
            List<User> users = _dbContext.Users.ToList();
            List<Deliverer> deliverers = new List<Deliverer>();
            foreach (User item in users)
            {
                if (item.UserKind == "deliverer" && (((Deliverer)item).Verified == "processing"))
                {
                    deliverers.Add((Deliverer)item);
                }
            }

            return _mapper.Map<List<DisplayDelivererDto>>(deliverers);
        }

        public List<DisplayDelivererDto> GetDenied()
        {
            List<User> users = _dbContext.Users.ToList();
            List<Deliverer> deliverers = new List<Deliverer>();
            foreach (User item in users)
            {
                if (item.UserKind == "deliverer" && ((Deliverer)item).Verified == "denied")
                    deliverers.Add((Deliverer)item);
            }

            return _mapper.Map<List<DisplayDelivererDto>>(deliverers);
        }

        public List<DisplayDelivererDto> GetAccepted()
        {
            List<User> users = _dbContext.Users.ToList();
            List<Deliverer> deliverers = new List<Deliverer>();
            foreach (User item in users)
            {
                if (item.UserKind == "deliverer" && ((Deliverer)item).Verified == "accepted")
                    deliverers.Add((Deliverer)item);
            }

            return _mapper.Map<List<DisplayDelivererDto>>(deliverers);
        }

        public bool AcceptDeliverer(string email)
        {
            User user = _dbContext.Users.Find(email);
            if (user == null)
                return false;

            lock (lockObject)
            {
                ((Deliverer)user).Verified = "accepted";
                _dbContext.SaveChanges();
            }

            var message = new Message(new string[] { "bojanbrdarevic@gmail.com" }, "Registration request", "Your registration is accepted.");
            //SendEmail(message);

            return true;
        }

        public bool DeclineDeliverer(string email)
        {
            User user = _dbContext.Users.Find(email);
            if (user == null)
                return false;

            lock (lockObject)
            {
                ((Deliverer)user).Verified = "denied";
                _dbContext.SaveChanges();
            }

            var message = new Message(new string[] { "bojanbrdarevic@gmail.com" }, "Registration request", "Your registration is denied.");
            //SendEmail(message);

            return true;
        }

        public List<ProductDto> GetAllProducts()
        {
            return _mapper.Map<List<ProductDto>>(_dbContext.Products.ToList());
        }

        public bool AddNewProduct(ProductDto product)
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

        public void SendEmail(Message message)
        {
            var emailMessage = CreateEmailMessage(message);
            Send(emailMessage);
        }

        private MimeMessage CreateEmailMessage(Message message)
        {
            var emailMessage = new MimeMessage();
            emailMessage.From.Add(new MailboxAddress("email", _emailConfig.From));
            emailMessage.To.AddRange(message.To);
            emailMessage.Subject = message.Subject;
            emailMessage.Body = new TextPart(MimeKit.Text.TextFormat.Text) { Text = message.Content };
            return emailMessage;
        }

        private void Send(MimeMessage mailMessage)
        {
            using (var client = new SmtpClient())
            {
                try
                {
                    client.Connect(_emailConfig.SmtpServer, _emailConfig.Port, true);
                    client.AuthenticationMechanisms.Remove("XOAUTH2");
                    client.Authenticate(_emailConfig.Username, _emailConfig.Password);
                    client.Send(mailMessage);
                }
                catch (Exception e)
                {
                    
                    //log an error message or throw an exception or both.
                    throw;
                }
                finally
                {
                    client.Disconnect(true);
                    client.Dispose();
                }
            }

        }

        public List<OrderDto> GetOrders()
        {
            return _mapper.Map<List<OrderDto>>(ConsumerService.orders.ToList());
        }
    }
}
