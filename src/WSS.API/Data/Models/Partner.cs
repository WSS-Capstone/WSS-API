using System;
using System.Collections.Generic;

namespace WSS.API.Data.Models;

public partial class Partner
{
    public Guid Id { get; set; }

    public string? Fullname { get; set; }

    public DateTime? DataOfBirth { get; set; }

    public string? Phone { get; set; }

    public string? Address { get; set; }

    public string? ImageUrl { get; set; }

    public int? Gender { get; set; }

    public Guid? RoleId { get; set; }

    public virtual Account IdNavigation { get; set; } = null!;

    public virtual ICollection<PartnerPaymentHistory> PartnerPaymentHistories { get; set; } = new List<PartnerPaymentHistory>();

    public virtual PartnerService? PartnerService { get; set; }

    public virtual Role? Role { get; set; }

    public virtual Task? Task { get; set; }
}
