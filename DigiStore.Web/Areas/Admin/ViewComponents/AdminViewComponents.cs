using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.Ticket;
using DigiStore.Domain.ViewModels.User;
using DigiStore.Web.PresentationExtensions;
using Microsoft.AspNetCore.Mvc;

namespace DigiStore.Web.Areas.Admin.ViewComponents
{
    #region AdminUserInformation
    public class AdminUserInformation : ViewComponent
    {
        private readonly IUserService _userService;

        public AdminUserInformation(IUserService userService)
        {
            _userService = userService;
        }

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("AdminUserInformation", await _userService.GetInformationUserForSidebarById(User.GetUserId()));
        }
    }
    #endregion

    #region SidebarMenu
    public class SidebarMenu : ViewComponent
    {
        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("SidebarMenu");
        }
    }
    #endregion

    #region ProductlistViewCompnnet
    public class ProductsUnderProgress : ViewComponent
    {
        #region Constructor
        private readonly IProductService _productService;
        public ProductsUnderProgress(IProductService productService)
        {
            _productService = productService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterProductViewModel()
            {
                FilterProductState = FilterProductState.Accepted,
                FilterProductOrderBy = FilterProductOrderBy.Create_Date_Desc,
                TakeEntity = 10
            };
            return View("ProductsUnderProgress", await _productService.FilterProduct(filter));
        }
    }
    #endregion

    #region ProductReport

    public class ProductReport : ViewComponent
    {
        #region Constructor

        private readonly IReportService _reportService;
        public ProductReport(IReportService reportService)
        {
            _reportService = reportService;
        }
        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("ProductReport", await _reportService.ProductReport());
        }
    }
    #endregion

    #region LastUserRegister
    public class LastUserRegister : ViewComponent
    {
        #region Constructor
        private readonly IUserService _userService;
        public LastUserRegister(IUserService userService)
        {
            _userService = userService;
        }
        #endregion
        public async Task<IViewComponentResult> InvokeAsync()
        {
            var filter = new FilterUserViewModel()
            {
                UserOrderBy = FilterUserOrderBy.Create_Date_Desc,
            };
            return View("LastUserRegister",await _userService.FilterUser(filter));
        }
    }
    #endregion

    #region HeaderReportAdmin
    public class ReportHeaderAdmin : ViewComponent
    {
        #region Constructor
        private readonly IReportService _reportService;

        public ReportHeaderAdmin(IReportService reportService)
        {
            _reportService = reportService;
        }

        #endregion

        public async Task<IViewComponentResult> InvokeAsync()
        {
            return View("ReportHeaderAdmin",await _reportService.ReportForAdminpanel());
        }
    }
    #endregion
}
