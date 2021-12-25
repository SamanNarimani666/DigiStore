using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.Guarantee
{
    public interface IProductGuaranteeRepository : IAsyncDisposable
    {
        Task AddProductGuarantee(List<Entities.Guarantee> guarantees);
        List<Entities.Guarantee> GetProductGuaranteesByProductId(int productId);
        void DeleteProductGuarantee(List<Entities.Guarantee> guarantees);
        Task Save();
    }

}
