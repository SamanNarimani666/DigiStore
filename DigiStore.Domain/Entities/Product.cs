using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Product
    {
        public Product()
        {
            Colors = new HashSet<Color>();
            FavoriteProductUsers = new HashSet<FavoriteProductUser>();
            Guarantees = new HashSet<Guarantee>();
            ProductDiscounts = new HashSet<ProductDiscount>();
            ProductFeatures = new HashSet<ProductFeature>();
            ProductGalleries = new HashSet<ProductGallery>();
            ProductSelectedCategories = new HashSet<ProductSelectedCategory>();
            ProductVisiteds = new HashSet<ProductVisited>();
            Productcomments = new HashSet<Productcomment>();
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public int? BrandId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ProductAcceptOrRejectDescription { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public byte? ProductAcceptanceState { get; set; }
        public string ImageName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }
        public int? SiteProfile { get; set; }

        public virtual Brand Brand { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual ICollection<Color> Colors { get; set; }
        public virtual ICollection<FavoriteProductUser> FavoriteProductUsers { get; set; }
        public virtual ICollection<Guarantee> Guarantees { get; set; }
        public virtual ICollection<ProductDiscount> ProductDiscounts { get; set; }
        public virtual ICollection<ProductFeature> ProductFeatures { get; set; }
        public virtual ICollection<ProductGallery> ProductGalleries { get; set; }
        public virtual ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
        public virtual ICollection<ProductVisited> ProductVisiteds { get; set; }
        public virtual ICollection<Productcomment> Productcomments { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
