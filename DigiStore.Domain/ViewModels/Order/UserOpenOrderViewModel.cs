using System.Collections.Generic;
using System.Linq;

namespace DigiStore.Domain.ViewModels.Order
{
    public class UserOpenOrderViewModel
    {
        public long UserId { get; set; }

        public string Description { get; set; }

        public List<UserOpenOrderDetailItemViewModel> Details { get; set; }

        public int GetTotalPrice()
        {
            return Details.Sum(s => (s.ProductPrice + s.ProductColorPrice) * s.Qty);
        }

        public int GetTotalDiscounts()
        {
            return Details.Sum(s => s.GetOrderDetailWithDiscountPriceAmount());
        }


    }
}
