using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.ContactUs;
using DigiStore.Domain.ViewModels.Contacts;
using DigiStore.Domain.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.ContactUs
{
    public class ContactUsRepository : IContactUsRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ContactUsRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddContactUs
        public async Task AddContactUs(ContactU contactU)
        {
            await _context.ContactUs.AddAsync(contactU);
        }
        #endregion

        #region FilterContactUs
        public async Task<FilterContactUsViewModel> FilterContactUs(FilterContactUsViewModel filterContactUs)
        {
            var query = _context.ContactUs.OrderByDescending(p=>p.ModifiedDate).AsQueryable();

            #region Filter
            if (!string.IsNullOrEmpty(filterContactUs.Email))
                query = query.Where(p => EF.Functions.Like(p.Email, $"%{filterContactUs.Email}%"));
            #endregion

            #region paging
            var pager = Pager.Build(filterContactUs.PageId, await query.CountAsync(), filterContactUs.TakeEntity,
                filterContactUs.HowManyShowPageAfterAndBefore);
            var allProduct = query.Paging(pager).ToList();
            return filterContactUs.SetPaging(pager).SetContactUs(allProduct);

            #endregion
        }
        #endregion

        #region GetContactUById
        public async Task<ContactU> GetContactUById(int contactUId)
        {
            return await _context.ContactUs.SingleOrDefaultAsync(p => p.ContactUsid == contactUId);
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
