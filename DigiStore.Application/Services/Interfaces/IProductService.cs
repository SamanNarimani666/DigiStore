using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IProductService:IAsyncDisposable
    {
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct);
        Task<List<ProductCategory>> GetAllActiveProductCategory();
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(int? parentId);
        Task<CreateProductResult> CreateProduc(CreateProductViewModel createProduct,int sellerId,IFormFile productImage);
    }
}
