using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Application.Services.Interfaces;
using DigiStore.Domain.Entities;
using DigiStore.Domain.IRepositories.Brand;

namespace DigiStore.Application.Services.Implementations
{
    public class BranadService : IBranadService
    {
        #region Constructor
        private readonly IBrandRepository _brandRepository;
        public BranadService(IBrandRepository brandRepository)
        {
            _brandRepository = brandRepository;
        }
        #endregion

        #region GetAllBrands
        public async Task<List<Brand>> GetAllBrands()
        {
            return await _brandRepository.GetAllBrands();
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _brandRepository.DisposeAsync();
        }
        #endregion
    }
}
