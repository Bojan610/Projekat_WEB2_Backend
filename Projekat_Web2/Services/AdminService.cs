using AutoMapper;
using Microsoft.Extensions.Configuration;
using Projekat_Web2.DTO;
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

        public AdminService(IMapper mapper, IConfiguration config)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
        }

        private List<User> users = new List<User>()
        {
            new Admin
            {
                Email = "pedja@gmail.com",
                Username = "pedja",
                Name = "Predrag",
                LastName = "Glavas",
                Birth = DateTime.Today.Date,
                Address = "Neka adresa",
                UserKind = "admin",
                Password = "$2a$11$L.fb./NAUzUTNLGFJiv8quleGSjDb.30RCG2BKYjxp6GNtGIT5/ji" //1234
            },
              new Deliverer
            {
                  Email = "tanja@gmail.com",
                Username = "tanja",
                Name = "Tanja",
                LastName = "Radojcic",
                 Birth = DateTime.Today.Date,
                  Address = "Neka adresa",
                UserKind = "deliverer",
                Password = "$2a$11$L.fb./NAUzUTNLGFJiv8quleGSjDb.30RCG2BKYjxp6GNtGIT5/ji", //1234
              
            },
                new Consumer
            {
                Email = "pera@gmail.com",
                Username = "pera",
                Name = "Petar",
                LastName = "Glavas",
                 Birth = DateTime.Today.Date,
                 Address = "Neka adresa",
                UserKind = "consumer",
                Password = "$2a$11$L.fb./NAUzUTNLGFJiv8quleGSjDb.30RCG2BKYjxp6GNtGIT5/ji" //1234
            }
        };

        public List<DisplayDelivererDto> GetProcessing()
        {
            List<Deliverer> deliverers = new List<Deliverer>();
            foreach (User item in users)
            {
                if (item is Deliverer && (((Deliverer)item).Verified == null || ((Deliverer)item).Verified == "processing"))
                {
                    ((Deliverer)item).Verified = "processing";
                    deliverers.Add((Deliverer)item);
                }
            }

            return _mapper.Map<List<DisplayDelivererDto>>(deliverers);
        }

        public List<DisplayDelivererDto> GetDenied()
        {
            List<Deliverer> deliverers = new List<Deliverer>();
            foreach (User item in users)
            {
                if (item is Deliverer && ((Deliverer)item).Verified == "denied")
                    deliverers.Add((Deliverer)item);
            }

            return _mapper.Map<List<DisplayDelivererDto>>(deliverers);
        }

        public List<DisplayDelivererDto> GetAccepted()
        {
            List<Deliverer> deliverers = new List<Deliverer>();
            foreach (User item in users)
            {
                if (item is Deliverer && ((Deliverer)item).Verified == "accepted")
                    deliverers.Add((Deliverer)item);
            }

            return _mapper.Map<List<DisplayDelivererDto>>(deliverers);
        }

        public bool AcceptDeliverer(string email)
        {
            User user = users.First(x => x.Email == email);

            ((Deliverer)user).Verified = "accepted";
            return true;
        }

        public bool DeclineDeliverer(string email)
        {
            User user = users.First(x => x.Email == email);
          
            ((Deliverer)user).Verified = "denied";
            return true;
        }

    }
}
