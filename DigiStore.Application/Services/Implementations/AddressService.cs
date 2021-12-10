using System;
using System.Threading.Tasks;
using DigiStore.Application.Security;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Address;
using DigiStore.Domain.ViewModels.Address;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Application.Services.Implementations
{
    public class AddressService : IAddressService
    {
        #region Constructor
        private readonly IAddressRepository _addressRepository;

        public AddressService(IAddressRepository addressRepository)
        {
            _addressRepository = addressRepository;
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
                    City = addAddress.City.SanitizeText(),
                    PostalCode = addAddress.PostalCode.SanitizeText(),
                    State = addAddress.State.SanitizeText(),
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
            var ticket = await _addressRepository.GetTicketById(editAddress.AddressId);
            if (ticket == null) return EditAddressResult.NotFound;
            if (ticket.UserId != userId) return EditAddressResult.NotFoundUser;
            try
            {
                ticket.Address1 = editAddress.Address.SanitizeText();
                ticket.Zipcode = editAddress.Zipcode.SanitizeText();
                ticket.PostalCode = editAddress.PostalCode.SanitizeText();
                ticket.State = editAddress.State.SanitizeText();
                ticket.City = editAddress.City.SanitizeText();
                ticket.Unit = editAddress.Unit.SanitizeText();
                _addressRepository.EditAddress(ticket);
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
            var address = await _addressRepository.GetTicketById(addressId);
            if (address == null) return false;
            if (address.UserId != userId) return false;
            address.IsDelete = true;
            address.ModifiedDate=DateTime.Now;
            _addressRepository.EditAddress(address);
            await _addressRepository.Save();
            return true;
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _addressRepository.DisposeAsync();
        }
        #endregion
    }
}
