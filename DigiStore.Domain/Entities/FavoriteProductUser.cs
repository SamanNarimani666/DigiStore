using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class FavoriteProductUser
    {
        public int FavoriteProductUserId { get; set; }
        public int ProductId { get; set; }
        public int UserId { get; set; }
        public bool IsDelete { get; set; }
        public DateTime CreateDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
