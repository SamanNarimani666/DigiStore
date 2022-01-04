using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class SalesOrderHeader
    {
        public SalesOrderHeader()
        {
            SalesOrderDetails = new HashSet<SalesOrderDetail>();
        }

        public int SalesOrderId { get; set; }
        public int UserId { get; set; }
        public bool IsPaiy { get; set; }
        public int OrderSum { get; set; }
        public string TracingCode { get; set; }
        public DateTime? PaymentDate { get; set; }
        public string Descriptions { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User User { get; set; }
        public virtual ICollection<SalesOrderDetail> SalesOrderDetails { get; set; }
    }
}
