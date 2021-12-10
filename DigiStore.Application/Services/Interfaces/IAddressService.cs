using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Address;

namespace DigiStore.Application.Services.Interfaces
{
   public interface IAddressService:IAsyncDisposable
   {
       Task<AddAddressResult> AddAddressUser(AddAddressViewModel addAddress,int userId);
       Task<FilterAddressVieweModel> FilterAddress(FilterAddressVieweModel filterAddress);
       Task<EditAddressResult> EditAddress(EditAddressViewModel editAddress, int userId);
       Task<bool> DeleteAddress(int addressId,int userId);
   }
}
