using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Product;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IProductService:IAsyncDisposable
    {
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct);
        Task<List<ProductCategory>> GetAllActiveProductCategory();
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(int? parentId);
        Task<CreateProductResult> CreateProduc(CreateProductViewModel createProduct,string imageName,int sellerId);
    }
}
