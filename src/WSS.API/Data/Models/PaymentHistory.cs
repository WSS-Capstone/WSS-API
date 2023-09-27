using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class PaymentHistory
    {
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public string? PaymentType { get; set; }
        public Guid? RequestUserid { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime? CreateDate { get; set; }

        public virtual Order? Order { get; set; }
    }
}
