using System;
using System.Collections.Generic;
using System.Data;
using System.Linq;
using System.Threading.Tasks;
using Dapper;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.Enums.Product;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.Product;
using Microsoft.Data.SqlClient;
using Microsoft.EntityFrameworkCore;
using Microsoft.Extensions.Configuration;

namespace DigiStore.Data.Repositories.Product
{
    public class ProductRepository : IProductRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        private readonly IDbConnection db;
        public ProductRepository(DigiStore_DBContext context, IConfiguration configuration)
        {
            _context = context;
            this.db = new SqlConnection(configuration.GetConnectionString("DigiStoretConnectionStrings"));
        }
        #endregion

        #region FilterProduct
        public async Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct)
        {
            var product = _context.Products
                .Include(p => p.Seller)
                .Include(p => p.ProductSelectedCategories)
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
                    product = product.Where(s => s.IsActive && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Rejected:
                    product = product.Where(s => s.ProductAcceptanceState == (byte)ProductAcceptanceState.Rejected);
                    break;
                case FilterProductState.UnderProgress:
                    product = product.Where(s => s.ProductAcceptanceState == (byte)ProductAcceptanceState.UnderProgress);
                    break;
            }
            #endregion

            var selected = _context.ProductSelectedCategories.Include(p => p.ProductCategory).Select(p => p.ProductCategoryId).ToList();
            #region filter
            if (!string.IsNullOrEmpty(filterProduct.Name))
                product = product.Where(p => EF.Functions.Like(p.Name, ($"%{filterProduct.Name}%")));
            if (filterProduct.SellerId != null && filterProduct.SellerId != 0)
                product = product.Where(p => p.SellerId == filterProduct.SellerId.Value);
            if (filterProduct.CategoryId != 0)
                product = product.Where(s =>
                    s.ProductSelectedCategories.Any(f => f.ProductCategory.ProductCategoryId == filterProduct.CategoryId));
            if (filterProduct.Selectedbrands != 0)
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

            #region paging
            var pager = Pager.Build(filterProduct.PageId, await product.CountAsync(), filterProduct.TakeEntity,
                filterProduct.HowManyShowPageAfterAndBefore);
            var allProduct = product.Paging(pager).ToList();
            return filterProduct.SetPaging(pager).SetProduct(allProduct);

            #endregion
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
                .Include(p => p.ProductGalleries)
                .Include(p => p.Colors)
                .Include(p => p.ProductGalleries)
                .Include(p => p.ProductSelectedCategories)
                .Include(p => p.ProductFeatures)
                .Include(p => p.Seller)
                .Include(p => p.ProductSelectedCategories)
                .ThenInclude(p => p.ProductCategory)
                .SingleOrDefaultAsync(p => p.ProductId == productId);
            if (product == null) return null;
            var selectedCategotiesId = product.ProductSelectedCategories.Select(f => f.ProductCategoryId).ToList();
            return new ProductDetailViewModel()
            {
                ProductId = product.ProductId,
                Title = product.Name,
                ImageName = product.ImageName,
                Price = product.Price,
                ShortDescription = product.ShortDescription,
                Description = product.Description,
                ProductCategories = product.ProductSelectedCategories.Select(s => s.ProductCategory).ToList(),
                ProductGalleries = product.ProductGalleries.Where(p => !p.IsDelete).ToList(),
                ProductColors = product.Colors.Where(p => !p.IsDelete).ToList(),
                SellerId = product.SellerId,
                Seller = product.Seller,
                MainCategoryTitle = _context.ProductSelectedCategories.Include(p => p.ProductCategory)
                    .Where(p => p.ProductId == productId && p.ProductCategory.ParentId == null).Select(p => p.ProductCategory.Title).FirstOrDefault()
                    ?.ToString(),
                SubCategoryTitle = _context.ProductSelectedCategories.Include(p => p.ProductCategory)
                    .Where(p => p.ProductId == productId && p.ProductCategory.ParentId != null).Select(p => p.ProductCategory.Title).FirstOrDefault()
                    ?.ToString(),
                MainCategoryId = _context.ProductSelectedCategories.Include(p => p.ProductCategory)
                    .Where(p => p.ProductId == productId && p.ProductCategory.ParentId == null).Select(p => p.ProductCategory.ProductCategoryId).FirstOrDefault(),
                SubCategoryId = _context.ProductSelectedCategories.Include(p => p.ProductCategory)
                    .Where(p => p.ProductId == productId && p.ProductCategory.ParentId != null).Select(p => p.ProductCategory.ProductCategoryId).FirstOrDefault(),
                ProductFeatures = product.ProductFeatures.ToList(),
                RelatedProducts = await _context.Products.AsQueryable()
                    .Include(s => s.ProductSelectedCategories)
                    .Where(s => s.ProductSelectedCategories.Any(f => selectedCategotiesId.Contains(f.ProductCategoryId)) && s.ProductId != productId && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted).ToListAsync()
            };
        }
        #endregion

        #region FilterProductsForSellerByPorductName
        public async Task<List<Domain.Entities.Product>> FilterProductsForSellerByPorductName(int selleId, string productName)
        {
            return await _context.Products.AsQueryable().Where(p => p.SellerId == selleId && EF.Functions.Like(p.Name, $"%{productName}%")).ToListAsync();
        }
        #endregion

        #region GetPopularProduct
        public async Task<List<Domain.Entities.Product>> GetPopularProduct(int take)
        {
            return await _context.Products.Include(p => p.SalesOrderDetails)
                .Where(p => p.SalesOrderDetails.Any() && p.IsActive && p.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted)
                .OrderByDescending(d => d.SalesOrderDetails.Sum(c => c.OrderQty))
                .Take(take)
                .ToListAsync();
        }
        #endregion

        #region GetMostPopular
        public async Task<List<Domain.Entities.Product>> GetMostPopular(int take)
        {
            return await _context.Products.Include(p => p.FavoriteProductUsers)
                .Where(p => p.FavoriteProductUsers.Any() && p.IsActive && p.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted)
                .OrderByDescending(f => f.FavoriteProductUsers.Count)
                .Take(take)
                .ToListAsync();
        }
        #endregion

        #region RecommendedproductsForUser
        public async Task<List<Domain.Entities.Product>> RecommendedproductsForUser(int take, int userId)
        {

            var getLastProductVisitedByUser = await _context.ProductVisiteds.OrderByDescending(p => p.ModifiedDate)
                .FirstOrDefaultAsync(p => p.UserId == userId);

            if (getLastProductVisitedByUser == null) return null;

            var product = await _context.Products.Include(p => p.ProductSelectedCategories)
                .SingleOrDefaultAsync(p => p.ProductId == getLastProductVisitedByUser.ProductId);

            var selectedCategotiesId = product.ProductSelectedCategories
                .Select(f => f.ProductCategoryId).ToList();

            return await _context.Products.AsQueryable()
                .Include(s => s.ProductSelectedCategories)
                .Where(s => s.ProductSelectedCategories.Any(f => selectedCategotiesId.Contains(f.ProductCategoryId)) &&
                      s.IsActive && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted).ToListAsync();
        }
        #endregion

        #region TheMostVisitedProducts
        public async Task<List<Domain.Entities.Product>> TheMostVisitedProducts(int take)
        {
            return await _context.Products.Include(p => p.ProductVisiteds)
                .Where(p => p.ProductVisiteds.Any() && p.IsActive && p.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted)
                .OrderByDescending(p => p.ProductVisiteds.Count)
                .Take(take)
                .ToListAsync();
        }
        #endregion

        #region GetAllActiveProductByCategoryId
        public async Task<List<Domain.Entities.Product>> GetAllActiveProductByCategoryId(int categoryId, int count)
        {
            return await _context.ProductSelectedCategories.AsQueryable()
                .Include(p => p.Product)
                .Where(p => p.ProductCategoryId == categoryId && p.Product.IsActive && p.Product.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted)
                .OrderByDescending(s => s.ModifiedDate)
                .Distinct()
                .Select(s => s.Product).Distinct()
                .Take(count)
                .ToListAsync();
        }
        #endregion

        #region FilterForSiteSearch
        public async Task<FilterProductViewModel> FilterForSiteSearch(FilterProductViewModel filterProduct)
        {
            var query =
                @"SELECT   PP.[Name] ,PP.Price ,PP.ImageName,PP.ProductId,PP.SellerId,PP.CreatedDate,SS.StoreName FROM Production.Product AS PP INNER JOIN Store.Seller AS SS ON PP.SellerId=SS.SellerId INNER JOIN Production.ProductSelectedCategory AS PSC ON PP.ProductId=PSC.ProductId WHERE PP.ProductAcceptanceState=1 AND PP.IsActive=1 AND PP.IsDelete=0 ";

            if (!string.IsNullOrEmpty(filterProduct.Name))
            {
                query += $"AND PP.[Name] LIKE N'%{filterProduct.Name}%'";
            }

            if (filterProduct.Selectedbrand != null && filterProduct.Selectedbrand.Any())
            {
                query += "AND ";
                if (filterProduct.Selectedbrand.Count == 1)
                {
                    query += $"PP.BrandId={filterProduct.Selectedbrand.First()}";
                }
                else
                {
                    query += "( ";
                    byte iteration = 0;
                    foreach (var brands in filterProduct.Selectedbrand)
                    {
                        iteration++;
                        if (iteration < filterProduct.Selectedbrand.Count)
                        {
                            query += $"PP.BrandId={brands} OR ";

                        }
                        else
                        {
                            query += $"PP.BrandId={brands} ";

                        }
                    }

                    query += " )";
                }
            }

            if (filterProduct.SelectedPrductCategories != null && filterProduct.SelectedPrductCategories.Any())
            {
                query += "AND ";

                if (filterProduct.SelectedPrductCategories.Count == 1)
                {
                    query += $"PSC.ProductCategoryId={filterProduct.SelectedPrductCategories.First()}";
                }
                else
                {
                    query += "( ";
                    byte iteration = 0;
                    foreach (var category in filterProduct.SelectedPrductCategories)
                    {
                        iteration++;
                        if (iteration < filterProduct.SelectedPrductCategories.Count)
                        {
                            query += $"PSC.ProductCategoryId={category} OR ";

                        }
                        else
                        {
                            query += $"PSC.ProductCategoryId={category} ";

                        }
                    }
                    query += " )";

                }
            }

            query += "GROUP BY PP.[Name] ,PP.Price ,PP.ImageName ,PP.ProductId,PP.SellerId,PP.CreatedDate,SS.StoreName  ";

            switch (filterProduct.FilterProductOrderBy)
            {
                case FilterProductOrderBy.Create_Date_Asc:
                    query += "ORDER BY  PP.CreatedDate DESC";
                    break;
                case FilterProductOrderBy.Create_Date_Desc:
                    query += "ORDER BY  PP.CreatedDate ASC";
                    break;
                case FilterProductOrderBy.Price_Asc:
                    query += "ORDER BY  PP.Price ASC";
                    break;
                case FilterProductOrderBy.Price_Desc:
                    query += "ORDER BY  PP.Price DESC";
                    break;
            }

            var product = await db.QueryAsync<Domain.Entities.Product,Domain.Entities.Seller, Domain.Entities.Product>(query,(e,c)=>
            {
                e.Seller = c;
                return e;
            },splitOn: "SellerId");

            #region paging
            var pager = Pager.Build(filterProduct.PageId,  product.Count(), filterProduct.TakeEntity,
                filterProduct.HowManyShowPageAfterAndBefore);
            var allProduct = product.Paging(pager).ToList();
            return filterProduct.SetPaging(pager).SetProduct(allProduct);

            #endregion
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
