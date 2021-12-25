namespace DigiStore.Domain.ViewModels.Product
{
    public class EditProductViewModel : CreateProductViewModel
    {
        public int ProductId { get; set; }
        public string ProductImage { get; set; }
    }
    public enum EditProductResult
    {
        Success,
        Erorr,
        NotFoundUser,
        NotFoundProduct
    }
}
