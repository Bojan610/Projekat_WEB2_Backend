using UserAdminAPI.DTO;

namespace UserAdminAPI.Interfaces
{
    public interface IUserService
    {
        TokenDto Login(LogInUserDto dto);

        bool CreateUser(CreateUserDto newUser);
        DisplayUserDto GetUserByEmail(string email);

        bool UpdateUser(UpdateUserDto updateUserDto);

        TokenDto SocialLogin(SocialLoginDto model);
        RetStringDto VerifyCheckDeliverer(string email);
    }
}
