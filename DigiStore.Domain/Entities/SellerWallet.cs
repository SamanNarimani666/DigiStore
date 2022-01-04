using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class SellerWallet
    {
        public int SellerWalletId { get; set; }
        public int SellerId { get; set; }
        public int? Price { get; set; }
        public string Descriptions { get; set; }
        public byte? TransactionType { get; set; }
        public DateTime CreateDate { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Seller Seller { get; set; }
    }
}
