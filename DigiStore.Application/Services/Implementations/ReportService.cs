using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.IRepositories.Product;
using DigiStore.Domain.IRepositories.SalesOrder;
using DigiStore.Domain.IRepositories.Seller;
using DigiStore.Domain.IRepositories.User;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.Report;

namespace DigiStore.Application.Services.Implementations
{
    public class ReportService:IReportService
    {
        #region Constructor
        private readonly ISalesOrderHeaderRepository _orderHeaderRepository;
        private readonly IUserRepository _userRepository;
        private readonly IProductRepository _productRepository;
        private readonly ISellerRepository _sellerRepository;
        public ReportService(ISalesOrderHeaderRepository orderHeaderRepository, IUserRepository userRepository, IProductRepository productRepository, ISellerRepository sellerRepository)
        {
            _orderHeaderRepository = orderHeaderRepository;
            _userRepository = userRepository;
            _productRepository = productRepository;
            _sellerRepository = sellerRepository;
        }
        #endregion

        #region ProductReport
        public async Task<ProductReportViewModel> ProductReport()
        {
            return new ProductReportViewModel()
            {
                TheBestPopularProduct = await _productRepository.TheBestPopularProduct(),
                TheBestSellingProduct = await _productRepository.TheBestSellingProduct(),
                TheMostProductVisited = await _productRepository.TheMostProductVisited()
            };
        }
        #endregion

        #region ReportForAdminpanel
        public async Task<ReportAdminPanelViewModel> ReportForAdminpanel()
        {
            return new ReportAdminPanelViewModel()
            {
                NumberOfActiveProducts = await _productRepository.NumberOfActiveproduct(),
                NumberOfUser = await _userRepository.NumberOfUsers(),
                NumberofActiveSeller = await _sellerRepository.NumberOfActiveSeller(),
                TotalPurchaseToday = await _orderHeaderRepository.TotalPurchaseToday(),
                NumberOfAllUsers = await _userRepository.NumberOfAllActiveUser(),
                NumberAllProducts = await _productRepository.NumberAllProducts()
            };
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _orderHeaderRepository.DisposeAsync();
        }
        #endregion
      
    }
}
