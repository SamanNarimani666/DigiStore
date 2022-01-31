using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Common;
using DigiStore.Domain.ViewModels.Seller;

namespace DigiStore.Application.Services.Interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        Task<RequestSellerResult> AddNewSellerRequet(RequestSellerViewModel requestSeller, int userId);
        Task<FilterSellerViewModel> FilterSeller(FilterSellerViewModel filterSeller);
        Task<EditRequestSellerViewModel> GetEditRequestSellerInfo(int sellerId, int userId);
        Task<EditRequestSellerResult> EditSellerRequest(EditRequestSellerViewModel editRequestSeller, int userId);
        Task<bool> AcceptSellerRequest(int sellerId);
        Task<bool> RejectItem(RejectItemViewModel rejectItem);
        Task<Seller> GetLastActiveSellerByUserId(int userId);
        Task<bool> HasUserActiveSellerPanel(int userId);
        Task<SellerInfoViewModel> GetSellerInfoByUserId(int userId);
        Task<SellerDetialViewModel> SellerDetailsBySellerId(int sellerId);
    }
}
