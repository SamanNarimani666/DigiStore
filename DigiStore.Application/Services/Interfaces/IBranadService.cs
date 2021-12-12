using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Application.Services.Interfaces
{
   public interface IBranadService:IAsyncDisposable
    {
        Task<List<Domain.Entities.Brand>> GetAllBrands();
    }
}
