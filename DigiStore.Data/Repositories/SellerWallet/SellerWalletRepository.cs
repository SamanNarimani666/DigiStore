using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.SellerWallet;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.SellerWallet;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.SellerWallet
{
    public class SellerWalletRepository : ISellerWalletRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SellerWalletRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddSellerWallet
        public async Task AddSellerWallet(Domain.Entities.SellerWallet sellerWallet)
        {
            await _context.SellerWallets.AddAsync(sellerWallet);
        }
        #endregion

        #region FilterSellerWallet
        public async Task<FilterSellerWalletViewModel> FilterSellerWallet(FilterSellerWalletViewModel filterSellerWallet)
        {
            var sellerWallet = _context.SellerWallets.AsQueryable();

            #region Filter
            if (filterSellerWallet.SellerId != null && filterSellerWallet.SellerId != 0)
            {
                sellerWallet = sellerWallet.Where(s => s.SellerId == filterSellerWallet.SellerId.Value);
            }

            if (filterSellerWallet.PriceFrom != null)
            {
                sellerWallet = sellerWallet.Where(s => s.Price >= filterSellerWallet.PriceFrom.Value);
            }
            if (filterSellerWallet.PriceTo != null)
            {
                sellerWallet = sellerWallet.Where(s => s.Price <= filterSellerWallet.PriceTo.Value);
            }
            #endregion

            #region Paging
            var pager = Pager.Build(filterSellerWallet.PageId, await sellerWallet.CountAsync(), filterSellerWallet.TakeEntity,
                filterSellerWallet.HowManyShowPageAfterAndBefore);
            var allTickets = sellerWallet.Paging(pager).ToList();
            return filterSellerWallet.SetPaging(pager).SetSellers(allTickets);
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
