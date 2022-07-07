using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Projekat_Web2.DTO;
using Projekat_Web2.Interfaces;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Projekat_Web2.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;

        public UserService(IMapper mapper, IConfiguration config)
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
                Password = "$2a$11$L.fb./NAUzUTNLGFJiv8quleGSjDb.30RCG2BKYjxp6GNtGIT5/ji" //1234
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


        public TokenDto Login(LogInUserDto dto)
        {
            User user = null;
            foreach (User item in users)
            {
                if (item.Email == dto.Email)
                {
                    user = item;
                    break;
                }
            }

            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))//Uporedjujemo hes pasvorda iz baze i unetog pasvorda
            {
               

                //Kreiramo kredencijale za potpisivanje tokena. Token mora biti potpisan privatnim kljucem
                //kako bi se sprecile njegove neovlascene izmene
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44342", //url servera koji je izdao token
                   
                    expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                    signingCredentials: signinCredentials //kredencijali za potpis
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                return new TokenDto { Token = tokenString, UserType = user.UserKind.ToString() };
            }
            else
            {
                return null;
            }
        }

        public bool CreateUser(CreateUserDto newUser)
        {
            if (newUser.Password == newUser.PasswordConfirm)
            {
                foreach (User item in users)
                {
                    if (item.Email == newUser.Email)
                        return false;
                }

                newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                if (newUser.UserKind == "admin")
                    users.Add(_mapper.Map<Admin>(newUser));
                else if (newUser.UserKind == "deliverer")
                    users.Add(_mapper.Map<Deliverer>(newUser));
                else if (newUser.UserKind == "consumer")
                    users.Add(_mapper.Map<Consumer>(newUser));

                return true;
            }
            else
                return false;
        }

        public DisplayUserDto GetUserByEmail(string email)
        {
            return _mapper.Map<DisplayUserDto>(users.First(x => x.Email == email));
        }

        public bool UpdateUser(UpdateUserDto updateUserDto)
        {
            User user = users.First(x => x.Email == updateUserDto.Email);

            if (updateUserDto.Password == updateUserDto.PasswordConfirm)
            {
                user.Username = updateUserDto.Username;
                user.Name = updateUserDto.Name;
                user.LastName = updateUserDto.LastName;
                user.Birth = updateUserDto.Birth;
                user.Address = updateUserDto.Address;
                user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);

                return true;
            }
            else
                return false;
        }

       
    }
}
