using System;
using System.Collections.Generic;

#nullable disable

namespace DigiStore.Domain.Entities
{
    public partial class StrengthsAndWeakness
    {
        public int StrengthsAndWeaknessesId { get; set; }
        public int ProductId { get; set; }
        public string Strengths { get; set; }
        public string WeakPoints { get; set; }
        public DateTime ModifiedDate { get; set; }
        public Guid Rowguid { get; set; }

        public virtual Product Product { get; set; }
    }
}
