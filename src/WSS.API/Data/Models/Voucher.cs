using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Voucher
    {
        public Guid Id { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public double? DiscountValueVoucher { get; set; }
        public double? MinAmount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public Guid? CreateBy { get; set; }

        public virtual Owner? CreateByNavigation { get; set; }
    }
}
