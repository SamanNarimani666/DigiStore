using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Product;
using DigiStore.Domain.ViewModels.Report;

namespace DigiStore.Application.Services.Interfaces
{
    public interface IReportService:IAsyncDisposable
    {
        Task<ProductReportViewModel> ProductReport();
        Task<ReportAdminPanelViewModel> ReportForAdminpanel();
    }
}
