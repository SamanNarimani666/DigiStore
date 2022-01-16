using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductCategory
    {
        public ProductCategory()
        {
            InverseParent = new HashSet<ProductCategory>();
            ProductSelectedCategories = new HashSet<ProductSelectedCategory>();
        }

        public int ProductCategoryId { get; set; }
        public int? ParentId { get; set; }
        public string Title { get; set; }
        public string UrlName { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }
        public string IconName { get; set; }

        public virtual ProductCategory Parent { get; set; }
        public virtual ICollection<ProductCategory> InverseParent { get; set; }
        public virtual ICollection<ProductSelectedCategory> ProductSelectedCategories { get; set; }
    }
}
