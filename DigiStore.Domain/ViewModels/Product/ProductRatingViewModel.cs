using System;
using System.ComponentModel.DataAnnotations;

namespace DigiStore.Domain.ViewModels.Product
{
    public class ProductRatingViewModel
    {
        public int ProductId { get; set; }
        [Display(Name = "کیفیت ساخت")] 
        public int ConstructionQuality { get; set; } = 0;
        [Display(Name = "ارزش خرید نسبت به قیمت")]
        public int PurchaseValueRelativeToPrice { get; set; } = 0;
        [Display(Name = "نوآوری")]
        public int Innovation { get; set; } = 0;
        [Display(Name = "ویژگی ها و قابلیت ها")]
        public int FeaturesAndCapabilities { get; set; } = 0;
        [Display(Name = "سهولت استفاده")]
        public int EaseOfUse { get; set; } = 0;
        [Display(Name = "طراحی و ظاهر")]
        public int DesignAndAppearance { get; set; } = 0;

        public int CountRating { get; set; }

        public  int ProductRatingSum()
        {
            return ConstructionQuality + PurchaseValueRelativeToPrice + Innovation + FeaturesAndCapabilities +
                           EaseOfUse + DesignAndAppearance;
        }

        public double ProductRatingAverage()
        {
            var RatingSum = ProductRatingSum();
            if (RatingSum >= 0)
            {
                return RatingSum / 6;
            }

            return 0;
        }

    }
}
