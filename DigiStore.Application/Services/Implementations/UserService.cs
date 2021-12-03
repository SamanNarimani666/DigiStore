using System;
using System.IO;
using System.Threading.Tasks;
using DigiMarket.Application.ViewModels.Account;
using DigiStore.Application.Convertors;
using DigiStore.Application.Extensions;
using DigiStore.Application.Security;
using DigiStore.Application.Security.PassWordHashing;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Application.Utils;
using DigiStore.Application.ViewModels.Account;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.ViewModels.Account;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Field
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        #endregion

        #region Constructor
        public UserService(IUserRepository repository, IPasswordHelper passwordHelper)
        {
            _userRepository = repository;
            _passwordHelper = passwordHelper;
        }
        #endregion

        #region RegisterUser
        public async Task<RegisterResult> RegisterUser(RegisterViewModel registerUser)
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
                ActiveCode = Generators.Generators.GeneratorsUniqueCode()
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
        #endregion

        #region GetUserByEmail
        public async Task<User> GetUserByEmail(string email)
        {
            return await _userRepository.GetUserByEmail(email);
        }
        #endregion

        #region ActiveUserByActiveCode
        public async Task<bool> ActiveUserByActiveCode(string activeCode)
        {
            var user = await _userRepository.GetUserByActiveCode(activeCode);
            if (user == null || user.IsActive) return false;
            if (string.IsNullOrEmpty(activeCode)) return false;
            user.IsActive = true;
            user.ActiveCode = Generators.Generators.GeneratorsUniqueCode();
            _userRepository.EditUser(user);
            await _userRepository.Save();
            return true;
        }
        #endregion

        #region LoginUser
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
        #endregion

        #region GetUserByEmailOrPhone
        public async Task<User> GetUserByEmailOrPhone(string emailOrMobile)
        {
            if (emailOrMobile.Contains("@gmail.com") || emailOrMobile.Contains("@yahoo.com"))
                return await _userRepository.GetUserByEmail(FixedText.FixEmail(emailOrMobile.SanitizeText()));
            else if (emailOrMobile.StartsWith("09"))
                return await _userRepository.GetUserByMobile(emailOrMobile);
            return null;
        }
        #endregion

        #region ForgotPassWordUser
        public async Task<ForgotPassResult> ForgotPassWordUser(ForgotPassViewModel forgotPass)
        {
            var user = await _userRepository.GetUserByEmail(FixedText.FixEmail(forgotPass.Email.SanitizeText()));
            if (user == null) return ForgotPassResult.NotFount;
            if (user.IsBlock) return ForgotPassResult.UserIsBlock;
            if (!user.IsActive) return ForgotPassResult.NotActive;
            return ForgotPassResult.FindUser;
        }

        public async Task<bool> IsExistsUserByActiveCode(string activeCode)
        {
            return await _userRepository.IsExistsUserByActiveCode(activeCode);
        }
        #endregion

        #region ResetPsssWordUser
        public async Task<ResetPsssWordResult> ResetPsssWordUser(ResetPsssWordViewModel resetPsssWord)
        {
            var user = await _userRepository.GetUserByActiveCode(resetPsssWord.ActiveCode);
            if (user == null) return ResetPsssWordResult.NotFound;

            user.PassWord = _passwordHelper.EncodePasswordMd5(resetPsssWord.Password);
            user.ActiveCode = Generators.Generators.GeneratorsUniqueCode();
            _userRepository.EditUser(user);
            await _userRepository.Save();
            return ResetPsssWordResult.Success;
        }
        #endregion

        #region GetInformationUserById
        public async Task<InformationUserViewModel> GetInformationUserById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return new InformationUserViewModel()
            {
                Email = user.Email,
                Mobile = user.Mobile,
                FullName = user.FullName,

                CreateData = user.CreateDate.ToStringShamsiDate()
            };
        }
        #endregion

        #region GetInformationUserForSidebarById
        public async Task<InformationUserForSidebarViewModel> GetInformationUserForSidebarById(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return new InformationUserForSidebarViewModel
            {
                Mobile = user.Mobile,
                UserAvatar = user.UserAvatar,
                Wallet = 16000
            };
        }
        #endregion

        #region InfoUserForEditProfile
        public async Task<EditUserProfileViewModel> InfoUserForEditProfile(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            return new EditUserProfileViewModel
            {
                FirstName = user.FirstName,
                LastName = user.LastName,
                AvatarName = user.UserAvatar
            };
        }
        #endregion

        #region EditUserProfile
        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileViewModel editUserProfile, IFormFile UserAvatar,int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return EditUserProfileResult.NotFound;
            if (!user.IsActive) return EditUserProfileResult.NotActive;
            if (user.IsBlock) return EditUserProfileResult.UserIsBlock;
            user.FirstName = editUserProfile.FirstName.SanitizeText();
            user.LastName = editUserProfile.LastName.SanitizeText();
            user.ModifiedDate = DateTime.Now;
            if (UserAvatar != null && UserAvatar.IsImage())
            {
                var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(UserAvatar.FileName);
                UserAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100, PathExtension.UserAvatarThumbServer, user.UserAvatar);
                user.UserAvatar = imageName;
            }
            _userRepository.EditUser(user);
            await _userRepository.Save();
            return EditUserProfileResult.Success;
        }
        #endregion

        #region ChangePassWord
        public async Task<ChangePasswordResult> ChangePassWord(ChangePasswordViewModel changePassword, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return ChangePasswordResult.NotFound;
            if (user.PassWord != _passwordHelper.EncodePasswordMd5(changePassword.CurrentPassword.SanitizeText()))
                return ChangePasswordResult.UnCurrentPassword;
            if (user.PassWord == _passwordHelper.EncodePasswordMd5(changePassword.NewPassword.SanitizeText()))
                return ChangePasswordResult.Error;
            user.PassWord = _passwordHelper.EncodePasswordMd5(changePassword.NewPassword.SanitizeText());
            _userRepository.EditUser(user);
           await _userRepository.Save();
            return ChangePasswordResult.Success;
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _userRepository.DisposeAsync();
        }
        #endregion
    }
}
