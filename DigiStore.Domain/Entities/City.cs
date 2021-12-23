using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class City
    {
        public City()
        {
            Addresses = new HashSet<Address>();
        }

        public int CityId { get; set; }
        public int StateId { get; set; }
        public string CityName { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual State State { get; set; }
        public virtual ICollection<Address> Addresses { get; set; }
    }
}
