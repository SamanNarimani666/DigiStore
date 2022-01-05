using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.ProductVisited;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IProductService : IAsyncDisposable
    {
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct);
        Task<List<ProductCategory>> GetAllActiveProductCategory();
        Task<List<ProductCategory>> GetAllProductCategoriesByParentId(int? parentId);
        Task<CreateProductResult> CreateProduc(CreateProductViewModel createProduct, int sellerId, IFormFile productImage);
        Task<bool> AcceptSellerProduct(int productId);
        Task<RejectProductViewModel> GetPorductInfoForReject(int productId);
        Task<bool> RejectSellerProduct(RejectProductViewModel rejectProduct);
        Task<EditProductViewModel> GetProductForEdit(int productId);
        Task<EditProductResult> EditProduct(EditProductViewModel editProduct, int userId, IFormFile ProductImage);
        void RemoveAllProductSelectedCategories(int productId);
        void RemoveAllProductSelectedColors(int productId);
        void RemoveAllProductSelectedGuarantee(int productId);
        Task AddProductSelectedCategories(int productId, List<int> CategoriesId);
        Task AddProductSelectedColors(int productId, List<CreateProductColorViewModel> createProductColor);
        Task AddAllProductSelectedGuarantee(int productId, List<CreateProductGuaranteeViewModel> createProductGuarantee);
        Task<List<ProductGallery>> GetAllProductGallery(int productId);
        Task<Product> GetProductBySellerOwnerId(int productId, int userId);
        Task<List<ProductGallery>> GetAllProductGalleryForSellerpanel(int productId, int sellerId);
        Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGalleryViewModel createProductGallery,int productId,int sellerId, IFormFile ProductImage);
        Task<EditOrDeleteProductGalleryViewModel> GetEditProductGalleryForEdit(int galleryId,int sellerId);
        Task<EditOrDeleteProductGalleryResult> EditProductGallery(EditOrDeleteProductGalleryViewModel editProductGallery, int galleryId, int sellerId, IFormFile ProductImage);
        Task<EditOrDeleteProductGalleryResult> DeleteProductGallery(EditOrDeleteProductGalleryViewModel deleteProductGallery, int galleryId, int sellerId);
        Task<EditOrDeleteProductGalleryResult> ResotrProductGallery(EditOrDeleteProductGalleryViewModel deleteProductGallery, int galleryId, int sellerId);
        Task<ProductDetailViewModel> GetProductDetail(int productId);
        Task<bool> AddProductVisited(int productId,string userIp,int? userId);
        Task AddProductFeatures(int productId,List<CreateProductFeatureViewModel> createProductFeature);
        void RemoveAllProductFeatures(int productId);
        Task<FilterProductVisitedViewModel> GetLastProductVisited(FilterProductVisitedViewModel filterProductVisited);
       
    }
}
