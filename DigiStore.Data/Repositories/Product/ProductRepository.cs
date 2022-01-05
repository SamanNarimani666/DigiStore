using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Enums.Product;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region FilterProduct
        public async Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct)
        {
            var product = _context.Products
                .Include(p => p.Seller)
                .Include(p => p.ProductSelectedCategories)
                .ThenInclude(p => p.ProductCategory)
                .AsQueryable();

            #region State
            switch (filterProduct.FilterProductState)
            {
                case FilterProductState.All:
                    break;
                case FilterProductState.Active:
                    product = product.Where(s => s.IsActive && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.NotActive:
                    product = product.Where(s => !s.IsActive && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Accepted:
                    product = product.Where(s =>s.IsActive &&s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Rejected:
                    product = product.Where(s => s.ProductAcceptanceState == (byte)ProductAcceptanceState.Rejected);
                    break;
                case FilterProductState.UnderProgress:
                    product = product.Where(s => s.ProductAcceptanceState == (byte)ProductAcceptanceState.UnderProgress);
                    break;
            }
            #endregion

            #region filter
            if (!string.IsNullOrEmpty(filterProduct.Name))
                product = product.Where(p => EF.Functions.Like(p.Name, ($"%{filterProduct.Name}%")));
            if (filterProduct.SellerId != null && filterProduct.SellerId != 0)
                product = product.Where(p => p.SellerId == filterProduct.SellerId.Value);
            if (!string.IsNullOrEmpty(filterProduct.Category))
                product = product.Where(s =>
                    s.ProductSelectedCategories.Any(f => f.ProductCategory.UrlName == filterProduct.Category));
            if (filterProduct.Selectedbrands != 0 && filterProduct.Selectedbrands != null)
                product = product.Where(p => p.BrandId == filterProduct.Selectedbrands);
            
            #endregion

            #region Order

            switch (filterProduct.FilterProductOrderBy)
            {
                case FilterProductOrderBy.Create_Date_Asc:
                    product = product.OrderBy(p => p.CreatedDate);
                    break;
                case FilterProductOrderBy.Create_Date_Desc:
                    product = product.OrderByDescending(p => p.CreatedDate);
                    break;
                case FilterProductOrderBy.Price_Asc:
                    product = product.OrderBy(p => p.Price);
                    break;
                case FilterProductOrderBy.Price_Desc:
                    product = product.OrderByDescending(p => p.Price);
                    break;
            }

            #endregion

            var pager = Pager.Build(filterProduct.PageId, await product.CountAsync(), filterProduct.TakeEntity,
                filterProduct.HowManyShowPageAfterAndBefore);
            var allProduct = product.Paging(pager).ToList();
            return filterProduct.SetPaging(pager).SetProduct(allProduct);
        }
        #endregion

        #region AddProduct
        public async Task AddProduct(Domain.Entities.Product product)
        {
            await _context.Products.AddAsync(product);
        }
        #endregion

        #region GetProductById
        public async Task<Domain.Entities.Product> GetProductById(int productId)
        {
            return await _context.Products.SingleOrDefaultAsync(p => p.ProductId == productId);
        }
        #endregion

        #region EditProduct
        public void EditProduct(Domain.Entities.Product product)
        {
            product.ModifiedDate = DateTime.Now;
            _context.Products.Update(product);
        }
        #endregion

        #region GetProductWithSellerById
        public async Task<Domain.Entities.Product> GetProductWithSellerById(int productId)
        {
            return await _context.Products.AsQueryable()
                .Include(p => p.Seller)
                .SingleOrDefaultAsync(p => p.ProductId == productId);
        }
        #endregion

        #region GetProductBySellerOwnerId
        public async Task<Domain.Entities.Product> GetProductBySellerOwnerId(int productId, int userId)
        {
            return await _context.Products.AsQueryable()
                .Include(p => p.Seller)
                .SingleOrDefaultAsync(p => p.ProductId == productId && p.Seller.UserId == userId);
        }
        #endregion

        #region GetProductDetail
        public async Task<ProductDetailViewModel> GetProductDetail(int productId)
        {
            var product = await _context.Products.AsQueryable()
                .Include(p=>p.ProductGalleries)
                .Include(p=>p.Guarantees)
                .Include(p => p.Colors)
                .Include(p=>p.ProductGalleries)
                .Include(p=>p.ProductSelectedCategories)
                .Include(p=>p.ProductFeatures)
                .Include(p => p.Seller)
                .ThenInclude(p => p.User)
                .Include(p => p.ProductSelectedCategories)
                .ThenInclude(p => p.ProductCategory)
                .SingleOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) return null;
            var selectedCategotiesId = product.ProductSelectedCategories.Select(f => f.ProductCategoryId).ToList();
            return new ProductDetailViewModel()
            {
                ProductId= product.ProductId,
                Title = product.Name,
                ImageName = product.ImageName,
                Price = product.Price,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                ProductCategories = product.ProductSelectedCategories.Select(s=>s.ProductCategory).ToList(),
                ProductGalleries = product.ProductGalleries.ToList(),
                ProductColors = product.Colors.ToList(),
                SellerId = product.SellerId,
                Seller = product.Seller,
                Guarantees = product.Guarantees.ToList(),
                ProductFeatures = product.ProductFeatures.ToList(),
                RelatedProducts = await _context.Products.AsQueryable()
                    .Include(s=>s.ProductSelectedCategories)
                    .Where(s=>s.ProductSelectedCategories.Any(f=>selectedCategotiesId.Contains(f.ProductCategoryId))&& s.ProductId!=productId&& s.ProductAcceptanceState==(byte)ProductAcceptanceState.Accepted).ToListAsync()
            };
        }
        #endregion

        #region FilterProductsForSellerByPorductName
        public async Task<List<Domain.Entities.Product>> FilterProductsForSellerByPorductName(int selleId, string productName)
        {
            return await _context.Products.AsQueryable().Where(p=>p.SellerId==selleId&& EF.Functions.Like(p.Name, $"%{productName}%")).ToListAsync();
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        #endregion
    }
}
