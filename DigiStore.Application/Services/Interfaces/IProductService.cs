using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.FavoriteProductUser;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.ProductComment;
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
        Task AddProductSelectedCategories(int productId, List<int> CategoriesId);
        Task AddProductSelectedColors(int productId, List<CreateProductColorViewModel> createProductColor);
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
        Task<List<Domain.Entities.Product>> GetPopularProduct(int take);
        Task<bool> CreateIFavoriteProductUser(CreateIFavoriteProductUserViewModel createIFavoriteProductUser,int userId);
        Task<bool> IsExistThisProductInUserFavoritList(int productId, int userId);
        Task<FilterFavoritViewModel> GetFavoriteProductUserByUserId(FilterFavoritViewModel filterFavorit);
        Task<bool> DeleteFavoritProduct(int favoritId,int productId,int userId);
        Task<CreateProductCommnetResult> CreateProductCommnet(CreateProductCommnetViewModel createProductCommnet,int userId);
        Task<Product> GetProductByProductId(int productId);
        Task<FilterProductCommentViewModel> filterFilterProductComment(FilterProductCommentViewModel filterProductComment);
        Task<List<Domain.Entities.Product>> GetMostPopular(int take);
        Task<List<Domain.Entities.Product>> RecommendedproductsForUser(int take, int userId);
        Task<List<Domain.Entities.Product>> TheMostVisitedProducts(int take);
        Task<ProductRatingViewModel> GetProductRatingByProductId(int productId);
        Task<List<Domain.Entities.Product>> GetAllActiveProductByCategoryId(int categoryId, int count);
        Task<ProductCategory> GetProductCategoryByCategoryId(int categoryId);
    }
}
