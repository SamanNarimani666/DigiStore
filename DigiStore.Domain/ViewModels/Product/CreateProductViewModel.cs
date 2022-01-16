using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;



namespace DigiStore.Domain.ViewModels.Product
{
    public class CreateProductViewModel
    {
        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "قیمت محصول")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Range(1, 100, ErrorMessage = "دصد تخفیف باید در بازه 1 تا 100 باشد")]
        public int SiteProfile { get; set; }


        [Display(Name = "فعال / غیرفعال")]
        public bool IsActive { get; set; }
        [Display(Name = "برند")]
        public int? BradnId { get; set; }
        public List<CreateProductColorViewModel> ProductColors { get; set; }
        public List<int> SelectedCategories { get; set; }
        public List<CreateProductFeatureViewModel> ProductFeature { get; set; }


    }

    public enum CreateProductResult
    {
        Success,
        Error,
        HasNoImage
    }
}
