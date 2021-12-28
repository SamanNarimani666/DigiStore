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
        Task Save();
    }
}
