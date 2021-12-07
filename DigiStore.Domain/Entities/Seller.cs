using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Seller
    {
        public int SellerId { get; set; }
        public int UserId { get; set; }
        public string StoreName { get; set; }
        public string Phone { get; set; }
        public string Mobile { get; set; }
        public string Address { get; set; }
        public string Descriptions { get; set; }
        public string AdminDescription { get; set; }
        public bool IsDelete { get; set; }
        public string Logo { get; set; }
        public string StoreDoucument { get; set; }
        public byte StoreaceptanceState { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual User User { get; set; }
    }
}
