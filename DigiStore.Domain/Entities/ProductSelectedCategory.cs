using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductSelectedCategory
    {
        public int ProductSelectedCategoryId { get; set; }
        public int? ProductId { get; set; }
        public int? ProductCategoryId { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
        public virtual ProductCategory ProductCategory { get; set; }
    }
}
