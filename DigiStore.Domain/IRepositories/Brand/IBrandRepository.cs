using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.Brand
{
    public interface IBrandRepository:IAsyncDisposable
    {
        Task<List<Entities.Brand>> GetAllBrands();
    }
}
