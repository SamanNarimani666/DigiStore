using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Order;

namespace DigiStore.Domain.IRepositories.SalesOrder
{
    public interface ISalesOrderHeaderRepository:IAsyncDisposable
    {
        Task AddSalesOrderHeader(SalesOrderHeader orderHeader);
        Task<SalesOrderHeader> GetSalesOrderHeaderByUserId(int userId);
        Task<bool> IsOpenSalesOrderHeaderByUser(int userId);
        void UpdateSalesOrderHeader(SalesOrderHeader orderHeader);
        Task<FilterOrderViewModel> FilterOrder(FilterOrderViewModel filterOrder);
        Task<int> TotalPurchaseToday();
        Task Save();
    }
}
