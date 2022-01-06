using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.ProductDiscountUse
{
    public interface IProductDiscountUseRepository : IAsyncDisposable
    {
        Task AddProductDiscountUse(Entities.ProductDiscountUse discountUse);
        Task Save();
    }
}
