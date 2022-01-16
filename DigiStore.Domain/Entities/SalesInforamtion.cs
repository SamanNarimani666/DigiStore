using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class SalesInforamtion
    {
        public int SalesInforamtionId { get; set; }
        public int UserId { get; set; }
        public int SalesOrderId { get; set; }
        public int AddressId { get; set; }
        public string ReceiverName { get; set; }
        public string ReceiverMobile { get; set; }
        public string ReceiverNaationalId { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Address Address { get; set; }
        public virtual SalesOrderHeader SalesOrder { get; set; }
        public virtual User User { get; set; }
    }
}
