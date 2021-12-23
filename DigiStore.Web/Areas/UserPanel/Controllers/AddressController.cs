using Microsoft.AspNetCore.Mvc;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Address;
using DigiStore.Web.Http;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc.Rendering;

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

        #region GetCity
        [HttpGet("GetCity/{stateId}")]
        public async Task<IActionResult> GetCity(int stateId)
        {
            var cities = await _addressService.GetListCityByStateId(stateId);
            return Json(cities);
        }


        #endregion

        #region AddAddress

        [HttpGet("AddAddress")]
        public async Task<IActionResult> AddAddress()
        {
            ViewBag.State = await _addressService.GetAllState();
            return PartialView();
        }

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

        #region DeleteAddress
        public async Task<IActionResult> DeleteAddress(int id)
        {
            var result = await _addressService.DeleteAddress(id, User.GetUserId());
            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "آدرس مورد نظر با موفقیت ثبت حذف شد",
                    null
                );
            }
            return JsonResponseStatus.SendStatus(
                JsonResponseStatusType.Danger,
                "اطلاعاتی با این مشخصات یافت نشد",
                null
            );

        }
        #endregion

        #region EditAddress
        [HttpGet("EditAddress/{id}")]
        public async Task<IActionResult> EditAddress(int id)
        {
            var address = await _addressService.EditInfoAddress(User.GetUserId(), id);
            ViewBag.State = await _addressService.GetAllState();
            ViewBag.SelectedState = await _addressService.GetStateById(address.StateId);

            return PartialView(address);
        }
        [HttpPost("EditAddress/{id}")]
        public async Task<IActionResult> EditAddress(EditAddressViewModel editAddress)
        {
            if (ModelState.IsValid)
            {
                var res = await _addressService.EditAddress(editAddress, User.GetUserId());
                switch (res)
                {
                    case EditAddressResult.Error:
                        TempData[ErrorMessage] = "خطا در ویرایش آدرس ";
                        return RedirectToAction("ListAddress", "Address");
                    case EditAddressResult.NotFound:
                        TempData[ErrorMessage] = "آدرسی با این مشخصات یافت نشد";
                        return RedirectToAction("ListAddress", "Address");
                    case EditAddressResult.NotFoundUser:
                        TempData[ErrorMessage] = "کاربری با این مشخصات یافت نشد";
                        return RedirectToAction("ListAddress", "Address");
                    case EditAddressResult.Success:
                        TempData[SuccessMessage] = "آدرس شما با موفقیت ویرایش شد";
                        return RedirectToAction("ListAddress", "Address");
                }
            }
            ViewBag.State = await _addressService.GetAllState();
            ViewBag.SelectedState = await _addressService.GetStateById(editAddress.StateId);
            return PartialView(editAddress);
        }
        #endregion

    }
}
