using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Productcomment
    {
        public int ProductcommentId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public int SellerId { get; set; }
        public string Title { get; set; }
        public string Comment { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
        public virtual Seller Seller { get; set; }
        public virtual User User { get; set; }
    }
}
