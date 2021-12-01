using DigiStore.Web.Models;
using Microsoft.AspNetCore.Mvc;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.Linq;
using System.Threading.Tasks;

namespace DigiStore.Web.Controllers
{
    public class HomeController : SiteBaseController
    {
        public IActionResult Index()
        {
           
            return View();
        }

        public IActionResult Error404()
        {
            return View();
        }
    }
}
