namespace DigiStore.Domain.ViewModels.Product
{
    public class ProductReportViewModel
    {
        public Entities.Product TheMostProductVisited { get; set; }
        public Entities.Product TheBestSellingProduct { get; set; }
        public Entities.Product TheBestPopularProduct { get; set; }

    }
}
