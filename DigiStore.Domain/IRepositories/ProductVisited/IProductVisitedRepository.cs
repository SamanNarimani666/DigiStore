using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.ProductVisited;

namespace DigiStore.Domain.IRepositories.ProductVisited
{
    public interface IProductVisitedRepository:IAsyncDisposable
    {
        Task AddProductVisited(Entities.ProductVisited productVisited);
        Task<Entities.ProductVisited> GetProductVisitedByProductIdAndUserId(int productId, int userId);
        Task<FilterProductVisitedViewModel> filterFilterProductVisited(FilterProductVisitedViewModel filterProductVisited);
        Task<bool> IsVisitedUserThisProduct(int productId,int userId);
        void UpdateProductVisited(Entities.ProductVisited productVisited);
        Task Save();
    }
}
