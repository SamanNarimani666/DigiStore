using System.Threading.Tasks;
using DigiMarket.Application.ViewModels.Account;
using DigiStore.Application.Convertors;
using DigiStore.Application.Security;
using DigiStore.Application.Security.PassWordHashing;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Application.Utils;
using DigiStore.Application.ViewModels.Account;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.ViewModels.Account;

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
            if (user==null || user.IsActive) return false;
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
            return  new InformationUserForSidebarViewModel
            {
                Mobile = user.Mobile,
                UserAvatar = user.UserAvatar,
                Wallet = 16000
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
