using System;
using System.Collections.Generic;
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
using DigiStore.Domain.IRepositories.UserRole;
using DigiStore.Domain.ViewModels.Account;
using DigiStore.Domain.ViewModels.User;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Implementations
{
    public class UserService : IUserService
    {
        #region Field
        private readonly IUserRepository _userRepository;
        private readonly IPasswordHelper _passwordHelper;
        private readonly IUserRoleRepository _userRoleRepository;
        #endregion

        #region Constructor
        public UserService(IUserRepository userRepository, IPasswordHelper passwordHelper, IUserRoleRepository userRoleRepository)
        {
            _userRepository = userRepository;
            _passwordHelper = passwordHelper;
            _userRoleRepository = userRoleRepository;
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
                ActiveCode = Generators.Generators.GeneratorsUniqueCode(),

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
                if (user.IsDelete) return LoginResult.DeletedAccount;
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
        public async Task<EditUserProfileResult> EditUserProfile(EditUserProfileViewModel editUserProfile, IFormFile UserAvatar, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return EditUserProfileResult.NotFound;
            if (!user.IsActive) return EditUserProfileResult.NotActive;
            if (user.IsBlock) return EditUserProfileResult.UserIsBlock;
            user.FirstName = editUserProfile.FirstName.SanitizeText();
            user.LastName = editUserProfile.LastName.SanitizeText();
            user.ModifiedDate = DateTime.Now;
            try
            {
                if (UserAvatar != null)
                {
                    if (UserAvatar.IsImage())
                    {
                        var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(UserAvatar.FileName);
                        var res = UserAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100, PathExtension.UserAvatarThumbServer, user.UserAvatar);
                        if (res)
                        {
                            user.UserAvatar = imageName;

                        }
                        else
                        {
                            return EditUserProfileResult.NotIsIamage;
                        }
                    }
                    else
                    {
                        return EditUserProfileResult.NotIsIamage;
                    }
                }

                _userRepository.EditUser(user);
                await _userRepository.Save();
                return EditUserProfileResult.Success;
            }
            catch
            {
                return EditUserProfileResult.Error;
            }
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

        #region FilterUser
        public async Task<FilterUserViewModel> FilterUser(FilterUserViewModel filterUser)
        {
            return await _userRepository.FilterUserTask(filterUser);
        }
        #endregion

        #region DeleteUser
        public async Task<bool> DeleteUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return false;
            try
            {
                user.IsDelete = true;
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region RestoreUser
        public async Task<bool> RestoreUser(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return false;
            try
            {
                user.IsDelete = false;
                _userRepository.EditUser(user);
                await _userRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region CreateUser
        public async Task<CreateUserResult> CreateUser(CreateUserViewModel createUser, List<int> rolesId, IFormFile userAvatar)
        {
            if (await _userRepository.IsExistsUserByEmail(FixedText.FixEmail(createUser.Email)))
                return CreateUserResult.ExistsEmail;
            if (await _userRepository.IsExistsUserByUserName(createUser.UserName))
                return CreateUserResult.ExistUserName;
            if (await _userRepository.IsExistsUserByMobile(createUser.Mobile))
                return CreateUserResult.ExistMobile;

            var user = new User()
            {
                UserName = createUser.UserName.SanitizeText(),
                Mobile = createUser.Mobile.SanitizeText(),
                Email = createUser.Email.SanitizeText(),
                PassWord = _passwordHelper.EncodePasswordMd5(createUser.PassWord.SanitizeText()),
                ActiveCode = Generators.Generators.GeneratorsUniqueCode(),
                IsActive = true

            };
            try
            {
                if (userAvatar != null && userAvatar.IsImage())
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(userAvatar.FileName);
                    var res = userAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100,
                        PathExtension.UserAvatarThumbServer);
                    if (res)
                    {
                        user.UserAvatar = imageName;
                    }
                }
                await _userRepository.AddUser(user);
                await _userRepository.Save();
                await _userRoleRepository.AddRoleToUser(rolesId, user.UserId);
                await _userRoleRepository.Save();
                return CreateUserResult.Success;
            }
            catch
            {
                return CreateUserResult.Error;
            }
        }
        #endregion

        #region UserInfoForEdit
        public async Task<EditUserForAdminViewModel> UserInfoForEdit(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;
            return new EditUserForAdminViewModel()
            {
                UserId = userId,
                FirstName = user.FirstName,
                LastName = user.LastName,
                UserName = user.UserName,
                UserImage = user.UserAvatar,
                IsActive = user.IsActive,
                IsBlock = user.IsBlock
            };
        }
        #endregion

        #region EditUserForAdmin
        public async Task<EditUserResult> EditUserForAdmin(EditUserForAdminViewModel editUser, List<int> rolesId, IFormFile userAvatar)
        {
            var user = await _userRepository.GetUserById(editUser.UserId);
            if (user == null) return EditUserResult.NotFound;
            user.FirstName = editUser.FirstName.SanitizeText();
            user.LastName = editUser.LastName.SanitizeText();
            user.IsActive = editUser.IsActive;
            user.IsBlock = editUser.IsBlock;

            if (!string.IsNullOrEmpty(editUser.PassWord))
            {
                user.PassWord = _passwordHelper.EncodePasswordMd5(editUser.PassWord.SanitizeText());
            }
            try
            {
                if (userAvatar != null && userAvatar.IsImage())
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(userAvatar.FileName);
                    var res = userAvatar.AddImageToServer(imageName, PathExtension.UserAvatarOriginServer, 100, 100,
                        PathExtension.UserAvatarThumbServer, user.UserAvatar);
                    if (res)
                    {
                        user.UserAvatar = imageName;
                    }
                }
                _userRepository.EditUser(user);
                await _userRepository.Save();
                _userRoleRepository.DeleteUserRoles(editUser.UserId);
                await _userRoleRepository.Save();
                await _userRoleRepository.AddRoleToUser(rolesId, user.UserId);
                await _userRoleRepository.Save();
                return EditUserResult.Success;
            }
            catch
            {
                return EditUserResult.Error;
            }

        }
        #endregion

        #region GetUserByUserId
        public async Task<User> GetUserByUserId(int userId)
        {
            return await _userRepository.GetUserById(userId);
        }
        #endregion

        #region GetUserDetialByProductId
        public async Task<UserDetialViewModel> GetUserDetialByProductId(int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user == null) return null;
            return new UserDetialViewModel()
            {
                UserName = user.UserName,
                Email = user.Email,
                IsActive = user.IsActive,
                Mobile = user.Mobile,
                IsBlock = user.IsBlock,
                RegisterDate = user.CreateDate,
                UserAvatar = user.UserAvatar,
                FullName = user.FullName,
                IsDelete = user.IsDelete
            };
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
