using ChatApp_Api.DTOS.Auth;
using ChatApp_Api.Helpers;
using ChatApp_Api.Models;
using Microsoft.AspNetCore.Identity;

namespace ChatApp_Api.Interfaces
{
    public interface IAuthService
    {
        Task<UserReturnDto> UserRegister(UserRegisterDto user);
        Task<UserReturnDto> UserLogin(UserLoginDto user);
    }
}
