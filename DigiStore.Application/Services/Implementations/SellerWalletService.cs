using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.SellerWallet;
using DigiStore.Domain.ViewModels.SellerWallet;

namespace DigiStore.Application.Services.Implementations
{

   public class SellerWalletService : ISellerWalletService
    {
        #region Constructor
        private readonly ISellerWalletRepository _sellerWalletRepository;
        public SellerWalletService(ISellerWalletRepository sellerWalletRepository)
        {
            _sellerWalletRepository = sellerWalletRepository;
        }
        #endregion

        #region FilterSellerWallet
        public async Task<FilterSellerWalletViewModel> FilterSellerWallet(FilterSellerWalletViewModel filterSellerWallet)
        {
           return await _sellerWalletRepository.FilterSellerWallet(filterSellerWallet);
        }
        #endregion

      

        #region Dispose
        public async  ValueTask DisposeAsync()
        {
            await _sellerWalletRepository.DisposeAsync();
        }
        #endregion
    }
}
