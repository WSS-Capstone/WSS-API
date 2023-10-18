using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Commission
    {
        public Commission()
        {
            Categories = new HashSet<Category>();
        }

        public Guid Id { get; set; }
        public DateTime? DateOfApply { get; set; }
        public double? CommisionValue { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual ICollection<Category> Categories { get; set; }
    }
}
