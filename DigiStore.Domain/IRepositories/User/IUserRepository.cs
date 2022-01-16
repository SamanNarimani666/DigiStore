using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.User;

namespace DigiStore.Domain.IRepositories.User
{
    public interface IUserRepository:IAsyncDisposable
    {
        Task<bool> IsExistsUserByEmail(string email);
        Task<bool> IsExistsUserByMobile(string mobile);
        Task<bool> IsExistsUserByUserName(string userName);
        Task<Entities.User> GetUserByEmail(string email);
        Task<bool> IsExistsUserByActiveCode(string activeCode);
        Task<Entities.User> GetUserByActiveCode(string activeCode);
        void EditUser(Entities.User user);
        Task<Entities.User> GetUserByMobile(string mobile);
        Task<Entities.User> GetUserById(int userId);
        Task Save();
        Task AddUser(Entities.User user);
        Task<FilterUserViewModel> FilterUserTask(FilterUserViewModel filterUser);
        Task<Entities.User> GetUserByUserName(string userName);

    }
}
