using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.User;
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
