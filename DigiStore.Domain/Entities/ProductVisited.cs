using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductVisited
    {
        public int VisiteId { get; set; }
        public int ProductId { get; set; }
        public int? UserId { get; set; }
        public string UserIp { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
        public virtual User User { get; set; }
    }
}
