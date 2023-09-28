using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models
{
    public partial class Partner
    {
        public Partner()
        {
            PartnerPaymentHistories = new HashSet<PartnerPaymentHistory>();
        }

        public Guid Id { get; set; }
        public string? Fullname { get; set; }
        public DateTime? DateOfBirth { get; set; }
        public string? Phone { get; set; }
        public string? Address { get; set; }
        public string? ImageUrl { get; set; }
        public int? Gender { get; set; }
        public Guid? RoleId { get; set; }

        public virtual Account IdNavigation { get; set; } = null!;
        public virtual Role? Role { get; set; }
        public virtual PartnerService? PartnerService { get; set; }
        public virtual Task? Task { get; set; }
        public virtual ICollection<PartnerPaymentHistory> PartnerPaymentHistories { get; set; }
    }
}
