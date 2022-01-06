using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Application.Utils;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.ProductDiscount;
using DigiStore.Domain.IRepositories.ProductDiscountUse;
using DigiStore.Domain.ViewModels.Discount;
using DigiStore.Domain.ViewModels.ProductDiscount;

namespace DigiStore.Application.Services.Implementations
{
    public class ProductDiscountService : IProductDiscountService
    {
        #region Constructor
        private readonly IProductDiscountRepository _discountRepository;
        private readonly IProductDiscountUseRepository _discountUseRepository;
        private readonly IProductRepository _productRepository;
        public ProductDiscountService(IProductDiscountRepository discountRepository, IProductDiscountUseRepository discountUseRepository, IProductRepository productRepository)
        {
            _discountRepository = discountRepository;
            _discountUseRepository = discountUseRepository;
            _productRepository = productRepository;
        }
        #endregion

        #region FilterProductDiscount
        public async Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filterProductDiscount)
        {
            return await _discountRepository.FilterProductDiscount(filterProductDiscount);
        }
        #endregion

        #region CreateProductDiscoun
        public async Task<CreateProductDiscountResult> CreateProductDiscoun(CreateProductDiscountViewModel createProductDiscount, int sellerId)
        {
            var product = await _productRepository.GetProductById(createProductDiscount.ProductId);
            if (product == null) return CreateProductDiscountResult.NotFoundProduct;
            if (product.SellerId != sellerId) return CreateProductDiscountResult.NotFoundForSeller;
            try
            {
                var newDiscount = new ProductDiscount()
                {
                    ProductId = createProductDiscount.ProductId,
                    DiscountNumber = createProductDiscount.DiscountNumber,
                    Percentage = createProductDiscount.Percentage,
                    ExpierDate = createProductDiscount.ExpierDate.ToMiladiDateTime()
                };
                await _discountRepository.AddProductDiscount(newDiscount);
                await _discountRepository.Save();
                return CreateProductDiscountResult.Success;
            }
            catch
            {
                return CreateProductDiscountResult.Error;
            }
        }
        #endregion

        #region GetAlloffProducs
        public async Task<List<ProductDiscount>> GetAlloffProducs(int take)
        {
            return await _discountRepository.GetAlloffProducs(take);
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _discountRepository.DisposeAsync();
            await _discountUseRepository.DisposeAsync(); ;
        }
        #endregion
    }
}
