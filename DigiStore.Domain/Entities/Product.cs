using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Product
    {
        public int ProductId { get; set; }
        public int SellerId { get; set; }
        public string Name { get; set; }
        public int Price { get; set; }
        public string ShortDescription { get; set; }
        public string Description { get; set; }
        public string ProductAcceptOrRejectDescription { get; set; }
        public bool IsDelete { get; set; }
        public bool IsActive { get; set; }
        public byte? ProductAcceptanceState { get; set; }
        public string ImageName { get; set; }
        public DateTime CreatedDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Seller Seller { get; set; }
    }
}
