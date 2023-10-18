using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class PaymentHistory
    {
        public string? Code { get; set; }
        public Guid Id { get; set; }
        public Guid? OrderId { get; set; }
        public string? PaymentType { get; set; }
        public Guid? RequestUserId { get; set; }
        public double? TotalAmount { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }

        public virtual Order? Order { get; set; }
    }
}
