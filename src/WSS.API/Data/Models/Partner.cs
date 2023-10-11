using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Partner
    {
        public Partner()
        {
            Commissions = new HashSet<Commission>();
            PartnerPaymentHistories = new HashSet<PartnerPaymentHistory>();
        }

        public Guid Id { get; set; }
        public string? Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int? Gender { get; set; }
        public Guid? CategoryId { get; set; }
        public DateTime? CreateDate { get; set; }
        public Guid? CreateBy { get; set; }
        public DateTime? UpdateDate { get; set; }
        public Guid? UpdateBy { get; set; }

        public virtual Category? Category { get; set; }
        public virtual Account IdNavigation { get; set; } = null!;
        public virtual PartnerService? PartnerService { get; set; }
        public virtual Task? Task { get; set; }
        public virtual ICollection<Commission> Commissions { get; set; }
        public virtual ICollection<PartnerPaymentHistory> PartnerPaymentHistories { get; set; }
    }
}
