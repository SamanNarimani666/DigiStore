using System;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.SalesOrderDetail
{
    public interface ISalesOrderDetailRepository:IAsyncDisposable
    {
        Task AddOrderDetail(Entities.SalesOrderDetail orderDetail);
        void UpdateSalesOrderDetail(Entities.SalesOrderDetail orderDetail);
        void DeleteSalesOrderDetail(Entities.SalesOrderDetail orderDetail);
        Task Save();
    }
}
