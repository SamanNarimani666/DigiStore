using System.Collections.Generic;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Brand
{
    public class FilterBrandViewModel:BasePaging
    {
        public string BrandTitle { get; set; }
        public List<Entities.Brand> Brands { get; set; }

        #region Methods
        public FilterBrandViewModel SetBrand(List<Entities.Brand> brands)
        {
            this.Brands = brands;
            return this;
        }
        public FilterBrandViewModel SetPaging(BasePaging paging)
        {
            this.PageId = paging.PageId;
            this.AllEntitiesCount = paging.AllEntitiesCount;
            this.StartPage = paging.StartPage;
            this.EndPage = paging.EndPage;
            this.PageCount = paging.PageCount;
            this.HowManyShowPageAfterAndBefore = paging.HowManyShowPageAfterAndBefore;
            this.TakeEntity = paging.TakeEntity;
            this.SkipEntity = paging.SkipEntity;
            return this;
        }
        #endregion
    }
}
