using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Application.Security;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Address;
using DigiStore.Domain.IRepositories.AddressCity;
using DigiStore.Domain.IRepositories.AddressState;
using DigiStore.Domain.ViewModels.Address;

namespace DigiStore.Application.Services.Implementations
{
    public class AddressService : IAddressService
    {
        #region Constructor
        private readonly IAddressRepository _addressRepository;
        private readonly IStateRepository _stateRepository;
        private readonly ICityRepository _cityRepository;
        private IAddressService _addressServiceImplementation;

        public AddressService(IAddressRepository addressRepository, IStateRepository stateRepository, ICityRepository cityRepository)
        {
            _addressRepository = addressRepository;
            _stateRepository = stateRepository;
            _cityRepository = cityRepository;
        }
        #endregion

        #region GetAllState
        public async Task<List<State>> GetAllState()
        {
            return await _stateRepository.GetAllState();
        }
        #endregion

        #region GetListCityByStateId
        public async Task<List<City>> GetListCityByStateId(int stateId)
        {
            return await _cityRepository.GetListCityByStateId(stateId);
        }
        #endregion

        #region GetStateById
        public async Task<State> GetStateById(int stateId)
        {
            return await _stateRepository.GetStateById(stateId);
        }
        #endregion

        #region AddAddressUser
        public async Task<AddAddressResult> AddAddressUser(AddAddressViewModel addAddress, int userId)
        {
            try
            {
                var address = new Address
                {
                    UserId = userId,
                    CityId = addAddress.CityId,
                    PostalCode = addAddress.PostalCode.SanitizeText(),
                    StateId = addAddress.StateId,
                    Zipcode = addAddress.Zipcode.SanitizeText(),
                    Address1 = addAddress.Address.SanitizeText(),
                    Unit = addAddress.Unit.SanitizeText()
                };
              await  _addressRepository.AddAddress(address);
              await _addressRepository.Save();
              return AddAddressResult.Success;
            }
            catch
            {
                return AddAddressResult.Error;
            }
        }
        #endregion

        #region FilterAddress
        public async Task<FilterAddressVieweModel> FilterAddress(FilterAddressVieweModel filterAddress)
        {
           return await _addressRepository.FilterAddress(filterAddress);
        }
        #endregion

        #region EditAddress
        public async Task<EditAddressResult> EditAddress(EditAddressViewModel editAddress, int userId)
        {
            var address = await _addressRepository.GetAddressById(editAddress.AddressId);
            if (address == null) return EditAddressResult.NotFound;
            if (address.UserId != userId) return EditAddressResult.NotFoundUser;
            try
            {
                address.Address1 = editAddress.Address.SanitizeText();
                address.Zipcode = editAddress.Zipcode.SanitizeText();
                address.PostalCode = editAddress.PostalCode.SanitizeText();
                address.StateId = editAddress.StateId;
                address.CityId = editAddress.CityId;
                address.Unit = editAddress.Unit.SanitizeText();
                _addressRepository.EditAddress(address);
                await _addressRepository.Save();
                return EditAddressResult.Success;
            }
            catch
            {
                return EditAddressResult.Error;
            }
        }
        #endregion

        #region DeleteAddress
        public async Task<bool> DeleteAddress(int addressId, int userId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address == null) return false;
            if (address.UserId != userId) return false;
            address.IsDelete = true;
            address.ModifiedDate=DateTime.Now;
            _addressRepository.EditAddress(address);
            await _addressRepository.Save();
            return true;
        }
        #endregion

        #region EditInfoAddress
        public async Task<EditAddressViewModel> EditInfoAddress(int userId, int addressId)
        {
            var address = await _addressRepository.GetAddressById(addressId);
            if (address.UserId != userId) return null;
            return new EditAddressViewModel()
            {
                AddressId = addressId,
                Address = address.Address1,
                StateId = address.StateId,
                Zipcode = address.Zipcode,
                PostalCode = address.PostalCode,
                CityId = address.CityId,
                Unit = address.Unit
            };
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _addressRepository.DisposeAsync();
            await _stateRepository.DisposeAsync();
            await _cityRepository.DisposeAsync();
        }
        #endregion
    }
}
