using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Brand;
using DigiStore.Domain.IRepositories.Category;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.ViewModels.Product;

namespace DigiStore.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region Constructor
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
        }
        #endregion

        #region FilterProduct
        public async Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct)
        {
            return await _productRepository.FilterProduct(filterProduct);
        }
        #endregion

        #region GetAllProductCategory
        public async Task<List<ProductCategory>> GetAllActiveProductCategory()
        {
            return await _productCategoryRepository.GetAllActiveProductCategory();
        }
        #endregion

        #region GetAllProductCategoriesByParentId
        public async Task<List<ProductCategory>> GetAllProductCategoriesByParentId(int? parentId)
        {
            return await _productCategoryRepository.GetAllProductCategoriesByParentId(parentId);
        }
        #endregion

        #region CreateProduc
        public async Task<CreateProductResult> CreateProduc(CreateProductViewModel createProduct, string imageName, int sellerId)
        {
            throw new System.NotImplementedException();
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
            await _productCategoryRepository.DisposeAsync();
        }
        #endregion
    }
}
