using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductDiscount
    {
        public int ProductDiscountId { get; set; }
        public int ProductId { get; set; }
        public int Percentage { get; set; }
        public DateTime ExpierDate { get; set; }
        public int? DiscountNumber { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
    }
}
