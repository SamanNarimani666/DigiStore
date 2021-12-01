using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class SiteSetting
    {
        public int SiteSettingId { get; set; }
        public string Phone { get; set; }
        public string Email { get; set; }
        public string FooterText { get; set; }
        public string CopyRight { get; set; }
        public string Address { get; set; }
        public bool? IsDefault { get; set; }
        public string AboutUs { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }
    }
}
