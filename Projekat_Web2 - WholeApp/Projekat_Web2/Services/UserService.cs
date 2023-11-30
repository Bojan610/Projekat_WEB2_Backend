using AutoMapper;
using Microsoft.Extensions.Configuration;
using Microsoft.IdentityModel.Tokens;
using Newtonsoft.Json;
using Projekat_Web2.DTO;
using Projekat_Web2.Infrastructure;
using Projekat_Web2.Interfaces;
using Projekat_Web2.Models;
using System;
using System.Collections.Generic;
using System.IdentityModel.Tokens.Jwt;
using System.Linq;
using System.Net.Http;
using System.Security.Claims;
using System.Text;
using System.Threading.Tasks;

namespace Projekat_Web2.Services
{
    public class UserService : IUserService
    {
        private readonly IMapper _mapper;
        private readonly IConfigurationSection _secretKey;
        private readonly WebAppDbContext _dbContext;
        private readonly object lockObject = new object();


        public UserService(IMapper mapper, IConfiguration config, WebAppDbContext dbContext)
        {
            _mapper = mapper;
            _secretKey = config.GetSection("SecretKey");
            _dbContext = dbContext;
        }

        public TokenDto Login(LogInUserDto dto)
        {
            User user = _dbContext.Users.Find(dto.Email);
         
            if (user == null)
                return null;

            if (BCrypt.Net.BCrypt.Verify(dto.Password, user.Password))//Uporedjujemo hes pasvorda iz baze i unetog pasvorda
            {
                List<Claim> claims = new List<Claim>();
                //Mozemo dodati Claimove u token, oni ce biti vidljivi u tokenu i mozemo ih koristiti za autorizaciju
                if (user.UserKind == "admin")
                    claims.Add(new Claim(ClaimTypes.Role, "admin")); //Add user type to claim
                if (user.UserKind == "deliverer")
                    claims.Add(new Claim(ClaimTypes.Role, "deliverer")); //Add user type to claim
                if (user.UserKind == "consumer")
                    claims.Add(new Claim(ClaimTypes.Role, "consumer")); //Add user type to claim
                //mozemo izmisliti i mi neki nas claim
                claims.Add(new Claim("Neki_moj_claim", "imam_ga"));

                //Kreiramo kredencijale za potpisivanje tokena. Token mora biti potpisan privatnim kljucem
                //kako bi se sprecile njegove neovlascene izmene
                SymmetricSecurityKey secretKey = new SymmetricSecurityKey(Encoding.UTF8.GetBytes(_secretKey.Value));
                var signinCredentials = new SigningCredentials(secretKey, SecurityAlgorithms.HmacSha256);
                var tokeOptions = new JwtSecurityToken(
                    issuer: "https://localhost:44342", //url servera koji je izdao token
                    claims: claims, //claimovi
                    expires: DateTime.Now.AddMinutes(20), //vazenje tokena u minutama
                    signingCredentials: signinCredentials //kredencijali za potpis
                );
                string tokenString = new JwtSecurityTokenHandler().WriteToken(tokeOptions);
                //return tokenString;
                return new TokenDto { Token = tokenString, UserType = user.UserKind.ToString() };
            }
            else
            {
                return null;
            }
        }

        public bool CreateUser(CreateUserDto newUser)
        {      
            if (newUser.Email != "" && newUser.Password != "" && newUser.UserKind != "" && newUser.Password == newUser.PasswordConfirm)
            {
                lock (lockObject)
                {
                    List<User> users = _dbContext.Users.ToList();
                    foreach (User item in users)
                    {
                        if (item.Email == newUser.Email)
                            return false;
                    }

                    newUser.Password = BCrypt.Net.BCrypt.HashPassword(newUser.Password);
                    if (newUser.UserKind == "admin")
                    {
                        _dbContext.Users.Add(_mapper.Map<Admin>(newUser));
                        _dbContext.SaveChanges();
                    }
                    else if (newUser.UserKind == "deliverer")
                    {
                        Deliverer deliverer = _mapper.Map<Deliverer>(newUser);
                        deliverer.Verified = "processing";
                        _dbContext.Users.Add(deliverer);
                        _dbContext.SaveChanges();
                    }
                    else if (newUser.UserKind == "consumer")
                    {
                      
                        _dbContext.Users.Add(_mapper.Map<Consumer>(newUser));
                        _dbContext.SaveChanges();
                    }
                    return true;
                }
            }
            else
                return false;
        }

        public DisplayUserDto GetUserByEmail(string email)
        {
            User user = _dbContext.Users.Find(email);

            if (user == null)
                return null;

            return _mapper.Map<DisplayUserDto>(_dbContext.Users.Find(email));
        }

        public bool UpdateUser(UpdateUserDto updateUserDto)
        {
            User user = _dbContext.Users.Find(updateUserDto.Email);

            if (user == null)
                return false;

            if (updateUserDto.Password == updateUserDto.PasswordConfirm)
            {
                lock (lockObject)
                {
                    user.Username = updateUserDto.Username;
                    user.Name = updateUserDto.Name;
                    user.LastName = updateUserDto.LastName;
                    user.Birth = updateUserDto.Birth;
                    user.Address = updateUserDto.Address;
                    user.Password = BCrypt.Net.BCrypt.HashPassword(updateUserDto.Password);

                    _dbContext.SaveChanges();
                    return true;
                }
            }
            else
                return false;
        }

        public TokenDto SocialLogin(SocialLoginDto model)
        {
            User user = _dbContext.Users.Find(model.Email);

            if (user == null)
            {
                CreateUserDto createUserDto = new CreateUserDto();
                createUserDto.Email = model.Email;
                createUserDto.Password = "1234";
                createUserDto.PasswordConfirm = "1234";
                createUserDto.Name = model.FirstName;
                createUserDto.LastName = model.LastName;
                createUserDto.UserKind = "consumer";
                CreateUser(createUserDto);
            }
           
            LogInUserDto logIn = new LogInUserDto();
            logIn.Email = model.Email;
            logIn.Password = "1234";
            TokenDto token = Login(logIn);
            
            return token;
        }

        public RetStringDto VerifyCheckDeliverer(string email)
        {
            RetStringDto retStringDto = new RetStringDto();
            User user = _dbContext.Users.Find(email);
            if (user == null)
                return null;

            retStringDto.RetValue = ((Deliverer)user).Verified.ToString();

            return retStringDto;
        }
    }
}
