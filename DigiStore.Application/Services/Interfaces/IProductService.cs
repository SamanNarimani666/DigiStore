using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Common;
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
        Task<bool> AcceptSellerProduct(int productId);
        Task<RejectProductViewModel> GetPorductInfoForReject(int productId);
        Task<bool> RejectSellerProduct(RejectProductViewModel rejectProduct);
        Task<EditProductViewModel> GetProductForEdit(int productId);
        Task<EditProductResult> EditProduct(EditProductViewModel editProduct,int userId, IFormFile ProductImage);
        void RemoveAllProductSelectedCategories(int productId);
        void RemoveAllProductSelectedColors(int productId);
        void RemoveAllProductSelectedGuarantee(int productId);
        Task AddProductSelectedCategories(int productId, List<int> CategoriesId);
        Task AddProductSelectedColors(int productId, List<CreateProductColorViewModel> createProductColor );
        Task AddAllProductSelectedGuarantee(int productId, List<CreateProductGuaranteeViewModel> createProductGuarantee);




    }
}
