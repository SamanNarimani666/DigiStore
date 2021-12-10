using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Address;
using DigiStore.Domain.ViewModels.Address;
using DigiStore.Domain.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Address
{
    public class AddressRepository :IAddressRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public AddressRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddAddress
        public async Task AddAddress(Domain.Entities.Address address)
        {
            await _context.Addresses.AddAsync(address);
        }
        #endregion

        #region GetTicketById
        public async Task<Domain.Entities.Address> GetTicketById(int addressId)
        {
            return await _context.Addresses.FindAsync(addressId);
        }
        #endregion

        #region EditAddress
        public void EditAddress(Domain.Entities.Address address)
        {
            _context.Addresses.Update(address);
        }
        #endregion

        #region FilterAddress
        public async Task<FilterAddressVieweModel> FilterAddress(FilterAddressVieweModel filterAddress)
        {
            var adddress = _context.Addresses
                .Where(a=>a.IsDelete==false)
                .OrderByDescending(a=>a.ModifiedDate)
                .AsQueryable();

            if (filterAddress.UserId != null)
                adddress = adddress.Where(a => a.UserId == filterAddress.UserId);

            var pager = Pager.Build(filterAddress.PageId, await adddress.CountAsync(), filterAddress.TakeEntity,
                filterAddress.HowManyShowPageAfterAndBefore);
            var allTickets = adddress.Paging(pager).ToList();
            return filterAddress.SetPaging(pager).SetAddress(allTickets);
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
