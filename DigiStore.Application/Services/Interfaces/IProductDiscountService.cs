using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Discount;
using DigiStore.Domain.ViewModels.ProductDiscount;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IProductDiscountService:IAsyncDisposable
    {
        Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filterProductDiscount);
        Task<CreateProductDiscountResult> CreateProductDiscoun(CreateProductDiscountViewModel createProductDiscount, int sellerId);
    }
}
