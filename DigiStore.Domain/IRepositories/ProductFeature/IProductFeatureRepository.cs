using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.ProductFeature
{
    public interface IProductFeatureRepository : IAsyncDisposable
    {
        Task AddProductFeature(List<Entities.ProductFeature> features);
        List<Entities.ProductFeature> GetProductFeatureByProductId(int productId);
        void RemoveProductFeature(List<Entities.ProductFeature> features);
        Task Save();
    }
}
