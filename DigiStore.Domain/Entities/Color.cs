using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Color
    {
        public int ColorId { get; set; }
        public int ProductId { get; set; }
        public string ColorName { get; set; }
        public int? Price { get; set; }
        public bool IsDelete { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
    }
}
