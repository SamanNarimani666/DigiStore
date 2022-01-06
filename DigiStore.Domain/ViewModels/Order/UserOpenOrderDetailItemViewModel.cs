namespace DigiStore.Domain.ViewModels.Order
{
    public class UserOpenOrderDetailItemViewModel
    {
        public int ProductId { get; set; }
        public int DetialId { get; set; }

        public string ProductTitle { get; set; }

        public string ProductImageName { get; set; }

        public int? ProductColorId { get; set; }

        public int Qty { get; set; }

        public int ProductPrice { get; set; }

        public int ProductColorPrice { get; set; }

        public string ColorCode { get; set; }

        public int? DiscountPercentage { get; set; }


        public int GetOrderDetailWithDiscountPriceAmount()
        {
            if (this.DiscountPercentage != null)
            {
                return (this.ProductPrice + this.ProductColorPrice) * this.DiscountPercentage.Value / 100 * this.Qty;
            }

            return 0;
        }

        public int GetTotalAmountByDiscount()
        {
            return (ProductPrice + ProductColorPrice) * Qty - this.GetOrderDetailWithDiscountPriceAmount();
        }


        public string GetOrderDetailWithDiscountPrice()
        {
            if (this.DiscountPercentage != null)
            {
                return this.GetOrderDetailWithDiscountPriceAmount().ToString("#,0 تومان");
            }

            return "------";
        }

    }
}
