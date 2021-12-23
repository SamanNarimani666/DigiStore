using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Product;
using DigiStore.Domain.IRepositories.Category;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.ProductColor;
using DigiStore.Domain.IRepositories.SelectedProductCategory;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.AspNetCore.Http;

namespace DigiStore.Application.Services.Implementations
{
    public class ProductService : IProductService
    {
        #region Constructor
        private readonly IProductRepository _productRepository;
        private readonly IProductCategoryRepository _productCategoryRepository;
        private readonly ISelectedProductCategoryRepository _selectedProductCategoryRepository;
        private readonly IProductColorRepository _colorRepository;
        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, ISelectedProductCategoryRepository selectedProductCategoryRepository, IProductColorRepository colorRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _selectedProductCategoryRepository = selectedProductCategoryRepository;
            _colorRepository = colorRepository;
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
        public async Task<CreateProductResult> CreateProduc(CreateProductViewModel createProduct, int sellerId, IFormFile productImage)
        {
            if (productImage == null) return CreateProductResult.HasNoImage;
            var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(productImage.FileName);
            var res = productImage.AddImageToServer(imageName, PathExtension.ProductImageOriginServer, 100, 100, PathExtension.ProductImageThumbServer);
            if (res)
            {
                try
                {
                    var product = new Product
                    {
                        SellerId = sellerId,
                        Name = createProduct.Title,
                        Price = createProduct.Price,
                        ShortDescription = createProduct.ShortDescription,
                        Description = createProduct.Description,
                        IsActive = createProduct.IsActive,
                        BrandId = createProduct.BradnId,
                        ImageName = imageName,
                        ProductAcceptanceState = (byte)ProductAcceptanceState.UnderProgress
                    };
                    await _productRepository.AddProduct(product);
                    await _productRepository.Save();
                    //Add Product Category
                    var productSelectedCategories = new List<ProductSelectedCategory>();
                    foreach (var categoryId in createProduct.SelectedCategories)
                    {
                        productSelectedCategories.Add(
                           new ProductSelectedCategory()
                           {
                               ProductId = product.ProductId,
                               ProductCategoryId = categoryId
                           });
                    }
                    await _selectedProductCategoryRepository.AddSelectedProductCategory(productSelectedCategories);
                    await _selectedProductCategoryRepository.Save();
                    //Add Product Color
                    var productColor = new List<Color>();
                    foreach (var colors in createProduct.ProductColors)
                    {
                        productColor.Add(new Color()
                        {
                            Price = colors.Price,
                            ColorName = colors.ColorName,
                            ProductId = product.ProductId
                        });
                    }
                    await _colorRepository.AddColor(productColor);
                    await _colorRepository.Save();
                    return CreateProductResult.Success;
                }
                catch
                {
                    return CreateProductResult.Error;
                }
            }
            return CreateProductResult.Error;
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
            await _productCategoryRepository.DisposeAsync();
            await _selectedProductCategoryRepository.DisposeAsync();
            await _colorRepository.DisposeAsync();
        }
        #endregion
    }
}
