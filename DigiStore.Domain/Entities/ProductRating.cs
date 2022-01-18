using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductRating
    {
        public int ProductRatingId { get; set; }
        public int ProductId { get; set; }
        public byte ConstructionQuality { get; set; }
        public byte PurchaseValueRelativeToPrice { get; set; }
        public byte Innovation { get; set; }
        public byte FeaturesAndCapabilities { get; set; }
        public byte EaseOfUse { get; set; }
        public byte DesignAndAppearance { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
    }
}
