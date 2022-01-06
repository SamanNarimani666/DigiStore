
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.FavoriteProductUser;

namespace DigiStore.Data.Repositories.FavoriteProductUser
{
    public class FavoriteProductUserRepository: IFavoriteProductUserRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public FavoriteProductUserRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddFavoriteProductUser
        public async Task AddFavoriteProductUser(Domain.Entities.FavoriteProductUser favoriteProductUser)
        {
            await _context.FavoriteProductUsers.AddAsync(favoriteProductUser);
        }
        #endregion

        #region UpdateFavoriteProductUser
        public void UpdateFavoriteProductUser(Domain.Entities.FavoriteProductUser favoriteProductUser)
        {
            _context.FavoriteProductUsers.Update(favoriteProductUser);
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
