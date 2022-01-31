using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Product;

namespace DigiStore.Domain.IRepositories.Product
{
    public interface IProductRepository : IAsyncDisposable
    {
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct);
        Task AddProduct(Entities.Product product);
        Task<Entities.Product> GetProductById(int productId);
        void EditProduct(Entities.Product product);
        Task<Entities.Product> GetProductWithSellerById(int productId);
        Task<Entities.Product> GetProductBySellerOwnerId(int productId,int userId);
        Task<ProductDetailViewModel> GetProductDetail(int productId);
        Task<List<Entities.Product>> GetPopularProduct(int take);
        Task<List<Entities.Product>> GetMostPopular(int take);
        Task<List<Entities.Product>> RecommendedproductsForUser(int take,int userId);
        Task<List<Entities.Product>> TheMostVisitedProducts(int take);
        Task<List<Entities.Product>> GetAllActiveProductByCategoryId(int categoryId, int count);
        Task<FilterProductViewModel> FilterForSiteSearch(FilterProductViewModel filterProduct);
        Task<Entities.Product> TheMostProductVisited();
        Task<Entities.Product> TheBestSellingProduct();
        Task<Entities.Product> TheBestPopularProduct();


        Task Save();
    }
}
