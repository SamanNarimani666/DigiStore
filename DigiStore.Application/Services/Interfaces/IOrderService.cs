using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Order;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IOrderService:IAsyncDisposable
    {
        Task<int> AddOrderForUser(int userId);
        Task<SalesOrderHeader> GetUserLastOpenOrder(int userId);
        Task AddProductToOpenOrder(int userId,AddProductToOrderViewModel addProductToOrder);
        Task<UserOpenOrderViewModel> GetUserOpenOrderDetials(int userId);
        Task<bool> RemoveOrderDetial(int detialId,int userId);
        Task ChangeOrderQty(int detialId,int userId, int qty);
        Task<int> GetTotalOrderPriceForPayment(int userId);
        Task PayOrderProductPriceToSeller(int userId, long trackingCode);
        Task<CreateOrderInforamtionViewModel> GetOpenOrderUserForAddInformation(int orderId,int userId);
        Task<CreateOrderInforamtionResult> CreateOrderInforamtion(CreateOrderInforamtionViewModel createOrderInforamtion,int userId);
        Task<FilterOrderViewModel> FilterOrder(FilterOrderViewModel filterOrder);
        Task<SalesOrderDetail> GetSalesOrderDetialByOrderId(int orderId,int userId);
    }
}
