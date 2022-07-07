using Projekat_Web2.DTO;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;

namespace Projekat_Web2.Interfaces
{
    public interface IUserService
    {
        TokenDto Login(LogInUserDto dto);

        bool CreateUser(CreateUserDto newUser);
        DisplayUserDto GetUserByEmail(string email);

        bool UpdateUser(UpdateUserDto updateUserDto);
    }
}
