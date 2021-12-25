using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;

namespace DigiStore.Domain.IRepositories.ProductColor
{
    public interface IProductColorRepository:IAsyncDisposable
    {
        Task AddColor(List<Color> colors);
        List<Color> GetColorProductByProductId(int productId);
        void DeleteProductColor(List<Color> colors);
        Task Save();
    }
}
