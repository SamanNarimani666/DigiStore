using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.SellerWallet;

namespace DigiStore.Application.Services.Interfaces
{
    public interface ISellerWalletService:IAsyncDisposable
    {
        Task<FilterSellerWalletViewModel> FilterSellerWallet(FilterSellerWalletViewModel filterSellerWallet);
        
    }
}
