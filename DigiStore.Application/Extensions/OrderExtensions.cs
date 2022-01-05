using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using DigiStore.Domain.ViewModels.Order;

namespace DigiStore.Application.Extensions
{
    public static class OrderExtensions
    {
        public static string GetOrderDetailWithDiscountPrice(this UserOpenOrderDetailItemViewModel detail)
        {
            if (detail.DiscountPercentage != null)
            {
                return ((detail.ProductPrice+ detail.ProductColorPrice)* (detail.DiscountPercentage.Value / 100) * detail.Qty).ToString("#,0 تومان");

              
            }

            return "------";
        }
    }
}
