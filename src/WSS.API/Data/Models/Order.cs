using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Order
    {
        public Order()
        {
            OrderDetails = new HashSet<OrderDetail>();
            PartnerPaymentHistories = new HashSet<PartnerPaymentHistory>();
            PaymentHistories = new HashSet<PaymentHistory>();
        }

        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
        public Guid? OwnerId { get; set; }
        public Guid? WeddingInformationId { get; set; }
        public string? Fullname { get; set; }
        public string? Address { get; set; }
        public string? Phone { get; set; }
        public Guid? VoucherId { get; set; }
        public Guid? ComboId { get; set; }
        public double? TotalAmount { get; set; }
        public double? TotalAmountRequest { get; set; }
        public string? Description { get; set; }
        public DateTime? CreateDate { get; set; }
        public int? Status { get; set; }

        public virtual Combo? Combo { get; set; }
        public virtual Customer? Customer { get; set; }
        public virtual Owner? Owner { get; set; }
        public virtual WeddingInformation? WeddingInformation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PartnerPaymentHistory> PartnerPaymentHistories { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
