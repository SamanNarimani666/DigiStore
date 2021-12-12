using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Enums.Store;
using DigiStore.Domain.IRepositories.Seller;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.Seller;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Seller
{
    public class SellerRepository : ISellerRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;

        public SellerRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddSeller
        public async Task AddSeller(Domain.Entities.Seller seller)
        {
            await _context.Sellers.AddAsync(seller);
        }
        #endregion

        #region HasHanderprograssRequestUser
        public async Task<bool> HasHanderprograssRequestUser(int userId)
        {
            return await _context.Sellers.AnyAsync(s => s.UserId == userId && s.StoreaceptanceState == (byte)StoreAcceptanceState.UnderProgress);
        }
        #endregion

        #region FilterSeller
        public async Task<FilterSellerViewModel> FilterSeller(FilterSellerViewModel filterSeller)
        {
            var seller = _context.Sellers
                .Include(s => s.User)
                .AsQueryable();

            #region State
            switch (filterSeller.State)
            {
                case FilterSellerState.All:
                    seller = seller.Where(s => !s.IsDelete);
                    break;

                case FilterSellerState.UnderProgress:
                    seller = seller.Where(s => s.StoreaceptanceState == (byte)StoreAcceptanceState.UnderProgress && !s.IsDelete);
                    break;
                case FilterSellerState.Accepted:
                    seller = seller.Where(s => s.StoreaceptanceState == (byte)StoreAcceptanceState.Accepted && !s.IsDelete);
                    break;
                case FilterSellerState.Rejected:
                    seller = seller.Where(s => s.StoreaceptanceState == (byte)StoreAcceptanceState.Rejected && !s.IsDelete);
                    break;
            }
            #endregion

            #region filter
            if (filterSeller.UserId != null && filterSeller.UserId != 0)
                seller = seller.Where(s => s.UserId == filterSeller.UserId);
            if (!string.IsNullOrEmpty(filterSeller.StoreName))
                seller = seller.Where(s => EF.Functions.Like(s.StoreName, $"%{filterSeller.StoreName}%"));
            if (!string.IsNullOrEmpty(filterSeller.Phone))
                seller = seller.Where(s => EF.Functions.Like(s.Phone, $"%{filterSeller.Phone}%"));
            if (!string.IsNullOrEmpty(filterSeller.Email))
                seller = seller.Where(s => EF.Functions.Like(s.Email, $"%{filterSeller.Email}%"));
            if (!string.IsNullOrEmpty(filterSeller.Mobile))
                seller = seller.Where(s => EF.Functions.Like(s.Mobile, $"%{filterSeller.Mobile}%"));
            if (!string.IsNullOrEmpty(filterSeller.Address))
                seller = seller.Where(s => EF.Functions.Like(s.Address, $"%{filterSeller.Address}%"));
            var pager = Pager.Build(filterSeller.PageId, await seller.CountAsync(), filterSeller.TakeEntity,
            #endregion

            #region Paging
            filterSeller.HowManyShowPageAfterAndBefore);
            var allTickets = seller.Paging(pager).ToList();
            return filterSeller.SetPaging(pager).SetSellers(allTickets);
            #endregion
        }
        #endregion

        #region GetSellerById
        public async Task<Domain.Entities.Seller> GetSellerById(int sellerId)
        {
            return await _context.Sellers.FirstOrDefaultAsync(s => s.SellerId == sellerId);
        }
        #endregion

        #region EditSeller
        public void EditSeller(Domain.Entities.Seller seller)
        {
            _context.Sellers.Update(seller);
        }
        #endregion

        #region GetLastActiveSellerByUserId
        public async Task<Domain.Entities.Seller> GetLastActiveSellerByUserId(int userId)
        {
            return await _context.Sellers.OrderByDescending(s => s.CreatedDate)
                .FirstOrDefaultAsync(s =>
                    s.UserId == userId && s.StoreaceptanceState == (byte) StoreAcceptanceState.Accepted);
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
