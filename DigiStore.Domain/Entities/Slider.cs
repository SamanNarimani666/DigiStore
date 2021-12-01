using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class Slider
    {
        public int SliderId { get; set; }
        public string ImageName { get; set; }
        public string Link { get; set; }
        public bool? IsActive { get; set; }
        public DateTime CreatedDate { get; set; }
        public Guid Rowguid { get; set; }
    }
}
