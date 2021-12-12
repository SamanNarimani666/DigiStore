using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Address
    {
        public int AddressId { get; set; }
        public int UserId { get; set; }
        public string State { get; set; }
        public string City { get; set; }
        public string Zipcode { get; set; }
        public string PostalCode { get; set; }
        public string Unit { get; set; }
        public string Address1 { get; set; }
        public bool IsDelete { get; set; }
        public Guid Rowguid { get; set; }
        public DateTime ModifiedDate { get; set; }

        public virtual User User { get; set; }
    }
}
