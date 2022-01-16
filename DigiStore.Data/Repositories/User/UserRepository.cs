using System;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.User;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public UserRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region IsExistsUserByEmail
        public async Task<bool> IsExistsUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }
        #endregion

        #region IsExistsUserByMobile
        public async Task<bool> IsExistsUserByMobile(string mobile)
        {
            return await _context.Users.AnyAsync(u => u.Mobile == mobile);
        }
        #endregion

        #region IsExistsUserByUserName
        public async Task<bool> IsExistsUserByUserName(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }
        #endregion

        #region GetUserByEmail

        public async Task<Domain.Entities.User> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }
        #endregion

        #region IsExistsUserByActiveCode
        public async Task<bool> IsExistsUserByActiveCode(string activeCode)
        {
            return await _context.Users.AnyAsync(u => u.ActiveCode == activeCode);
        }
        #endregion

        #region GetUserByActiveCode
        public async Task<Domain.Entities.User> GetUserByActiveCode(string activeCode)
        {
            return await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == activeCode);
        }
        #endregion

        #region GetUserByActiveCode
        public void EditUser(Domain.Entities.User user)
        {
            user.ModifiedDate=DateTime.Now;
            _context.Users.Update(user);
        }
        #endregion

        #region GetUserByMobile
        public async Task<Domain.Entities.User> GetUserByMobile(string mobile)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Mobile == mobile);
        }
        #endregion

        #region GetUserById
        public async Task<Domain.Entities.User> GetUserById(int userId)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
        }
        #endregion

        #region AddUser
        public async Task AddUser(Domain.Entities.User user)
        {
            await _context.AddAsync(user);
        }
        #endregion

        #region FilterUser
        public async Task<FilterUserViewModel> FilterUserTask(FilterUserViewModel filterUser)
        {
            var user = _context.Users.AsQueryable();

            #region Order
            switch (filterUser.UserOrderBy)
            {
                case FilterUserOrderBy.Create_Date_Asc:
                    user = user.OrderBy(p => p.CreateDate);
                    break;

                case FilterUserOrderBy.Create_Date_Desc:
                    user = user.OrderByDescending(p => p.CreateDate);
                    break;
            }
            #endregion

            #region filter
            if (filterUser.Users != null)
                user = user.Where(p => EF.Functions.Like(p.UserName, $"%{filterUser.UserName}%"));
            if (filterUser.Email != null)
                user = user.Where(p => EF.Functions.Like(p.Email, $"%{filterUser.Email}%"));
            if (filterUser.Mobile != null)
                user = user.Where(p => EF.Functions.Like(p.Mobile, $"%{filterUser.Mobile}%"));
            #endregion

            #region State
            switch (filterUser.UserState)
            {
                case UserState.All:
                    break;
                case UserState.Active:
                    user = user.Where(p => p.IsActive);
                    break;
                case UserState.NotActive:
                    user = user.Where(p => !p.IsActive);
                    break;
                case UserState.IsDelete:
                    user = user.Where(p => p.IsDelete);
                    break;
                case UserState.NotDelete:
                    user = user.Where(p => !p.IsDelete);
                    break;
                case UserState.IsBolick:
                    user = user.Where(p => p.IsBlock);
                    break;
                case UserState.NotBlock:
                    user = user.Where(p => !p.IsBlock);
                    break;
            }
            #endregion

            #region Paging
            var pager = Pager.Build(filterUser.PageId, await user.CountAsync(), filterUser.TakeEntity,
                filterUser.HowManyShowPageAfterAndBefore);
            var allProduct = user.Paging(pager).ToList();
            return filterUser.SetPaging(pager).SetUser(allProduct);

            #endregion
        }
        #endregion

        #region GetUserByUserName
        public async Task<Domain.Entities.User> GetUserByUserName(string userName)
        {
            return await _context.Users.SingleOrDefaultAsync(p => p.UserName == userName);
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
