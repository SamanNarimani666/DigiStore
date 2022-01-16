using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Address;

namespace DigiStore.Application.Services.Interfaces
{
   public interface IAddressService:IAsyncDisposable
   {
       Task<List<State>> GetAllState();
       Task<List<City>> GetListCityByStateId(int stateId);
       Task<State> GetStateById(int stateId);
       Task<AddAddressResult> AddAddressUser(AddAddressViewModel addAddress,int userId);
       Task<FilterAddressVieweModel> FilterAddress(FilterAddressVieweModel filterAddress);
       Task<EditAddressResult> EditAddress(EditAddressViewModel editAddress, int userId);
       Task<bool> DeleteAddress(int addressId,int userId);
       Task<EditAddressViewModel> EditInfoAddress(int userId,int addressId);
       Task<List<Domain.Entities.Address>> GetUserAddressByUserId(int userId);
   }
}
