namespace DigiStore.Domain.ViewModels.Order
{
    public class AddProductToOrderViewModel
    {
        public int ProductId { get; set; }
        public int? ProductColorId { get; set; }
        public int OrderQty { get; set; }
    }
}
