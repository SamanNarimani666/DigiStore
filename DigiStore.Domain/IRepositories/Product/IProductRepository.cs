using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Product;

namespace DigiStore.Domain.IRepositories.Product
{
    public interface IProductRepository : IAsyncDisposable
    {
        Task<FilterProductViewModel> FilterProduct(FilterProductViewModel filterProduct);
    }
}
