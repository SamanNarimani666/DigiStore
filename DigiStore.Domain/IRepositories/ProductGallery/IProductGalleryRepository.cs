using System;
using System.Collections.Generic;
using System.Threading.Tasks;

namespace DigiStore.Domain.IRepositories.ProductGallery
{
    public interface IProductGalleryRepository:IAsyncDisposable
    {
        Task<List<Entities.ProductGallery>> GetAllProductGallery(int productId);
        Task<List<Entities.ProductGallery>> GetAllProductGalleryForSellerpanel(int productId, int sellerId);
        Task<Entities.ProductGallery> GetProductGalleryByGalleryIdAndSellerId(int glleryId, int sellerId);
        Task AddProductGallery(Entities.ProductGallery productGallery);
        void EditProductGallery(Entities.ProductGallery productGallery);
        Task Save();
    }
}
