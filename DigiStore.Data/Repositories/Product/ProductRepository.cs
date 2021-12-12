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
    public class ProductRepository: IProductRepository
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
            var product = _context.Products.AsQueryable();

            #region State
            switch (filterProduct.FilterProductState)
            {
                case FilterProductState.Active:
                    product = product.Where(s => s.IsActive && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.NotActive:
                    product = product.Where(s => !s.IsActive && s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
                    break;
                case FilterProductState.Accepted:
                    product = product.Where(s => s.ProductAcceptanceState == (byte)ProductAcceptanceState.Accepted);
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
            if (filterProduct.SellerId != null || filterProduct.SellerId != 0)
                product = product.Where(p => p.SellerId == filterProduct.SellerId);
            #endregion

            var pager = Pager.Build(filterProduct.PageId, await product.CountAsync(), filterProduct.TakeEntity,
                filterProduct.HowManyShowPageAfterAndBefore);
            var allProduct = product.Paging(pager).ToList();
            return filterProduct.SetPaging(pager).SetProduct(allProduct);
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
