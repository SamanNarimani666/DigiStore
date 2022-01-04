using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class SalesOrderDetail
    {
        public int SalesOrderDetailId { get; set; }
        public int SalesOrderId { get; set; }
        public int ProductId { get; set; }
        public int? ColorId { get; set; }
        public int? GuaranteeId { get; set; }
        public int? ColorPrice { get; set; }
        public int? GuaranteePrice { get; set; }
        public int OrderQty { get; set; }
        public int Price { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Color Color { get; set; }
        public virtual Guarantee Guarantee { get; set; }
        public virtual Product Product { get; set; }
        public virtual SalesOrderHeader SalesOrder { get; set; }
    }
}
