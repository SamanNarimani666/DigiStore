using System.Collections.Generic;
using System.Linq;
namespace DigiStore.Domain.ViewModels.Paging
{
    public static class PagingExtension
    {
        public static IEnumerable<T> Paging<T>(this IEnumerable<T> query, BasePaging paging)
        {
            return query.Skip(paging.SkipEntity).Take(paging.TakeEntity);
        }
        public static IQueryable<T> Paging<T>(this IQueryable<T> query, BasePaging paging)
        {
            return query.Skip(paging.SkipEntity).Take(paging.TakeEntity);
        }
    }
}
