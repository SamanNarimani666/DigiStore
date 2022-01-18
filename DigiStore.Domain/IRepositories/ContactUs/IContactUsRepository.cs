using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.ContactUs
{
    public interface IContactUsRepository:IAsyncDisposable
    {
        Task AddContactUs(ContactU contactU);
        Task Save();
    }
}
