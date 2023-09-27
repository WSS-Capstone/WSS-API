using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class CurrentPrice
    {
        public Guid Id { get; set; }
        public DateTime? DateOfApply { get; set; }
        public Guid? ServiceId { get; set; }
        public double? Price { get; set; }

        public virtual Service? Service { get; set; }
    }
}
