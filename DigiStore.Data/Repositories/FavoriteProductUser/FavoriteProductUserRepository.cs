using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.FavoriteProductUser;
using DigiStore.Domain.ViewModels.FavoriteProductUser;
using DigiStore.Domain.ViewModels.Paging;
using Microsoft.EntityFrameworkCore;

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

        #region GetFavoriteProductUserById
        public async Task<Domain.Entities.FavoriteProductUser> GetFavoriteProductUserById(int favoritId)
        {
            return await _context.FavoriteProductUsers.FirstOrDefaultAsync(p => p.FavoriteProductUserId == favoritId);
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

        #region IsExistThisProductInUserFavoritList
        public async Task<bool> IsExistThisProductInUserFavoritList(int productId, int userId)
        {
            return await _context.FavoriteProductUsers.AnyAsync(p=>p.ProductId==productId&&p.UserId==userId&&!p.IsDelete);
        }
        #endregion

        #region GetFavoriteProductUserByUserId
        public async Task<FilterFavoritViewModel> GetFavoriteProductUserByUserId(FilterFavoritViewModel filterFavorit)
        {
            var product =  _context.FavoriteProductUsers.Include(p => p.Product)
                .Where(p => p.UserId == filterFavorit.UserId&& !p.IsDelete).OrderByDescending(p=>p.CreateDate);
            
            var pager = Pager.Build(filterFavorit.PageId, await product.CountAsync(), filterFavorit.TakeEntity,
                filterFavorit.HowManyShowPageAfterAndBefore);
           var allProduct = product.Paging(pager).ToList();
           return filterFavorit.SetPaging(pager).SetProduct(allProduct);
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
