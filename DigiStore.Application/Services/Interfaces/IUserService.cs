using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Account;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IUserService:IAsyncDisposable
    {
        Task<RegisterResult> RegisterUser(RegisterUserViewModel registerUser);
        Task<User> GetUserByEmail(string email);
        Task<bool> ActiveUserByActiveCode(string activeCode);
        Task<LoginResult> LoginUser(LoginViewModel login);
        Task<User> GetUserByEmailOrPhone(string emailOrMobile);
    }
}
