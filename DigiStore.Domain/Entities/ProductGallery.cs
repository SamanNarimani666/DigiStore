using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ProductGallery
    {
        public int ProductGalleryId { get; set; }
        public int ProductId { get; set; }
        public string ImageName { get; set; }
        public byte? DisplayPrority { get; set; }
        public bool IsDelete { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
    }
}
