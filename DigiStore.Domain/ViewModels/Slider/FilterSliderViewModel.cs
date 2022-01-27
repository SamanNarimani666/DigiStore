using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using DigiStore.Domain.ViewModels.Paging;

namespace DigiStore.Domain.ViewModels.Slider
{
    public class FilterSliderViewModel : BasePaging
    {
        public bool IsDelete { get; set; }
        public List<Entities.Slider> Sliders { get; set; }
        public SliderOrder SliderOrder { set; get; }

        #region Methods
        public FilterSliderViewModel SetSlider(List<Entities.Slider> sliders)
        {
            this.Sliders = sliders;
            return this;
        }
        public FilterSliderViewModel SetPaging(BasePaging paging)
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
    public enum SliderOrder
    {
        [Display(Name = "جدید ")]
        Create_Date_Desc,
        [Display(Name = " قدیمی")]
        Create_Date_Asc
    }
}
