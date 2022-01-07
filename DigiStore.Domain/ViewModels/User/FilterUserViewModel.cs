using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.User
{
    public class FilterUserViewModel : BasePaging
    {
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Mobile { get; set; }
        public List<Entities.User> Users { get; set; }
        public UserState UserState { get; set; }
        public FilterUserOrderBy UserOrderBy { get; set; }


        #region Methods

        public FilterUserViewModel SetUser(List<Entities.User> users)
        {
            this.Users = users;
            return this;
        }
        public FilterUserViewModel SetPaging(BasePaging paging)
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

    public enum UserState
    {
        [Display(Name = "همه")]
        All,
        [Display(Name = "حذف شده")]
        IsDelete,
        [Display(Name = "حذف نشده")]
        NotDelete,
        [Display(Name = "فعال")]
        Active,
        [Display(Name = "غیر فعال")]
        NotActive,
        [Display(Name = "بلاک شده")]
        IsBolick,
        [Display(Name = "بلاک نشده")]
        NotBlock
    }
    public enum FilterUserOrderBy
    {
        [Display(Name = "نزولی ")]
        Create_Date_Desc,
        [Display(Name = " صعودی")]
        Create_Date_Asc,
    }
}
