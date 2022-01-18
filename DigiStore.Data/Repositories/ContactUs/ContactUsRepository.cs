using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.ContactUs;

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
