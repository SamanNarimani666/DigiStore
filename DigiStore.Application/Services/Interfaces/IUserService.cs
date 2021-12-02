﻿using System;
using System.Threading.Tasks;
using DigiMarket.Application.ViewModels.Account;
using DigiStore.Application.ViewModels.Account;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Account;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IUserService:IAsyncDisposable
    {
        Task<RegisterResult> RegisterUser(RegisterViewModel registerUser);
        Task<User> GetUserByEmail(string email);
        Task<bool> ActiveUserByActiveCode(string activeCode);
        Task<LoginResult> LoginUser(LoginViewModel login);
        Task<User> GetUserByEmailOrPhone(string emailOrMobile);
        Task<ForgotPassResult> ForgotPassWordUser(ForgotPassViewModel forgotPass);
        Task<bool> IsExistsUserByActiveCode(string activeCode);
        Task<ResetPsssWordResult> ResetPsssWordUser(ResetPsssWordViewModel resetPsssWord);
        Task<InformationUserViewModel> GetInformationUserById(int userId);
        Task<InformationUserForSidebarViewModel> GetInformationUserForSidebarById(int userId);
    }
}