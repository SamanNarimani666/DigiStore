using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.ProductDiscount;

namespace DigiStore.Domain.IRepositories.ProductDiscount
{
    public interface IProductDiscountRepository:IAsyncDisposable
    {
        Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filterProductDiscount);
        Task AddProductDiscount(Entities.ProductDiscount productDiscount);
        Task<Entities.ProductDiscount> GetProductDiscountByProductId(int productId);
        Task<List<Domain.Entities.ProductDiscount>> GetAlloffProducs(int take);
        Task Save();
    }
}
