using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Voucher
    {
        public Voucher()
        {
            Orders = new HashSet<Order>();
        }

        public Guid Id { get; set; }
        public string? Code { get; set; }
        public string? Name { get; set; }
        public string? ImageUrl { get; set; }
        public double? DiscountValueVoucher { get; set; }
        public double MinAmount { get; set; }
        public DateTime? CreateDate { get; set; }
        public DateTime? EndTime { get; set; }
        public DateTime? StartTime { get; set; }
        public Guid? CreateBy { get; set; }
        public Guid? UpdateBy { get; set; }
        public DateTime? UpdateDate { get; set; }

        public virtual User? CreateByNavigation { get; set; }
        public virtual ICollection<Order> Orders { get; set; }
    }
}
