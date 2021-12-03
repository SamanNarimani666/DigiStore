﻿using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Ticket;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.UserPanel.Controllers
{
    [Route("Ticket")]
    public class TicketController : UserBaseController
    {
        #region Constructor
        private readonly ITicketService _ticketService;
        public TicketController(ITicketService ticketService)
        {
            _ticketService = ticketService;
        }
        #endregion

        #region ListTicket
        [HttpGet("ListTicket")]
        public async Task<IActionResult> ListTicket(FilterTicketViewModel filterTicket)
        {
            filterTicket.UserId = User.GetUserId();
            filterTicket.OrderBy = FilterTicketOrder.CreateDate_DES;
            return View(await _ticketService.FilterTickets(filterTicket));
        }
        #endregion

        #region AddTicket
        [HttpGet("add-Ticket")]
        public IActionResult AddTicket()
        {
            return View();
        }
        [HttpPost("add-Ticket")]
        public async Task<IActionResult> AddTicket(AddTicketViewModel addTicket)
        {
            if (ModelState.IsValid)
            {
                var res = await _ticketService.AddUserTicket(addTicket, User.GetUserId());
                switch (res)
                {
                    case AddTicketResult.Error:
                        TempData[ErrorMessage] = "خطا در ثبت تیکت";
                        break;
                    case AddTicketResult.Success:
                        TempData[SuccessMessage] = "تیکت شما با موفقیت ثبت شد";
                        return RedirectToAction("ListTicket", "Ticket", new {area = "UserPanel"});
                }
            }
            return View(addTicket);
        }
        #endregion

        #region TicketDetails
        [HttpGet("tickets/{ticketId}")]
        public async Task<IActionResult> TicketDetail(int ticketId)
        {
            var ticket = await _ticketService.TicketDetailsForShow(ticketId, User.GetUserId());
            if (ticket == null) return NotFound();
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
                var res = await _ticketService.AnswerTicket(answer, User.GetUserId());
                switch (res)
                {
                    case AnswerTicketResult.NotForUser:
                        TempData[ErrorMessage] = "عدم دسترسی";
                        return RedirectToAction("ListTicket");
                    case AnswerTicketResult.NotFound:
                        TempData[WarningMessage] = "اطلاعات مورد نظر یافت نشد";
                        return RedirectToAction("ListTicket");
                    case AnswerTicketResult.Success:
                        TempData[SuccessMessage] = "اطلاعات مورد نظر با موفقیت ثبت شد";
                        break;
                }
            }

            return RedirectToAction("TicketDetail", "Ticket", new { area = "UserPanel", ticketId = answer.TicketId });
        }

        #endregion
    }
}
