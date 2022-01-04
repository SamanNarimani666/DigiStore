using System;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.SalesOrder
{
    public interface ISalesOrderHeaderRepository:IAsyncDisposable
    {
        Task AddSalesOrderHeader(SalesOrderHeader orderHeader);
        Task<SalesOrderHeader> GetSalesOrderHeaderByUserId(int userId);
        Task<bool> IsOpenSalesOrderHeaderByUser(int userId);
        void UpdateSalesOrderHeader(SalesOrderHeader orderHeader);
        Task Save();
    }
}
