using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.ProductDiscount;

namespace DigiStore.Domain.IRepositories.ProductDiscount
{
    public interface IProductDiscountRepository:IAsyncDisposable
    {
        Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filterProductDiscount);
        Task AddProductDiscount(Entities.ProductDiscount productDiscount);
        Task Save();
    }
}
