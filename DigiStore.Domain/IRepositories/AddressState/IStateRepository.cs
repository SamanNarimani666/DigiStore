using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using DigiStore.Domain.Entities;


namespace DigiStore.Domain.IRepositories.AddressState
{
    public interface IStateRepository : IAsyncDisposable
    {
        Task<List<State>> GetAllState();
        Task<State> GetStateById(int stateId);
    }
}
