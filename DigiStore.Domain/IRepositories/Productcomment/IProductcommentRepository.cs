using System;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.ProductComment;

namespace DigiStore.Domain.IRepositories.Productcomment
{
    public interface IProductcommentRepository : IAsyncDisposable
    {
        Task AddProductcomment(Entities.Productcomment productcomment);
        void UpdateProductcomment(Entities.Productcomment productcomment);
        Task<FilterProductCommentViewModel> filterFilterProductComment(FilterProductCommentViewModel filterProductComment);
        Task Save();
    }
}
