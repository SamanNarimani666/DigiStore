using System.Linq;
using System.Threading.Tasks;
using DigiStore.Data.Context;
using DigiStore.Domain.IRepositories.Productcomment;
using DigiStore.Domain.ViewModels.Paging;
using DigiStore.Domain.ViewModels.ProductComment;
using Microsoft.EntityFrameworkCore;

namespace DigiStore.Data.Repositories.Productcomment
{
    public class ProductcommentRepository: IProductcommentRepository
    {
        #region Constructor
        private readonly DigiStore_DBContext _context;
        public ProductcommentRepository(DigiStore_DBContext context)
        {
            _context = context;
        }
        #endregion

        #region AddProductcomment
        public async Task AddProductcomment(Domain.Entities.Productcomment productcomment)
        {
            await _context.AddAsync(productcomment);
        }
        #endregion

        #region UpdateProductcomment
        void UpdateProductcomment(Domain.Entities.Productcomment productcomment)
        {
            _context.Update(productcomment);
        }
        #endregion

        #region filterFilterProductComment
        public async Task<FilterProductCommentViewModel> filterFilterProductComment(FilterProductCommentViewModel filterProductComment)
        {
            var commment =  _context.Productcomments.Include(p => p.User).Include(p => p.Product).AsQueryable();

            #region Order
            switch (filterProductComment.OrderBydate)
            {
                case OrderBydate.Desc_Date:
                    commment = commment.OrderByDescending(p => p.CreateDate);
                    break;
                case OrderBydate.Desc_Asc:
                    commment = commment.OrderBy(p => p.CreateDate);
                    break;
            }
            #endregion

            #region filter
            if (filterProductComment.UserId != 0 && filterProductComment.UserId != null)
                commment = commment.Where(p => p.UserId == filterProductComment.UserId);
            if (filterProductComment.ProductId != 0 && filterProductComment.ProductId != null)
                commment = commment.Where(p => p.ProductId == filterProductComment.ProductId);
            if (filterProductComment.SellerId != 0 && filterProductComment.SellerId != null)
                commment = commment.Where(p => p.SellerId == filterProductComment.SellerId);
            #endregion

            #region paging
            var pager = Pager.Build(filterProductComment.PageId, await commment.CountAsync(), filterProductComment.TakeEntity,
                filterProductComment.HowManyShowPageAfterAndBefore);
            var allProduct = commment.Paging(pager).ToList();
            return filterProductComment.SetPaging(pager).SetComment(allProduct);

            #endregion

        }
        #endregion

        #region Save
        public async Task Save()
        {
            await _context.SaveChangesAsync();
        }
        #endregion

        #region DisposeAsync
        public async ValueTask DisposeAsync()
        {
            if (_context != null)
            {
                await _context.DisposeAsync();
            }
        }

        void IProductcommentRepository.UpdateProductcomment(Domain.Entities.Productcomment productcomment)
        {
            throw new System.NotImplementedException();
        }
        #endregion
    }
}
