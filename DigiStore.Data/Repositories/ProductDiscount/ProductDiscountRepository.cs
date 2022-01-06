using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductDiscount;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.ProductDiscount;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.ProductDiscount
{
    public class ProductDiscountRepository: IProductDiscountRepository
    {
        #region Contructor
        private readonly DigiStore_DBContext _context;
        public ProductDiscountRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region FilterProductDiscount
        public async Task<FilterProductDiscountViewModel> FilterProductDiscount(FilterProductDiscountViewModel filterProductDiscount)
        {
            var query =  _context.ProductDiscounts.Include(p=>p.Product).OrderByDescending(p=>p.ModifiedDate).AsQueryable();

            #region Filter
            if (filterProductDiscount.ProductId != null && filterProductDiscount.ProductId!=0)
            {
                query = query.Where(p => p.ProductId == filterProductDiscount.ProductId.Value);
            }
            if (filterProductDiscount.SellerId != null && filterProductDiscount.SellerId != 0)
            {
                query = query.Where(p => p.Product.SellerId==filterProductDiscount.SellerId.Value);
            }
            #endregion

            #region Paging
            var pager = Pager.Build(filterProductDiscount.PageId, await query.CountAsync(), filterProductDiscount.TakeEntity,
                filterProductDiscount.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filterProductDiscount.SetPaging(pager).SetDiscount(allProduct);
            #endregion
        }
        #endregion

        #region AddProductDiscount
        public async Task AddProductDiscount(Domain.Entities.ProductDiscount productDiscount)
        {
            await _context.AddAsync(productDiscount);
        }
        #endregion

        #region GetProductDiscountByProductId
        public async Task<Domain.Entities.ProductDiscount> GetProductDiscountByProductId(int productId)
        {
            return await _context.ProductDiscounts.Include(p => p.ProductDiscountUses)
                .OrderByDescending(p=>p.ModifiedDate)
                .FirstOrDefaultAsync(p =>
                    p.ProductId == productId && p.DiscountNumber.Value - p.ProductDiscountUses.Count > 0);

        }
        #endregion

        #region GetAlloffProducs
        public async Task<List<Domain.Entities.ProductDiscount>> GetAlloffProducs(int take)
        {
            return await _context.ProductDiscounts
                .Include(p=>p.Product)
                .Where(p => p.ExpierDate >= DateTime.Now)
                .OrderByDescending(p => p.ExpierDate)
                .Skip(0)
                .Take(take)
                .Distinct()
                .ToListAsync();
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
            await _context.DisposeAsync();
        }
        #endregion
    }
}
