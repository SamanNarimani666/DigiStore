using System;
using System.Collections.Generic;
using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.ProductGallery;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.ProductGallery
{
    public class ProductGalleryRepository : IProductGalleryRepository
    {
        #region Constructor

        private readonly DigiStore_DBContext _context;
        public ProductGalleryRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region GetAllProductGallery
        public async Task<List<Domain.Entities.ProductGallery>> GetAllProductGallery(int productId)
        {
            return await _context.ProductGalleries.Where(p => p.ProductId == productId&& p.IsDelete==false).OrderBy(p=>p.DisplayPrority.Value).ToListAsync();
        }
        #endregion

        #region GetAllProductGalleryForSellerpanel
        public async Task<List<Domain.Entities.ProductGallery>> GetAllProductGalleryForSellerpanel(int productId, int sellerId)
        {
            return await _context.ProductGalleries
                .Include(p => p.Product)
                .Where(p => p.ProductId == productId &&  p.Product.SellerId == sellerId)
                .OrderBy(p=>p.DisplayPrority.Value)
                .ToListAsync();
        }
        #endregion

        #region GetProductGalleryByGalleryIdAndSellerId
        public async Task<Domain.Entities.ProductGallery> GetProductGalleryByGalleryIdAndSellerId(int glleryId, int sellerId)
        {
            return await _context.ProductGalleries
                .AsQueryable()
                .Include(p => p.Product)
                .SingleOrDefaultAsync(p=>p.ProductGalleryId== glleryId&& p.Product.SellerId==sellerId);
        }

        #endregion

        #region AddProductGallery
        public async Task AddProductGallery(Domain.Entities.ProductGallery productGallery)
        {
            await _context.ProductGalleries.AddAsync(productGallery);
        }
        #endregion

        #region EditProductGallery
        public void EditProductGallery(Domain.Entities.ProductGallery productGallery)
        {
            productGallery.ModifiedDate=DateTime.Now;
            _context.ProductGalleries.Update(productGallery);
        }
        #endregion

        #region CheackProductGalleryDisplayPrority
        public async Task<bool> CheackProductGalleryDisplayPrority(byte displayPrority,int productId)
        {
            return await _context.ProductGalleries.AnyAsync(
                p =>p.ProductId==productId&& p.DisplayPrority == displayPrority && !p.IsDelete);
        }
        #endregion

        #region CheackProductGalleryDisplayPrority
        public async Task<bool> CheackProductGalleryDisplayProrityForEdit(int galleryId, byte displayPrority, int productId)
        {
            return await _context.ProductGalleries.AnyAsync(
                p =>p.ProductGalleryId!=galleryId&&p.ProductId==productId&& p.DisplayPrority.Value == displayPrority && !p.IsDelete);
        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region Dispose
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }
        #endregion

    }
}
