using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Seller;

namespace DigiStore.Application.Services.Interfaces
{
    public interface ISellerService : IAsyncDisposable
    {
        Task<RequestSellerResult> AddNewSellerRequet(RequestSellerViewModel requestSeller,int userId);
        Task<FilterSellerViewModel> FilterSeller(FilterSellerViewModel filterSeller);
        Task<EditRequestSellerViewModel> GetEditRequestSellerInfo(int sellerId,int userId);
       
        Task<EditRequestSellerResult> EditSellerRequest(EditRequestSellerViewModel editRequestSeller,int userId);
    }
}
