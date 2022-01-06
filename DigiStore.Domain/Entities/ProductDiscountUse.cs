using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductDiscountUse
    {
        public int ProductDiscountUseId { get; set; }
        public int ProductDiscountId { get; set; }
        public int UserId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual ProductDiscount ProductDiscount { get; set; }
        public virtual User User { get; set; }
    }
}
