using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Ticket;
using DigiStore.Web.Http;
using DigiStore.Web.PresentationExtensions;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    public class TicketController : AdminBaseController
    {
        #region Constructor
        private readonly ITicketService _ticketService;
        private readonly IUserService _userService;
        public TicketController(ITicketService ticketService, IUserService userService)
        {
            _ticketService = ticketService;
            _userService = userService;
        }
        #endregion

        #region filterTicket
        public async Task<IActionResult> filterTicket(FilterTicketViewModel filterTicket)
        {
            filterTicket.OrderBy = FilterTicketOrder.CreateDate_DES;
            var filter = await _ticketService.FilterTickets(filterTicket);
            return View(filter);
        }
        #endregion

        #region TicketDetails
        [HttpGet("tickets/{ticketId}/{userId}")]
        public async Task<IActionResult> TicketDetail(int ticketId,int userId)
        {
            var ticket = await _ticketService.TicketDetailsForShow(ticketId, userId);
            if (ticket == null) return NotFound();
            ViewBag.UserInfo = await _userService.GetUserByUserId(User.GetUserId());
            return View(ticket);
        }
        #endregion

        #region answer ticket
        [HttpPost("answer-ticket"), ValidateAntiForgeryToken]
        public async Task<IActionResult> AnswerTicket(AnswerTicketViewModel answer)
        {
            if (string.IsNullOrEmpty(answer.Text))
            {
                TempData[ErrorMessage] = "لطفا متن پیام خود را وارد نمایید";
            }

            if (ModelState.IsValid)
            {
                var res = await _ticketService.AnswerTicketForAdmin(answer, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("filterTicket");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("TicketDetail", "Ticket", new { area = "Admin", ticketId = answer.TicketId ,userId=answer.UserId});
        }
        #endregion

        #region DeleteTicketMessage
        [HttpPost("delete-ticketMessage")]
        public async Task<IActionResult> DeleteTicketMessage(DeleteTicketViewModel deleteTicket)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.DeleteTicket(deleteTicket, User.GetUserId());
                switch (res)
                {
                    case DeleteTicketResult.Error:
                        TempData[ErrorMessage] = "خطا در حذف پیام تیکت";
                        break;
                    case DeleteTicketResult.NotFoundTicket:
                        TempData[ErrorMessage] = "تیکتی با این مشخصات یافت نشد";
                        break;
                    case DeleteTicketResult.NotFoundTicketMessage:
                        TempData[ErrorMessage] = "پیامی یافت نشد";
                        break;
                    case DeleteTicketResult.NotFoundUser:
                        TempData[ErrorMessage] = "عدم دسترسی در صورت تکرار  حساب کاربری شما بلک خواهد شد";
                        break;
                    case DeleteTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت با موفقیت قبت شد";
                        break;
                }
            }
            return RedirectToAction("TicketDetail", "Ticket", new { area = "Admin", ticketId = deleteTicket.TicketId, userId = deleteTicket.UserId });
        }
        #endregion

        #region AcceptSellerRequest
        public async Task<IActionResult> ClosedTheTicket(int ticketId)
        {
            var result = await _ticketService.ClosedTheTicket(ticketId);
            if (result)
            {
                return JsonResponseStatus.SendStatus(
                    JsonResponseStatusType.Success,
                    "درخواست مورد نطر با موفقیت ثبت شد",
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
    }
}
