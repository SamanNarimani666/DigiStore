using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductDiscount;

namespace DigiStore.Data.Repositories.ProductDiscount
{
    public class ProductDiscountRepository: IProductDiscountRepository
    {
        #region Contructor
        private readonly DigiStore_DBContext _context;
        public ProductDiscountRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            await _context.DisposeAsync();
        }
        #endregion
    }
}
