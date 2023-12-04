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

        public string? Code { get; set; }
        public Guid Id { get; set; }
        public Guid? CustomerId { get; set; }
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
        public int? StatusOrder { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }
        public int? StatusPayment { get; set; }
        public string? Reason { get; set; }

        public virtual Combo? Combo { get; set; }
        public virtual User? Customer { get; set; }
        public virtual Voucher? Voucher { get; set; }
        public virtual WeddingInformation? WeddingInformation { get; set; }
        public virtual ICollection<OrderDetail> OrderDetails { get; set; }
        public virtual ICollection<PartnerPaymentHistory> PartnerPaymentHistories { get; set; }
        public virtual ICollection<PaymentHistory> PaymentHistories { get; set; }
    }
}
