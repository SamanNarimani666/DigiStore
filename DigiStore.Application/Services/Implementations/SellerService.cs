using System;
using System.Threading.Tasks;
using DigiStore.Application.Security;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Store;
using DigiStore.Domain.IRepositories.Seller;
using DigiStore.Domain.IRepositories.SellerWallet;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.ViewModels.Common;
using DigiStore.Domain.ViewModels.Seller;

namespace DigiStore.Application.Services.Implementations
{
    public class SellerService : ISellerService
    {
        #region Constructor
        private readonly ISellerRepository _sellerRepository;
        private readonly IUserRepository _userRepository;
        private readonly ISellerWalletRepository _sellerWalletRepository;
        public SellerService(ISellerRepository sellerRepository, IUserRepository userRepository, ISellerWalletRepository sellerWalletRepository)
        {
            _sellerRepository = sellerRepository;
            _userRepository = userRepository;
            _sellerWalletRepository = sellerWalletRepository;
        }
        #endregion

        #region AddNewSellerRequet
        public async Task<RequestSellerResult> AddNewSellerRequet(RequestSellerViewModel requestSeller, int userId)
        {
            var user = await _userRepository.GetUserById(userId);
            if (user.IsBlock) return RequestSellerResult.HasNotPermission;
            var hasUnderPrograssRequest = await _sellerRepository.HasHanderprograssRequestUser(userId);
            if (hasUnderPrograssRequest) return RequestSellerResult.HasUnderProgressRequest;
            try
            {
                var newSeller = new Seller
                {
                    UserId = userId,
                    StoreName = requestSeller.StoreName.SanitizeText(),
                    Phone = requestSeller.Phone.SanitizeText(),
                    Email = requestSeller.Email.SanitizeText(),
                    Address = requestSeller.Address,
                    StoreaceptanceState = (byte)StoreAcceptanceState.UnderProgress,
                    Descriptions = requestSeller.Description.SanitizeText(),
                    Logo = requestSeller.SellerLogo,
                    StoreDoucument = requestSeller.SellerDoucemnt
                };
                await _sellerRepository.AddSeller(newSeller);
                await _userRepository.Save();
                return RequestSellerResult.Success;
            }
            catch
            {
                return RequestSellerResult.Erorr;

            }
        }
        #endregion

        #region FilterSeller
        public async Task<FilterSellerViewModel> FilterSeller(FilterSellerViewModel filterSeller)
        {
            return await _sellerRepository.FilterSeller(filterSeller);
        }
        #endregion

        #region GetEditRequestSellerInfo
        public async Task<EditRequestSellerViewModel> GetEditRequestSellerInfo(int sellerId, int userId)
        {
            var seller = await _sellerRepository.GetSellerById(sellerId);
            if (seller == null || seller.UserId != userId) return null;
            return new EditRequestSellerViewModel
            {
                Email = seller.Email,
                SellerLogo = seller.Logo,
                Phone = seller.Phone,
                Address = seller.Address,
                Description = seller.Descriptions,
                StoreName = seller.StoreName,
                SellerDoucemnt = seller.StoreDoucument,
                SellerID = seller.SellerId
            };

        }
        #endregion

        #region EditSeller
        public async Task<EditRequestSellerResult> EditSellerRequest(EditRequestSellerViewModel editRequestSeller, int userId)
        {
            var seller = await _sellerRepository.GetSellerById(editRequestSeller.SellerID);
            if (seller == null || seller.UserId != userId) return EditRequestSellerResult.NotFound;
            try
            {
                seller.Address = editRequestSeller.Address;
                seller.Descriptions = editRequestSeller.Description;
                seller.Logo = editRequestSeller.SellerLogo;
                seller.StoreName = editRequestSeller.StoreName;
                seller.Phone = editRequestSeller.Phone;
                seller.Email = editRequestSeller.Email;
                seller.StoreDoucument = editRequestSeller.SellerDoucemnt;
                seller.StoreaceptanceState = (byte)StoreAcceptanceState.UnderProgress;
                _sellerRepository.EditSeller(seller);
                await _sellerRepository.Save();
                return EditRequestSellerResult.Success;
            }
            catch
            {
                return EditRequestSellerResult.NotFound;
            }
        }
        #endregion

        #region AcceptSellerRequest
        public async Task<bool> AcceptSellerRequest(int sellerId)
        {
            var seller = await _sellerRepository.GetSellerById(sellerId);
            if (seller == null) return false;
            seller.StoreaceptanceState = (byte)StoreAcceptanceState.Accepted;
            seller.StoreAceptanceStateDescription = "اطلاعات پنل فروشگاهی شما با موفقیت تایید شده است";
            _sellerRepository.EditSeller(seller);
            await _sellerRepository.Save();
            return true;
        }
        #endregion

        #region RejectItem
        public async Task<bool> RejectItem(RejectItemViewModel rejectItem)
        {
            var seller = await _sellerRepository.GetSellerById(rejectItem.Id);
            if (seller == null) return false;
            seller.StoreAceptanceStateDescription = rejectItem.RejectMessage;
            seller.StoreaceptanceState = (byte)StoreAcceptanceState.Rejected;
            _sellerRepository.EditSeller(seller);
            await _sellerRepository.Save();
            return true;
        }
        #endregion

        #region GetLastActiveSellerByUserId
        public async Task<Seller> GetLastActiveSellerByUserId(int userId)
        {
            return await _sellerRepository.GetLastActiveSellerByUserId(userId);
        }
        #endregion

        #region HasUserActiveSellerPanel
        public async Task<bool> HasUserActiveSellerPanel(int userId)
        {
            return await _sellerRepository.HasUserActiveSellerPanel(userId);
        }
        #endregion

        #region GetSellerInfoByUserId
        public async Task<SellerInfoViewModel> GetSellerInfoByUserId(int userId)
        {
            var seller = await GetLastActiveSellerByUserId(userId);
            if (seller == null) return null;
            return new SellerInfoViewModel()
            {
                Email = seller.Email,
                Address = seller.Address,
                Description = seller.Descriptions,
                SellerLogo = seller.Logo,
                Phone = seller.Phone,
                StoreName = seller.StoreName,
                RequstDate = seller.CreatedDate,
                Wallet = await _sellerWalletRepository.GetSellerWalletValueBySellerId(seller.SellerId)
            };
        }
        #endregion

        #region SellerDetailsBySellerId
        public async Task<SellerDetialViewModel> SellerDetailsBySellerId(int sellerId)
        {
            var seller = await _sellerRepository.GetSellerById(sellerId);
            if (seller == null) return null;
            return new SellerDetialViewModel()
            {
                StoreName = seller.StoreName,
                Address = seller.Address,
                Descriptions = seller.Descriptions,
                Email = seller.Email,
                Logo = seller.Logo,
                Phone = seller.Phone,
                UserName = seller.User.UserName,
                UserEmail = seller.User.Email,
                RequstDate = seller.CreatedDate
            };
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _sellerRepository.DisposeAsync();
            await _userRepository.DisposeAsync();
        }
        #endregion
    }
}
