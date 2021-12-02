using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Context.Entities;
using DigiStore.Domain.IRepositories.User;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.User
{
    public class UserRepository : IUserRepository
    {
        private readonly DigiStore_DBContext _context;

        public UserRepository(DigiStore_DBContext context)
        {
            _context = context;
        }


        public async Task<bool> IsExistsUserByEmail(string email)
        {
            return await _context.Users.AnyAsync(u => u.Email == email);
        }

        public async Task<bool> IsExistsUserByMobile(string mobile)
        {
            return await _context.Users.AnyAsync(u => u.Mobile == mobile);
        }

        public async Task<bool> IsExistsUserByUserName(string userName)
        {
            return await _context.Users.AnyAsync(u => u.UserName == userName);
        }

        public async Task<Domain.Entities.User> GetUserByEmail(string email)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Email == email);
        }

        public async Task<bool> IsExistsUserByActiveCode(string activeCode)
        {
           return await _context.Users.AnyAsync(u => u.ActiveCode == activeCode);
        }

        public async Task<Domain.Entities.User> GetUserByActiveCode(string activeCode)
        {
           return await _context.Users.FirstOrDefaultAsync(u => u.ActiveCode == activeCode);
        }

        public void EditUser(Domain.Entities.User user)
        {
            _context.Users.Update(user);
        }

        public async Task<Domain.Entities.User> GetUserByMobile(string mobile)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.Mobile == mobile);
        }

        public async Task<Domain.Entities.User> GetUserById(int userId)
        {
            return await _context.Users.SingleOrDefaultAsync(u => u.UserId == userId);
        }

        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }


        public async Task AddUser(Domain.Entities.User user)
        {
            await _context.AddAsync(user);
        }

        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
    }
}
