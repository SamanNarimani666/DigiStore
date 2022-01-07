using Microsoft.AspNetCore.Mvc;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.User;

namespace DigiStore.Web.Areas.Admin.Controllers
{
    public class UserManagementController : AdminBaseController
    {
        #region Constructor
        private readonly IUserService _userService;
        public UserManagementController(IUserService userService)
        {
            _userService = userService;
        }
        #endregion

        #region Constructor
        public async Task<IActionResult> Index(FilterUserViewModel filterUser)
        {
            return View(await _userService.FilterUserTask(filterUser));
        }
        #endregion
    }
}
