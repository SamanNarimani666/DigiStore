using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Seller;

namespace DigiStore.Domain.IRepositories.Seller
{
    public interface ISellerRepository : IAsyncDisposable
    {
        Task AddSeller(Entities.Seller seller);
        Task<bool> HasHanderprograssRequestUser(int userId);
        Task<FilterSellerViewModel> FilterSeller(FilterSellerViewModel filterSeller);
        Task<Entities.Seller> GetSellerById(int sellerId);
        void EditSeller(Entities.Seller seller);
        Task<Entities.Seller> GetLastActiveSellerByUserId(int userId);
        Task<bool> HasUserActiveSellerPanel(int userId);
        Task<int> NumberOfActiveSeller();
        Task Save();
    }
}
