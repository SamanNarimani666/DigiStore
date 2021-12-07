using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Address;
using DigiStore.Web.PresentationExtensions;

namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    [Route("Address")]
    [AutoValidateAntiforgeryToken]
    public class AddressController : UserBaseController
    {
        #region Constructor
        private readonly IAddressService _addressService;
        public AddressController(IAddressService addressService)
        {
            _addressService = addressService;
        }
        #endregion

        #region ListAddress
        [HttpGet("ListAddress")]
        public async Task<IActionResult> ListAddress(FilterAddressVieweModel filterAddress)
        {
            filterAddress.UserId = User.GetUserId();
            return View(await _addressService.FilterAddress(filterAddress));
        }
        #endregion

        #region AddAddress
        [HttpGet("AddAddress")]
        public IActionResult AddAddress() => PartialView();

        [HttpPost("AddAddress"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AddAddress(AddAddressViewModel addAddress)
        {
            if (ModelState.IsValid)
            {
                var res = await _addressService.AddAddressUser(addAddress, User.GetUserId());
                switch (res)
                {
                    case AddAddressResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت آدرس ";
                        break;
                    case AddAddressResult.Success:
                        TempData[SuccessMessage] = "آدرس با موفقیت ثبت شد";
                        return RedirectToAction("ListAddress", "Address", new { area = "UserPanel" });
                }
            }
            return PartialView(addAddress);
        }
        #endregion

      
    }
}
