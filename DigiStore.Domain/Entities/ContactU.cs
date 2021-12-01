using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class ContactU
    {
        public int ContactUsid { get; set; }
        public string UserIp { get; set; }
        public string Email { get; set; }
        public string FullName { get; set; }
        public string Subject { get; set; }
        public string Text { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }
    }
}
