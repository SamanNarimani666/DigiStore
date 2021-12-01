using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Application.Convertors;
using DigiStore.Application.Security;
using DigiStore.Application.Security.PassWordHashing;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.ViewModels.Account;

namespace DigiStore.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;

        public UserService(IUserRepository repository, IPasswordHelper passwordHelper)
        {
            _userRepository = repository;
            _passwordHelper = passwordHelper;
        }
        public async Task<RegisterResult> RegisterUser(RegisterUserViewModel registerUser)
        {
            if (await _userRepository.IsExistsUserByEmail(FixedText.FixEmail(registerUser.Email)))
                return RegisterResult.ExistsEmail;
            if (await _userRepository.IsExistsUserByUserName(registerUser.UserName))
                return RegisterResult.ExistUserName;
            if (await _userRepository.IsExistsUserByMobile(registerUser.Mobile))
                return RegisterResult.ExistMobile;
            var user = new User()
            {
                UserName = registerUser.UserName.SanitizeText(),
                Mobile = registerUser.Mobile.SanitizeText(),
                Email = registerUser.Email.SanitizeText(),
                PassWord = _passwordHelper.EncodePasswordMd5(registerUser.PassWord.SanitizeText()),
                ActiveCode = Generators.Generators.GeneratorsUniqueCode(),
                UserAvatar = "Default.jpg"
            };
            try
            {
                await _userRepository.AddUser(user);
                await _userRepository.Save();
                return RegisterResult.Success;
            }
            catch
            {
                return RegisterResult.Failed;
            }

        }

        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }

        public async Task<bool> ActiveUserByActiveCode(string activeCode)
        {
            if (!await _userRepository.IsExistsUserByActiveCode(activeCode)) return false;
            if (string.IsNullOrEmpty(activeCode)) return false;
            var user = await _userRepository.GetUserByActiveCode(activeCode);
            user.IsActive = true;
            user.ActiveCode = Generators.Generators.GeneratorsUniqueCode();
            _userRepository.EditUser(user);
            await _userRepository.Save();
            return true;
        }

        public async Task<LoginResult> LoginUser(LoginViewModel login)
        {
            if (login.EmailOrMobiel.Contains("@gmail.com") || login.EmailOrMobiel.Contains("@yahoo.com"))
            {
                if (!await _userRepository.IsExistsUserByEmail(FixedText.FixEmail(login.EmailOrMobiel.SanitizeText()))) return LoginResult.NotFound;
                var user = await _userRepository.GetUserByEmail(login.EmailOrMobiel.SanitizeText());
                if (!user.IsActive) return LoginResult.NotActive;
                if (user.IsBlock) return LoginResult.UserIsBlock;
                if (user.PassWord != _passwordHelper.EncodePasswordMd5(login.PassWord.SanitizeText()))
                    return LoginResult.NotFound;
                return LoginResult.Success;
            }

            else if (login.EmailOrMobiel.StartsWith("09"))
            {
                if (!await _userRepository.IsExistsUserByMobile(FixedText.FixEmail(login.EmailOrMobiel.SanitizeText()))) return LoginResult.NotFound;
                var user = await _userRepository.GetUserByMobile(login.EmailOrMobiel.SanitizeText());
                if (!user.IsActive) return LoginResult.NotActive;
                if (user.IsBlock) return LoginResult.UserIsBlock;
                if (user.PassWord != _passwordHelper.EncodePasswordMd5(login.PassWord.SanitizeText()))
                    return LoginResult.NotFound;
                return LoginResult.Success;
            }

            return LoginResult.NotFound;
        }

        public async Task<User> GetUserByEmailOrPhone(string emailOrMobile)
        {
            if (emailOrMobile.Contains("@gmail.com") || emailOrMobile.Contains("@yahoo.com"))
            return await _userRepository.GetUserByEmail(FixedText.FixEmail(emailOrMobile.SanitizeText()));
            else if (emailOrMobile.StartsWith("09"))
                return await _userRepository.GetUserByMobile(emailOrMobile);
            return null;
        }

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
        }
        #endregion
    }
}
