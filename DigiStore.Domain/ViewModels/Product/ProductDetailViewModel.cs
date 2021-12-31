using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;

using DigiStore.Domain.Entities;

namespace DigiStore.Domain.ViewModels.Product
{
    public class ProductDetailViewModel
    {
        public int SellerId { get; set; }
        public int ProductId { get; set; }


        [Display(Name = "نام محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string Title { get; set; }

        [Display(Name = "تصویر محصول")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(300, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ImageName { get; set; }

        [Display(Name = "قیمت محصول")]
        public int Price { get; set; }

        [Display(Name = "توضیحات کوتاه")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        [MaxLength(500, ErrorMessage = "{0} نمی تواند بیشتر از {1} کاراکتر باشد")]
        public string ShortDescription { get; set; }

        [Display(Name = "توضیحات اصلی")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string Description { get; set; }

        public Entities.Seller Seller { get; set; }

        public List<ProductGallery> ProductGalleries { get; set; }

        public List<Color> ProductColors { get; set; }

        public List<ProductCategory> ProductCategories { get; set; }
        public List<Guarantee> Guarantees { get; set; }
    }
}
