using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Address;

namespace DigiStore.Domain.IRepositories.Address
{
    public interface IAddressRepository : IAsyncDisposable
    {
        Task AddAddress(Entities.Address address);
        Task<Entities.Address> GetAddressById(int addressId);
        void EditAddress(Entities.Address address);
        Task<FilterAddressVieweModel> FilterAddress(FilterAddressVieweModel filterAddress);
        Task Save();
    }
}
