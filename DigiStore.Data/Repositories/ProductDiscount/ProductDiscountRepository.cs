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
            var query =  _context.ProductDiscounts.Include(p=>p.Product).AsQueryable();

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
