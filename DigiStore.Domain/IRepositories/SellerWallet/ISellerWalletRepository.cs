using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.SellerWallet;

namespace DigiStore.Domain.IRepositories.SellerWallet
{
    public interface ISellerWalletRepository : IAsyncDisposable
    {
        Task AddSellerWallet(Entities.SellerWallet  sellerWallet);
        Task<FilterSellerWalletViewModel> FilterSellerWallet(FilterSellerWalletViewModel filterSellerWallet);
        Task<int> GetSellerWalletValueBySellerId(int sellerId);
        Task Save();
    }
}
