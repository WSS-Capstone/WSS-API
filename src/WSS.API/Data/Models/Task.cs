using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Task
    {
        public Guid Id { get; set; }
        public Guid? StaffId { get; set; }
        public Guid? PartnerId { get; set; }
        public Guid? OrderDetailId { get; set; }
        public string? TaskName { get; set; }
        public string? Content { get; set; }
        public DateTime? StartDate { get; set; }
        public DateTime? EndDate { get; set; }
        public string? ImageEvidence { get; set; }
        public int? Status { get; set; }
        public Guid? CreateBy { get; set; }
        public int? QuantityService { get; set; }
        public double? CommissionValue { get; set; }
        public double? TotalPrice { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Owner? CreateByNavigation { get; set; }
        public virtual OrderDetail? OrderDetail { get; set; }
        public virtual Partner? Partner { get; set; }
        public virtual staff? staff { get; set; }
    }
}
