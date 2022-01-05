using System;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Discount
{
    public class CreateProductDiscountViewModel
    {
        public int ProductId { get; set; }

        [Display(Name = "درصد تخفیف")]
        [Range(1, 100,ErrorMessage = "دصد تخفیف باید در بازه 1 تا 100 باشد")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public int Percentage { get; set; }

        [Display(Name = "تاریخ انقضا")]
        [Required(ErrorMessage = "لطفا {0} را وارد کنید")]
        public string ExpierDate { get; set; }

        public int DiscountNumber { get; set; }
    }

    public enum CreateProductDiscountResult
    {
        Success,
        NotFoundForSeller,
        NotFoundProduct,
        Error
    }
}

