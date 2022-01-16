using System.Collections.Generic;
using DigiStore.Domain.Entities;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels
{
    public class FilterRoleViewModel:BasePaging
    {
        public string RoleTitle { get; set; }
        public List<Role> Roles { get; set; }

        #region Methods
        public FilterRoleViewModel SetRoles(List<Entities.Role> roles)
        {
            this.Roles = roles;
            return this;
        }
        public FilterRoleViewModel SetPaging(BasePaging paging)
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
