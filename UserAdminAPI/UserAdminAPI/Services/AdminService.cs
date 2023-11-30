using AutoMapper;
using Microsoft.Extensions.Configuration;
using System.Collections.Generic;
using System.Net.Mail;
using System;
using UserAdminAPI.DTO;
using UserAdminAPI.Interfaces;
using UserAdminAPI.Models;
using UserAdminAPI.Infrastructure;
using System.Linq;
using MimeKit;
using MailKit.Net.Smtp;

namespace UserAdminAPI.Services
{
    public class AdminService : IAdminService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly UsersDbContext _dbContext;
        //private readonly EmailConfiguration _emailConfig;
        private readonly object lockObject = new object();

        public AdminService(IMapper mapper, IConfiguration config, UsersDbContext dbContext)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;
            //_emailConfig = emailConfig;
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

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Food delivery service", "bojanbrdarevic@gmail.com"));
            //message.To.Add(MailboxAddress.Parse(email));
            message.To.Add(MailboxAddress.Parse("bojanbrdarevic@gmail.com"));

            message.Subject = "Registration request";
            message.Body = new TextPart("plain")
            {
                Text = "Dear," +
                "\n\nYour registration request is accepted. Thank you for using our food service." +
                "\n\n Best regards," +
                "\n Admin team"
            };
            SendEmail(message);

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

            MimeMessage message = new MimeMessage();
            message.From.Add(new MailboxAddress("Food delivery service", "bojanbrdarevic@gmail.com"));
            //message.To.Add(MailboxAddress.Parse(email));
            message.To.Add(MailboxAddress.Parse("bojanbrdarevic@gmail.com"));

            message.Subject = "Registration request";
            message.Body = new TextPart("plain")
            {
                Text = "Dear," +
                "\n\nUnfortunately your registration request is denied." +
                "\n\n Best regards," +
                "\n Admin team"
            };
            SendEmail(message);

            return true;
        }

        public void SendEmail(MimeMessage message)
        {
            MailKit.Net.Smtp.SmtpClient smtpClient = new MailKit.Net.Smtp.SmtpClient();
            try
            {
                smtpClient.Connect("smtp.gmail.com", 465, true);
                smtpClient.Authenticate("webtestwebtest76", "gtlddqlodtqnndku");
                smtpClient.Send(message);
            }
            catch (Exception ex)
            {
                Console.WriteLine(ex.Message);
            }
            finally
            {
                smtpClient.Disconnect(true);
                smtpClient.Dispose();
            }         
        }      
    }
}
