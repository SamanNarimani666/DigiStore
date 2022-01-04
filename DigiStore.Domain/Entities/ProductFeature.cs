using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductFeature
    {
        public int ProductFeatureId { get; set; }
        public int ProductId { get; set; }
        public string FeatureTitle { get; set; }
        public string FeatureValue { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
    }
}
