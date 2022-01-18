using System;
using System.Threading.Tasks;


namespace DigiStore.Domain.IRepositories.ProductRating
{
    public interface IProductRatingRepository:IAsyncDisposable
    {
        Task AddProductRating(Entities.ProductRating productRating);
        Task Save();
    }
}
