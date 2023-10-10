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
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Service? Service { get; set; }
    }
}
