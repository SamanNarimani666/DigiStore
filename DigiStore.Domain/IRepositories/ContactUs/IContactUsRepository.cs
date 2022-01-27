using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Contacts;

namespace DigiStore.Domain.IRepositories.ContactUs
{
    public interface IContactUsRepository:IAsyncDisposable
    {
        Task AddContactUs(ContactU contactU);
        Task<FilterContactUsViewModel> FilterContactUs(FilterContactUsViewModel filterContactUs);
        Task<ContactU> GetContactUById(int contactUId);
        Task Save();
    }
}
