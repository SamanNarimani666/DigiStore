using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Extensions;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Application.Utils;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Product;
using DigiStore.Domain.IRepositories.Category;
using DigiStore.Domain.IRepositories.Guarantee;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.ProductColor;
using DigiStore.Domain.IRepositories.ProductGallery;
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
        private readonly IProductGuaranteeRepository _guaranteeRepository;
        private readonly IProductGalleryRepository _galleryRepository;
        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, ISelectedProductCategoryRepository selectedProductCategoryRepository, IProductColorRepository colorRepository, IProductGuaranteeRepository guaranteeRepository, IProductGalleryRepository galleryRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _selectedProductCategoryRepository = selectedProductCategoryRepository;
            _colorRepository = colorRepository;
            _guaranteeRepository = guaranteeRepository;
            _galleryRepository = galleryRepository;
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
                    await AddProductSelectedCategories(product.ProductId, createProduct.SelectedCategories);
                    //Add Product Color
                    await AddProductSelectedColors(product.ProductId, createProduct.ProductColors);
                    // Add Product Guarantee
                    await AddAllProductSelectedGuarantee(product.ProductId, createProduct.ProductGuarantee);
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

        #region AcceptSellerProduct
        public async Task<bool> AcceptSellerProduct(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product != null)
            {
                product.ProductAcceptanceState = (byte)ProductAcceptanceState.Accepted;
                product.ProductAcceptOrRejectDescription = $"محصول مورد نظر در تاریخ {DateTime.Now.ToShamsiDate()} در سایت قرار گرفت";
                _productRepository.EditProduct(product);
                await _productRepository.Save();
                return true;
            }
            return false;
        }

        public async Task<RejectProductViewModel> GetPorductInfoForReject(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            return new RejectProductViewModel()
            {
                ProductName = product.Name,
                Id = productId
            };
        }

        #endregion

        #region RejectSellerProduct
        public async Task<bool> RejectSellerProduct(RejectProductViewModel rejectProduct)
        {
            var product = await _productRepository.GetProductById(rejectProduct.Id);
            if (product != null)
            {
                product.ProductAcceptanceState = (byte)ProductAcceptanceState.Rejected;
                product.ProductAcceptOrRejectDescription = rejectProduct.RejectMessage;
                _productRepository.EditProduct(product);
                await _productRepository.Save();
                return true;
            }
            return false;
        }
        #endregion

        #region GetProductForEdit
        public async Task<EditProductViewModel> GetProductForEdit(int productId)
        {
            var product = await _productRepository.GetProductById(productId);
            return new EditProductViewModel()
            {
                Title = product.Name,
                BradnId = product.BrandId,
                Description = product.Description,
                IsActive = product.IsActive,
                Price = product.Price.Value,
                ShortDescription = product.ShortDescription,
                ProductImage = product.ImageName,
                ProductId = product.ProductId,
                ProductColors = _colorRepository.GetColorProductByProductId(productId).Select(c => new CreateProductColorViewModel()
                {
                    ColorName = c.ColorName,
                    Price = c.Price.Value
                }).ToList(),
                SelectedCategories = _selectedProductCategoryRepository.GetProductSelectedCategoryByProductId(productId).Select(p => p.ProductCategoryId.Value).ToList()
                ,
                ProductGuarantee = _guaranteeRepository.GetProductGuaranteesByProductId(productId).Select(g => new CreateProductGuaranteeViewModel()
                {
                    GuaranteeName = g.GuaranteeName,
                    Price = g.Price.Value
                }).ToList(),
            };
        }
        #endregion

        #region EditProduct
        public async Task<EditProductResult> EditProduct(EditProductViewModel editProduct, int userId, IFormFile ProductImage)
        {
            var mainProduct = await _productRepository.GetProductWithSellerById(editProduct.ProductId);
            if (mainProduct == null) return EditProductResult.NotFoundProduct;
            if (mainProduct.Seller.UserId != userId) return EditProductResult.NotFoundUser;

            try
            {
                mainProduct.Name = editProduct.Title;
                mainProduct.Price = editProduct.Price;
                mainProduct.BrandId = editProduct.BradnId;
                mainProduct.Description = editProduct.Description;
                mainProduct.ShortDescription = editProduct.ShortDescription;
                mainProduct.IsActive = editProduct.IsActive;
                mainProduct.ProductAcceptanceState = (byte)ProductAcceptanceState.UnderProgress;
                if (ProductImage != null)
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(ProductImage.FileName);
                    var res = ProductImage.AddImageToServer(imageName, PathExtension.ProductImageOriginServer, 100, 100, PathExtension.ProductImageThumbServer, mainProduct.ImageName);
                    if (res)
                    {
                        mainProduct.ImageName = imageName;
                    }
                }


                //Remove Color Product
                RemoveAllProductSelectedColors(editProduct.ProductId);
                //Add Product Color
                await AddProductSelectedColors(editProduct.ProductId, editProduct.ProductColors);



                //Remove Product Selected Categoty
                RemoveAllProductSelectedCategories(editProduct.ProductId);
                //Add Product Selected Categoty
                await AddProductSelectedCategories(editProduct.ProductId, editProduct.SelectedCategories);



                // Remove Product Selected guarantee
                RemoveAllProductSelectedGuarantee(editProduct.ProductId);
                // Add Product Guarantee
                await AddAllProductSelectedGuarantee(editProduct.ProductId, editProduct.ProductGuarantee);



                _productRepository.EditProduct(mainProduct);
                await _productRepository.Save();
                return EditProductResult.Success;
            }
            catch
            {
                return EditProductResult.Erorr;
            }


        }
        #endregion

        #region RemoveAllProductSelectedCategories
        public void RemoveAllProductSelectedCategories(int productId)
        {
            _selectedProductCategoryRepository.DeleteProductSelectedCategory(_selectedProductCategoryRepository.GetProductSelectedCategoryByProductId(productId));
        }
        #endregion

        #region RemoveAllProductSelectedColors
        public void RemoveAllProductSelectedColors(int productId)
        {
            _colorRepository.DeleteProductColor(_colorRepository.GetColorProductByProductId(productId));
        }
        #endregion

        #region RemoveAllProductSelectedGuarantee
        public void RemoveAllProductSelectedGuarantee(int productId)
        {
            _guaranteeRepository.DeleteProductGuarantee(_guaranteeRepository.GetProductGuaranteesByProductId(productId));
        }
        #endregion

        #region AddProductSelectedCategories
        public async Task AddProductSelectedCategories(int productId, List<int> CategoriesId)
        {
            if (CategoriesId != null)
            {
                var productSelectedCategories = new List<ProductSelectedCategory>();
                foreach (var categoryId in CategoriesId)
                {
                    productSelectedCategories.Add(
                        new ProductSelectedCategory()
                        {
                            ProductId = productId,
                            ProductCategoryId = categoryId
                        });
                }
                await _selectedProductCategoryRepository.AddSelectedProductCategory(productSelectedCategories);
            }
            await _selectedProductCategoryRepository.Save();
        }
        #endregion

        #region AddProductSelectedColors
        public async Task AddProductSelectedColors(int productId, List<CreateProductColorViewModel> createProductColor)
        {
            if (createProductColor != null)
            {
                var productColor = new List<Color>();
                foreach (var colors in createProductColor)
                {
                    if (productColor.All(c => c.ColorName != colors.ColorName))
                    {
                        productColor.Add(new Color()
                        {
                            Price = colors.Price,
                            ColorName = colors.ColorName,
                            ProductId = productId
                        });
                    }

                }
                await _colorRepository.AddColor(productColor);
            }
            await _colorRepository.Save();
        }
        #endregion

        #region AddAllProductSelectedGuarantee
        public async Task AddAllProductSelectedGuarantee(int productId, List<CreateProductGuaranteeViewModel> createProductGuarantee)
        {
            if (createProductGuarantee != null)
            {
                var productguarantee = new List<Guarantee>();
                foreach (var guarantees in createProductGuarantee)
                {
                    productguarantee.Add(new Guarantee()
                    {
                        ProductId = productId,
                        Price = guarantees.Price,
                        GuaranteeName = guarantees.GuaranteeName
                    });
                }

                await _guaranteeRepository.AddProductGuarantee(productguarantee);
            }
            await _guaranteeRepository.Save();
        }
        #endregion

        #region GetAllProductGallery
        public async Task<List<ProductGallery>> GetAllProductGallery(int productId)
        {
            return await _galleryRepository.GetAllProductGallery(productId);
        }
        #endregion

        #region GetAllProductGalleryForSellerpanel
        public async Task<List<ProductGallery>> GetAllProductGalleryForSellerpanel(int productId, int sellerId)
        {
            return await _galleryRepository.GetAllProductGalleryForSellerpanel(productId, sellerId);
        }
        #endregion

        #region GetProductWithSellerById
        public async Task<Product> GetProductBySellerOwnerId(int productId, int userId)
        {
            return await _productRepository.GetProductBySellerOwnerId(productId, userId);
        }
        #endregion

        #region CreateProductGallery
        public async Task<CreateProductGalleryResult> CreateProductGallery(CreateProductGalleryViewModel createProductGallery, int productId, int sellerId, IFormFile ProductImage)
        {
            var product = await _productRepository.GetProductById(productId);
            if (product == null) return CreateProductGalleryResult.ProductNotFound;
            if (product.SellerId != sellerId) return CreateProductGalleryResult.NotForUserProduct;
            if (ProductImage == null || !ProductImage.IsImage()) return CreateProductGalleryResult.ImageIsNull;
            try
            {
                var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(ProductImage.FileName);
                var res = ProductImage.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100, PathExtension.ProductGalleryThumbServer);
                if (res)
                {
                    await _galleryRepository.AddProductGallery(new ProductGallery()
                    {
                        ProductId = product.ProductId,
                        ImageName = imageName,
                        DisplayPrority = createProductGallery.DisplayPrority
                    });
                    await _galleryRepository.Save();
                }
                return CreateProductGalleryResult.Success;
            }
            catch
            {
                return CreateProductGalleryResult.Error;
            }
        }
        #endregion

        #region GetEditProductGalleryForEdit
        public async Task<EditOrDeleteProductGalleryViewModel> GetEditProductGalleryForEdit(int galleryId, int sellerId)
        {
            var gallery = await _galleryRepository.GetProductGalleryByGalleryIdAndSellerId(galleryId, sellerId);
            if (gallery == null) return null;
            return new EditOrDeleteProductGalleryViewModel()
            {
                DisplayPrority = gallery.DisplayPrority.Value,
                ImageName = gallery.ImageName,
                GalleryId = gallery.ProductGalleryId,
                ProductId = gallery.ProductId
            };
        }
        #endregion

        #region EditProductGallery
        public async Task<EditOrDeleteProductGalleryResult> EditProductGallery(EditOrDeleteProductGalleryViewModel editProductGallery, int galleryId, int sellerId,
            IFormFile ProductImage)
        {
            var maingallery = await _galleryRepository.GetProductGalleryByGalleryIdAndSellerId(galleryId, sellerId);
            if (maingallery == null) return EditOrDeleteProductGalleryResult.GalleryNotFound;
            if (maingallery.Product.SellerId != sellerId) return EditOrDeleteProductGalleryResult.NotForUserProduct;
            try
            {
                if (ProductImage != null && ProductImage.IsImage())
                {
                    var imageName = Generators.Generators.GeneratorsUniqueCode() + Path.GetExtension(ProductImage.FileName);
                    var res = ProductImage.AddImageToServer(imageName, PathExtension.ProductGalleryOriginServer, 100, 100, PathExtension.ProductGalleryThumbServer, maingallery.ImageName);
                    if (res)
                    {
                        maingallery.ImageName = imageName;

                    }
                }
                maingallery.DisplayPrority = editProductGallery.DisplayPrority;
                _galleryRepository.EditProductGallery(maingallery);
                await _galleryRepository.Save();
                return EditOrDeleteProductGalleryResult.Success;
            }
            catch
            {
                return EditOrDeleteProductGalleryResult.Error;
            }
        }
        #endregion

        #region DeleteProductGallery
        public async Task<EditOrDeleteProductGalleryResult> DeleteProductGallery(EditOrDeleteProductGalleryViewModel deleteProductGallery, int galleryId, int sellerId)
        {
            var maingallery = await _galleryRepository.GetProductGalleryByGalleryIdAndSellerId(galleryId, sellerId);
            if (maingallery == null) return EditOrDeleteProductGalleryResult.GalleryNotFound;
            if (maingallery.Product.SellerId != sellerId) return EditOrDeleteProductGalleryResult.NotForUserProduct;
            try
            {
                maingallery.IsDelete = true;
                _galleryRepository.EditProductGallery(maingallery);
                await _galleryRepository.Save();
                return EditOrDeleteProductGalleryResult.Success;
            }
            catch
            {
                return EditOrDeleteProductGalleryResult.Error;
            }
        }
        #endregion

        #region ResotrProductGallery
        public async Task<EditOrDeleteProductGalleryResult> ResotrProductGallery(EditOrDeleteProductGalleryViewModel deleteProductGallery, int galleryId, int sellerId)
        {
            var maingallery = await _galleryRepository.GetProductGalleryByGalleryIdAndSellerId(galleryId, sellerId);
            if (maingallery == null) return EditOrDeleteProductGalleryResult.GalleryNotFound;
            if (maingallery.Product.SellerId != sellerId) return EditOrDeleteProductGalleryResult.NotForUserProduct;
            try
            {
                maingallery.IsDelete = false;
                _galleryRepository.EditProductGallery(maingallery);
                await _galleryRepository.Save();
                return EditOrDeleteProductGalleryResult.Success;
            }
            catch
            {
                return EditOrDeleteProductGalleryResult.Error;
            }
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
            await _productCategoryRepository.DisposeAsync();
            await _selectedProductCategoryRepository.DisposeAsync();
            await _colorRepository.DisposeAsync();
            await _guaranteeRepository.DisposeAsync();
            await _galleryRepository.DisposeAsync();
        }
        #endregion
    }
}
