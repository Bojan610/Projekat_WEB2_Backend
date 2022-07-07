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
    public class DelivererService : IDelivererService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;

        public DelivererService(IMapper mapper, IConfiguration config)
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

        public RetStringDto VerifyCheck(string email)
        {
            RetStringDto retStringDto = new RetStringDto();
            User user = users.First(x => x.Email == email);

            if (((Deliverer)user).Verified == null)
                retStringDto.RetValue = "processing";
            else
                retStringDto.RetValue = ((Deliverer)user).Verified.ToString();

            return retStringDto;
        }
    }
}
