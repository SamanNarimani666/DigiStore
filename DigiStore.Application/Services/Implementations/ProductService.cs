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
using DigiStore.Domain.IRepositories.FavoriteProductUser;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.ProductColor;
using DigiStore.Domain.IRepositories.Productcomment;
using DigiStore.Domain.IRepositories.ProductFeature;
using DigiStore.Domain.IRepositories.ProductGallery;
using DigiStore.Domain.IRepositories.ProductRating;
using DigiStore.Domain.IRepositories.ProductVisited;
using DigiStore.Domain.IRepositories.SelectedProductCategory;
using DigiStore.Domain.ViewModels.FavoriteProductUser;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.ProductComment;
using DigiStore.Domain.ViewModels.ProductVisited;
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
        private readonly IProductGalleryRepository _galleryRepository;
        private readonly IProductVisitedRepository _productVisitedRepository;
        private readonly IProductFeatureRepository _productFeatureRepository;
        private readonly IFavoriteProductUserRepository _favoriteProductUserRepository;
        private readonly IProductcommentRepository _productcommentRepository;
        private readonly IProductRatingRepository _productRatingRepository;
        public ProductService(IProductRepository productRepository, IProductCategoryRepository productCategoryRepository, ISelectedProductCategoryRepository selectedProductCategoryRepository, IProductColorRepository colorRepository, IProductGalleryRepository galleryRepository, IProductVisitedRepository productVisitedRepository, IProductFeatureRepository productFeatureRepository, IFavoriteProductUserRepository favoriteProductUserRepository, IProductcommentRepository productcommentRepository, IProductRatingRepository productRatingRepository)
        {
            _productRepository = productRepository;
            _productCategoryRepository = productCategoryRepository;
            _selectedProductCategoryRepository = selectedProductCategoryRepository;
            _colorRepository = colorRepository;
            _galleryRepository = galleryRepository;
            _productVisitedRepository = productVisitedRepository;
            _productFeatureRepository = productFeatureRepository;
            _favoriteProductUserRepository = favoriteProductUserRepository;
            _productcommentRepository = productcommentRepository;
            _productRatingRepository = productRatingRepository;
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
                        SiteProfile = createProduct.SiteProfile,
                        ProductAcceptanceState = (byte)ProductAcceptanceState.UnderProgress
                    };
                    await _productRepository.AddProduct(product);
                    await _productRepository.Save();
                    //Add Product Category
                    await AddProductSelectedCategories(product.ProductId, createProduct.SelectedCategories);
                    //Add Product Color
                    await AddProductSelectedColors(product.ProductId, createProduct.ProductColors);
                    //Add Product Features
                    await AddProductFeatures(product.ProductId, createProduct.ProductFeature);
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
                Price = product.Price,
                ShortDescription = product.ShortDescription,
                ProductImage = product.ImageName,
                ProductId = product.ProductId,
                SiteProfile = product.SiteProfile.Value,
                ProductColors = _colorRepository.GetColorProductByProductId(productId).Select(c => new CreateProductColorViewModel()
                {
                    ColorCode = c.ColorCode,
                    Price = c.Price
                }).ToList(),
                SelectedCategories = _selectedProductCategoryRepository.GetProductSelectedCategoryByProductId(productId).Select(p => p.ProductCategoryId.Value).ToList()
                ,
                ProductFeature = _productFeatureRepository.GetProductFeatureByProductId(productId).Select(f => new CreateProductFeatureViewModel()
                {
                    FeatureTitle = f.FeatureTitle,
                    FeatureValue = f.FeatureValue
                }).ToList()
            };
        }
        #endregion

        #region EditProduct
        public async Task<EditProductResult> EditProduct(EditProductViewModel editProduct, int userId, IFormFile ProductImage)
        {
            var mainProduct = await _productRepository.GetProductWithSellerById(editProduct.ProductId);
            if (mainProduct == null) return EditProductResult.NotFoundProduct;
            if (mainProduct.Seller.UserId != userId) return EditProductResult.NotFoundUser;
            //Remove Color Product
            RemoveAllProductSelectedColors(editProduct.ProductId);
            //Add Product Color
            await AddProductSelectedColors(editProduct.ProductId, editProduct.ProductColors);
            try
            {
                mainProduct.Name = editProduct.Title;
                mainProduct.Price = editProduct.Price;
                mainProduct.BrandId = editProduct.BradnId;
                mainProduct.Description = editProduct.Description;
                mainProduct.ShortDescription = editProduct.ShortDescription;
                mainProduct.IsActive = editProduct.IsActive;
                mainProduct.SiteProfile = editProduct.SiteProfile;
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




                //Remove Product Selected Categoty
                RemoveAllProductSelectedCategories(editProduct.ProductId);
                //Add Product Selected Categoty
                await AddProductSelectedCategories(editProduct.ProductId, editProduct.SelectedCategories);

                //Remove Color Product
                RemoveAllProductSelectedColors(editProduct.ProductId);
                //Add Product Color
                await AddProductSelectedColors(editProduct.ProductId, editProduct.ProductColors);

                //Remove Product Features
                RemoveAllProductFeatures(editProduct.ProductId);
                //Add Product Features
                await AddProductFeatures(editProduct.ProductId, editProduct.ProductFeature);

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
            var productsColors = _colorRepository.GetColorProductByProductId(productId);
            foreach (var colors in productsColors)
            {
                colors.IsDelete = true;
            }
            _colorRepository.EditProductColoe(productsColors);
        }
        #endregion

        #region AddProductSelectedCategories
        public async Task AddProductSelectedCategories(int productId, List<int> CategoriesId)
        {
            if (CategoriesId != null && CategoriesId.Any())
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
            if (createProductColor != null && createProductColor.Any())
            {
                var productColor = new List<Color>();
                foreach (var colors in createProductColor)
                {
                    if (productColor.All(c => c.ColorCode != colors.ColorCode))
                    {
                        productColor.Add(new Color()
                        {
                            Price = colors.Price,
                            ColorCode = colors.ColorCode,
                            ProductId = productId
                        });
                    }

                }
                _colorRepository.AddColor(productColor);
            }
            await _colorRepository.Save();
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

        #region GetProductDetail
        public async Task<ProductDetailViewModel> GetProductDetail(int productId)
        {
            return await _productRepository.GetProductDetail(productId);
        }

        #endregion

        #region AddProductVisited
        public async Task<bool> AddProductVisited(int productId, string userIp, int? userId)
        {
            if (userId == 0)
                userId = null;
            if (productId != 0 && userIp != null)
            {
                try
                {
                    if (userId != null && await _productVisitedRepository.IsVisitedUserThisProduct(productId, userId.Value))
                    {
                        var IsVisitedUserThisProduct = await _productVisitedRepository.GetProductVisitedByProductIdAndUserId(productId, userId.Value);
                        IsVisitedUserThisProduct.ModifiedDate = DateTime.Now;
                        _productVisitedRepository.UpdateProductVisited(IsVisitedUserThisProduct);
                        var newProductVisted = new ProductVisited()
                        {
                            ProductId = productId,
                            UserId = null,
                            UserIp = userIp
                        };
                        await _productVisitedRepository.AddProductVisited(newProductVisted);
                    }
                    else
                    {
                        var newProductVisted = new ProductVisited()
                        {
                            ProductId = productId,
                            UserId = userId,
                            UserIp = userIp
                        };
                        await _productVisitedRepository.AddProductVisited(newProductVisted);
                    }
                    await _productVisitedRepository.Save();
                    return true;
                }
                catch
                {
                    return false;
                }
            }
            return false;
        }
        #endregion

        #region AddProductFeatures
        public async Task AddProductFeatures(int productId, List<CreateProductFeatureViewModel> createProductFeature)
        {
            var newFeatures = new List<ProductFeature>();
            if (createProductFeature != null && createProductFeature.Any())
            {
                foreach (var features in createProductFeature)
                {
                    newFeatures.Add(new ProductFeature()
                    {
                        ProductId = productId,
                        FeatureTitle = features.FeatureTitle,
                        FeatureValue = features.FeatureValue
                    });
                }

                await _productFeatureRepository.AddProductFeature(newFeatures);
                await _productFeatureRepository.Save();
            }
        }
        #endregion

        #region RemoveAllProductFeatures
        public void RemoveAllProductFeatures(int productId)
        {
            _productFeatureRepository.RemoveProductFeature(_productFeatureRepository.GetProductFeatureByProductId(productId));
        }
        #endregion

        #region GetLastProductVisited
        public async Task<FilterProductVisitedViewModel> GetLastProductVisited(FilterProductVisitedViewModel filterProductVisited)
        {
            return await _productVisitedRepository.filterFilterProductVisited(filterProductVisited);
        }
        #endregion

        #region GetPopularProduct
        public async Task<List<Product>> GetPopularProduct(int take)
        {
            return await _productRepository.GetPopularProduct(take);
        }
        #endregion

        #region CreateIFavoriteProductUser
        public async Task<bool> CreateIFavoriteProductUser(CreateIFavoriteProductUserViewModel createIFavoriteProductUser, int userId)
        {
            var product = await _productRepository.GetProductById(createIFavoriteProductUser.ProductId);
            if (product == null) return false;
            try
            {
                await _favoriteProductUserRepository.AddFavoriteProductUser(new FavoriteProductUser()
                {
                    UserId = userId,
                    ProductId = createIFavoriteProductUser.ProductId
                });
                await _favoriteProductUserRepository.Save();

                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region IsExistThisProductInUserFavoritList
        public async Task<bool> IsExistThisProductInUserFavoritList(int productId, int userId)
        {
            return await _favoriteProductUserRepository.IsExistThisProductInUserFavoritList(productId, userId);
        }
        #endregion

        #region GetFavoriteProductUserByUserId
        public async Task<FilterFavoritViewModel> GetFavoriteProductUserByUserId(FilterFavoritViewModel filterFavorit)
        {
            return await _favoriteProductUserRepository.GetFavoriteProductUserByUserId(filterFavorit);
        }
        #endregion

        #region DeleteFavoritProduct
        public async Task<bool> DeleteFavoritProduct(int favoritId, int productId, int userId)
        {
            var favoritProduct = await _favoriteProductUserRepository.GetFavoriteProductUserById(favoritId);
            if (favoritProduct == null) return false;
            if (favoritProduct.UserId != userId) return false;
            if (favoritProduct.ProductId != productId) return false;
            try
            {
                favoritProduct.IsDelete = true;
                _favoriteProductUserRepository.UpdateFavoriteProductUser(favoritProduct);
                await _favoriteProductUserRepository.Save();
                return true;
            }
            catch
            {
                return false;
            }
        }
        #endregion

        #region CreateProductCommnet
        public async Task<CreateProductCommnetResult> CreateProductCommnet(CreateProductCommnetViewModel createProductCommnet, int userId)
        {
            var product = await _productRepository.GetProductById(createProductCommnet.ProductId);
            if (product == null) return CreateProductCommnetResult.NotFoundProduct;
            try
            {
                var newComment = new Productcomment()
                {
                    UserId = userId,
                    SellerId = product.SellerId,
                    ProductId = createProductCommnet.ProductId,
                    Title = createProductCommnet.Title,
                    Comment = createProductCommnet.Comment
                };
                await _productcommentRepository.AddProductcomment(newComment);
                await _productcommentRepository.Save();
                var newProductRating = new ProductRating()
                {
                    ProductId = product.ProductId,
                    ConstructionQuality = createProductCommnet.ConstructionQuality,
                    DesignAndAppearance = createProductCommnet.DesignAndAppearance,
                    EaseOfUse = createProductCommnet.EaseOfUse,
                    FeaturesAndCapabilities = createProductCommnet.FeaturesAndCapabilities,
                    Innovation = createProductCommnet.Innovation,
                    PurchaseValueRelativeToPrice = createProductCommnet.PurchaseValueRelativeToPrice
                };
                await _productRatingRepository.AddProductRating(newProductRating);
                await _productRatingRepository.Save();
                return CreateProductCommnetResult.Success;
            }
            catch
            {
                return CreateProductCommnetResult.Error;
            }
        }
        #endregion

        #region GetProductByProductId
        public async Task<Product> GetProductByProductId(int productId)
        {
            return await _productRepository.GetProductById(productId);
        }
        #endregion

        #region filterFilterProductComment
        public async Task<FilterProductCommentViewModel> filterFilterProductComment(FilterProductCommentViewModel filterProductComment)
        {
            return await _productcommentRepository.filterFilterProductComment(filterProductComment);
        }
        #endregion

        #region GetMostPopular
        public async Task<List<Product>> GetMostPopular(int take)
        {
            return await _productRepository.GetMostPopular(take);
        }
        #endregion

        #region RecommendedproductsForUser
        public async Task<List<Product>> RecommendedproductsForUser(int take, int userId)
        {
            var result = await _productRepository.RecommendedproductsForUser(take, userId);
            return result;
        }
        #endregion

        #region TheMostVisitedProducts
        public async Task<List<Product>> TheMostVisitedProducts(int take)
        {
            return await _productRepository.TheMostVisitedProducts(take);
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _productRepository.DisposeAsync();
            await _productCategoryRepository.DisposeAsync();
            await _selectedProductCategoryRepository.DisposeAsync();
            await _colorRepository.DisposeAsync();
            await _galleryRepository.DisposeAsync();
            await _productVisitedRepository.DisposeAsync();
            await _productFeatureRepository.DisposeAsync();
            await _favoriteProductUserRepository.Save();
            await _productcommentRepository.DisposeAsync();
            await _productRatingRepository.DisposeAsync();
        }
        #endregion
    }
}
