using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.Permission
{
    public interface IPermissionRepository:IAsyncDisposable
    {
        Task<List<Domain.Entities.Permission>> GetAllPermission();
        
    }
}
