using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Commission
    {
        public Guid Id { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? DateOfApply { get; set; }
        public double? CommisionValue { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Category? Category { get; set; }
    }
}
