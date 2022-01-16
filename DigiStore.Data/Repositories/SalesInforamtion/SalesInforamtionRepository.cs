using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.SalesInforamtion;

namespace DigiStore.Data.Repositories.SalesInforamtion
{
    public class SalesInforamtionRepository: ISalesInforamtionRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public SalesInforamtionRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddSalesInforamtion
        public async Task AddSalesInforamtion(Domain.Entities.SalesInforamtion salesInforamtion)
        {
            await _context.SalesInforamtions.AddAsync(salesInforamtion);
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DisposeAsync
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
