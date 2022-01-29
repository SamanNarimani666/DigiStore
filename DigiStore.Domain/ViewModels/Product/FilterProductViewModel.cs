using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Product
{
    public class FilterProductViewModel:BasePaging
    {
        #region Constructor
        public FilterProductViewModel()
        {
            FilterProductOrderBy = FilterProductOrderBy.Create_Date_Desc;

        }
        #endregion

        public string Name { get; set; }
        //public string Category { get; set; }
        public int CategoryId { get; set; }
        public int?  SellerId { get; set; }
        public int Selectedbrands { get; set; }
        public List<Entities.Product> Products { get; set; }
        public List<int> SelectedPrductCategories { get; set; }
        public List<int> Selectedbrand { get; set; }
        public FilterProductState FilterProductState { get; set; }
        public FilterProductOrderBy FilterProductOrderBy { get; set; }
        #region Methods

        public FilterProductViewModel SetProduct(List<Entities.Product> products)
        {
            this.Products = products;
            return this;

        }
        public FilterProductViewModel SetPaging(BasePaging paging)
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
    public enum FilterProductState : byte
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیر فعال")]
        NotActive,
        [Display(Name = "در حال برسی")]
        UnderProgress,
        [Display(Name = "تایید شده")]
        Accepted,
        [Display(Name = "تایید نشده")]
        Rejected
    }

    public enum FilterProductOrderBy
    {
        [Display(Name = "جدید ")]
        Create_Date_Desc,
        [Display(Name = " قدیمی")]
        Create_Date_Asc,
        [Display(Name = "قیمت نزولی")]
        Price_Desc,
        [Display(Name = "قیمت صعودی")]
        Price_Asc
    }
}
