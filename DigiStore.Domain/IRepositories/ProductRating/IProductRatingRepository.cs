using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Product;


namespace DigiStore.Domain.IRepositories.ProductRating
{
    public interface IProductRatingRepository:IAsyncDisposable
    {
        Task AddProductRating(Entities.ProductRating productRating);
        Task<ProductRatingViewModel> GetProductRatingByProductId(int productId);
        Task<bool> IsHaveProductRating(int productId);
        Task Save();
    }
}
